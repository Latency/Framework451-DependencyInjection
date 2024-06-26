// ****************************************************************************
// Project:  Microsoft.Extensions.DependencyInjection
// File:     CallSiteRuntimeResolver.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

using System.Diagnostics;
using System.Runtime.ExceptionServices;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup;

internal class CallSiteRuntimeResolver : CallSiteVisitor<ServiceProvider, object?>
{
    public object? Resolve(IServiceCallSite callSite, ServiceProvider provider) => VisitCallSite(callSite, provider);

    protected override object? VisitTransient(TransientCallSite transientCallSite, ServiceProvider provider) => provider.CaptureDisposable(VisitCallSite(transientCallSite.Service, provider));

    protected override object? VisitConstructor(ConstructorCallSite constructorCallSite, ServiceProvider provider)
    {
        object?[]? parameterValues = null;
        if (constructorCallSite.ParameterCallSites is not null)
        {
            parameterValues = new object?[constructorCallSite.ParameterCallSites.Length];

            for (var index = 0; index < parameterValues.Length; index++)
                parameterValues[index] = VisitCallSite(constructorCallSite.ParameterCallSites[index], provider);
        }

        try
        {
            return constructorCallSite.ConstructorInfo.Invoke(parameterValues);
        }
        catch (Exception ex) when (ex.InnerException != null)
        {
            ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
            // The above line will always throw, but the compiler requires we throw explicitly.
            throw;
        }
    }

    protected override object? VisitSingleton(SingletonCallSite singletonCallSite, ServiceProvider provider) => VisitScoped(singletonCallSite, provider.Root);

    protected override object? VisitScoped(ScopedCallSite scopedCallSite, ServiceProvider provider)
    {
        object? resolved;
        lock (provider.ResolvedServices)
        {
            if (!provider.ResolvedServices.TryGetValue(scopedCallSite.Key, out resolved))
            {
                resolved = VisitCallSite(scopedCallSite.ServiceCallSite, provider);
                provider.ResolvedServices.Add(scopedCallSite.Key, resolved);
            }
        }

        return resolved;
    }

    protected override object? VisitConstant(ConstantCallSite constantCallSite, ServiceProvider provider) => constantCallSite.DefaultValue;

    protected override object? VisitCreateInstance(CreateInstanceCallSite createInstanceCallSite, ServiceProvider provider)
    {
        try
        {
            Debug.Assert(createInstanceCallSite.Descriptor.ImplementationType != null);
            return Activator.CreateInstance(createInstanceCallSite.Descriptor.ImplementationType!);
        }
        catch (Exception ex) when (ex.InnerException != null)
        {
            ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
            // The above line will always throw, but the compiler requires we throw explicitly.
            throw;
        }
    }

    protected override object? VisitInstanceService(InstanceService instanceCallSite, ServiceProvider provider) => instanceCallSite.Descriptor.ImplementationInstance;

    protected override object VisitServiceProviderService(ServiceProviderService serviceProviderService, ServiceProvider provider) => provider;

    protected override object VisitEmptyIEnumerable(EmptyIEnumerableCallSite emptyIEnumerableCallSite, ServiceProvider provider) => emptyIEnumerableCallSite.ServiceInstance;

    protected override object VisitServiceScopeService(ServiceScopeService serviceScopeService, ServiceProvider provider) => new ServiceScopeFactory(provider);

    protected override object VisitClosedIEnumerable(ClosedIEnumerableCallSite closedIEnumerableCallSite, ServiceProvider provider)
    {
        Debug.Assert(closedIEnumerableCallSite.ItemType != null);
        var array = Array.CreateInstance(closedIEnumerableCallSite.ItemType!, closedIEnumerableCallSite.ServiceCallSites.Length);

        for (var index = 0; index < closedIEnumerableCallSite.ServiceCallSites.Length; index++)
        {
            var value = VisitCallSite(closedIEnumerableCallSite.ServiceCallSites[index], provider);
            array.SetValue(value, index);
        }

        return array;
    }

    protected override object? VisitFactoryService(FactoryService factoryService, ServiceProvider provider) => factoryService.Descriptor.ImplementationFactory?.Invoke(provider);
}