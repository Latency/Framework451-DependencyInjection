// ****************************************************************************
// Project:  Microsoft.Extensions.DependencyInjection
// File:     CallSiteValidator.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection.Abstractions;
using Microsoft.Extensions.DependencyInjection.Properties;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup;

internal class CallSiteValidator : CallSiteVisitor<CallSiteValidator.CallSiteValidatorState, Type?>
{
    // Keys are services being resolved via GetService, values - first scoped service in their call site tree
    private readonly ConcurrentDictionary<Type, Type> _scopedServices = new();

    public void ValidateCallSite(Type serviceType, IServiceCallSite callSite)
    {
        var scoped = VisitCallSite(callSite, default);
        if (scoped != null)
            _scopedServices[serviceType] = scoped;
    }

    public void ValidateResolution(Type serviceType, ServiceProvider serviceProvider)
    {
        if (ReferenceEquals(serviceProvider, serviceProvider.Root) && _scopedServices.TryGetValue(serviceType, out var scopedService))
        {
            if (serviceType == scopedService)
                throw new InvalidOperationException(Resources.FormatDirectScopedResolvedFromRootException(serviceType, nameof(ServiceLifetime.Scoped).ToLowerInvariant()));

            throw new InvalidOperationException(Resources.FormatScopedResolvedFromRootException(serviceType, scopedService, nameof(ServiceLifetime.Scoped).ToLowerInvariant()));
        }
    }

    protected override Type? VisitTransient(TransientCallSite transientCallSite, CallSiteValidatorState state) => VisitCallSite(transientCallSite.Service, state);

    protected override Type? VisitConstructor(ConstructorCallSite constructorCallSite, CallSiteValidatorState state) => constructorCallSite.ParameterCallSites.Select(parameterCallSite => VisitCallSite(parameterCallSite, state)).Aggregate<Type?, Type?>(null, (current, scoped) => current ?? scoped);

    protected override Type? VisitClosedIEnumerable(ClosedIEnumerableCallSite closedIEnumerableCallSite, CallSiteValidatorState state) => closedIEnumerableCallSite.ServiceCallSites.Select(serviceCallSite => VisitCallSite(serviceCallSite, state)).Aggregate<Type?, Type?>(null, (current, scoped) => current ?? scoped);

    protected override Type? VisitSingleton(SingletonCallSite singletonCallSite, CallSiteValidatorState state)
    {
        state.Singleton = singletonCallSite;
        return VisitCallSite(singletonCallSite.ServiceCallSite, state);
    }

    protected override Type? VisitScoped(ScopedCallSite scopedCallSite, CallSiteValidatorState state) =>
        // We are fine with having ServiceScopeService requested by singletons
        scopedCallSite.ServiceCallSite is ServiceScopeService
            ? null
            : state.Singleton != null 
                ? throw new InvalidOperationException(Resources.FormatScopedInSingletonException(scopedCallSite.Key.ServiceType, state.Singleton.Key.ServiceType, nameof(ServiceLifetime.Scoped).ToLowerInvariant(), nameof(ServiceLifetime.Singleton).ToLowerInvariant()))
                : scopedCallSite.Key.ServiceType;

    protected override Type? VisitConstant(ConstantCallSite constantCallSite, CallSiteValidatorState state) => null;

    protected override Type? VisitCreateInstance(CreateInstanceCallSite createInstanceCallSite, CallSiteValidatorState state) => null;

    protected override Type? VisitInstanceService(InstanceService instanceCallSite, CallSiteValidatorState state) => null;

    protected override Type? VisitServiceProviderService(ServiceProviderService serviceProviderService, CallSiteValidatorState state) => null;

    protected override Type? VisitEmptyIEnumerable(EmptyIEnumerableCallSite emptyIEnumerableCallSite, CallSiteValidatorState state) => null;

    protected override Type? VisitServiceScopeService(ServiceScopeService serviceScopeService, CallSiteValidatorState state) => null;

    protected override Type? VisitFactoryService(FactoryService factoryService, CallSiteValidatorState state) => null;

    internal struct CallSiteValidatorState
    {
        public SingletonCallSite Singleton { get; set; }
    }
}