// ****************************************************************************
// Project:  Microsoft.Extensions.DependencyInjection
// File:     ServiceProvider.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Abstractions;
using Microsoft.Extensions.DependencyInjection.Internal;
using Microsoft.Extensions.DependencyInjection.Properties;
using Microsoft.Extensions.DependencyInjection.ServiceLookup;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
///     The default IServiceProvider.
/// </summary>
internal class ServiceProvider : IServiceProvider, IDisposable
{
    private static readonly Func<Type, ServiceProvider, Func<ServiceProvider, object?>> CcreateServiceAccessor = CreateServiceAccessor;

    // CallSiteRuntimeResolver is stateless so can be shared between all instances
    private static readonly CallSiteRuntimeResolver        CallSiteRuntimeResolver = new();
    private readonly        CallSiteValidator?             _callSiteValidator;
    private readonly        ServiceTable                   _table;
    private                 bool                           _disposeCalled;
    private                 List<IDisposable>?             _transientDisposables;

    public ServiceProvider(IEnumerable<ServiceDescriptor> serviceDescriptors, bool validateScopes)
    {
        Root = this;

        if (validateScopes)
            _callSiteValidator = new();

        _table = new(serviceDescriptors);

        _table.Add(typeof(IServiceProvider),     new ServiceProviderService());
        _table.Add(typeof(IServiceScopeFactory), new ServiceScopeService());
        _table.Add(typeof(IEnumerable<>),        new OpenIEnumerableService(_table));
    }

    // This constructor is called exclusively to create a child scope from the parent
    internal ServiceProvider(ServiceProvider parent)
    {
        Root               = parent.Root;
        _table             = parent._table;
        _callSiteValidator = parent._callSiteValidator;
    }

    internal ServiceProvider             Root             { get; }
    internal Dictionary<object, object?> ResolvedServices { get; } = [];

    public void Dispose()
    {
        lock (ResolvedServices)
        {
            if (_disposeCalled)
                return;
            _disposeCalled = true;
            if (_transientDisposables != null)
            {
                foreach (var disposable in _transientDisposables)
                    disposable.Dispose();

                _transientDisposables.Clear();
            }

            // PERF: We've enumerating the dictionary so that we don't allocate to enumerate.
            // .Values allocates a ValueCollection on the heap, enumerating the dictionary allocates
            // a struct enumerator
            foreach (var entry in ResolvedServices)
                (entry.Value as IDisposable)?.Dispose();

            ResolvedServices.Clear();
        }
    }

    /// <summary>
    ///     Gets the service object of the specified type.
    /// </summary>
    /// <param name="serviceType"></param>
    /// <returns></returns>
    public object? GetService(Type serviceType)
    {
        var realizedService = _table.RealizedServices.GetOrAdd(serviceType, CcreateServiceAccessor, this);

        _callSiteValidator?.ValidateResolution(serviceType, this);

        return realizedService.Invoke(this);
    }

    private static Func<ServiceProvider, object?> CreateServiceAccessor(Type serviceType, ServiceProvider serviceProvider)
    {
        var callSite = serviceProvider.GetServiceCallSite(serviceType, new HashSet<Type?>());
        if (callSite != null)
        {
            serviceProvider._callSiteValidator?.ValidateCallSite(serviceType, callSite);
            return RealizeService(serviceProvider._table, serviceType, callSite);
        }

        return _ => null;
    }

    internal static Func<ServiceProvider, object?> RealizeService(ServiceTable table, Type serviceType, IServiceCallSite callSite)
    {
        var callCount = 0;
        return provider =>
        {
            if (Interlocked.Increment(ref callCount) == 2)
                Task.Run(() =>
                    {
                        var realizedService = new CallSiteExpressionBuilder(CallSiteRuntimeResolver).Build(callSite);
                        table.RealizedServices[serviceType] = realizedService;
                    }
                );

            return CallSiteRuntimeResolver.Resolve(callSite, provider);
        };
    }

    internal IServiceCallSite? GetServiceCallSite(Type serviceType, ISet<Type?> callSiteChain)
    {
        try
        {
            if (!callSiteChain.Add(serviceType))
                throw new InvalidOperationException(Resources.FormatCircularDependencyException(serviceType));

            if (_table.TryGetEntry(serviceType, out var entry))
                return GetResolveCallSite(entry.Last, callSiteChain);

            var emptyIEnumerableOrNull = GetEmptyIEnumerableOrNull(serviceType);
            return emptyIEnumerableOrNull != null ? new EmptyIEnumerableCallSite(serviceType, emptyIEnumerableOrNull) : null;
        }
        finally
        {
            callSiteChain.Remove(serviceType);
        }
    }

    internal IServiceCallSite GetResolveCallSite(IService service, ISet<Type?> callSiteChain)
    {
        var serviceCallSite = service.CreateCallSite(this, callSiteChain);

        // Instance services do not need caching/disposing
        if (serviceCallSite is InstanceService)
            return serviceCallSite;

        if (service.Lifetime == ServiceLifetime.Transient)
            return new TransientCallSite(serviceCallSite);

        if (service.Lifetime == ServiceLifetime.Scoped)
            return new ScopedCallSite(service, serviceCallSite);

        return new SingletonCallSite(service, serviceCallSite);
    }

    internal object? CaptureDisposable(object? service)
    {
        if (!ReferenceEquals(this, service))
            if (service is IDisposable disposable)
                lock (ResolvedServices)
                {
                    _transientDisposables ??= [];

                    _transientDisposables.Add(disposable);
                }

        return service;
    }

    private static object? GetEmptyIEnumerableOrNull(Type serviceType)
    {
        var typeInfo = serviceType.GetTypeInfo();

        if (typeInfo.IsGenericType && serviceType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
        {
            var itemType = typeInfo.GenericTypeArguments[0];
            return Array.CreateInstance(itemType, 0);
        }

        return null;
    }
}