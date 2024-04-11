// ****************************************************************************
// Project:  Microsoft.Extensions.DependencyInjection
// File:     CallSiteVisitor.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup;

internal abstract class CallSiteVisitor<TArgument, TResult>
{
    protected virtual TResult VisitCallSite(IServiceCallSite callSite, TArgument argument)
    {
        if (callSite is FactoryService factoryService)
            return VisitFactoryService(factoryService, argument);
        if (callSite is ClosedIEnumerableCallSite closedIEnumerableCallSite)
            return VisitClosedIEnumerable(closedIEnumerableCallSite, argument);
        if (callSite is ConstructorCallSite constructorCallSite)
            return VisitConstructor(constructorCallSite, argument);
        if (callSite is TransientCallSite transientCallSite)
            return VisitTransient(transientCallSite, argument);
        if (callSite is SingletonCallSite singletonCallSite)
            return VisitSingleton(singletonCallSite, argument);
        if (callSite is ScopedCallSite scopedCallSite)
            return VisitScoped(scopedCallSite, argument);
        if (callSite is ConstantCallSite constantCallSite)
            return VisitConstant(constantCallSite, argument);
        if (callSite is CreateInstanceCallSite createInstanceCallSite)
            return VisitCreateInstance(createInstanceCallSite, argument);
        if (callSite is InstanceService instanceCallSite)
            return VisitInstanceService(instanceCallSite, argument);
        if (callSite is ServiceProviderService serviceProviderService)
            return VisitServiceProviderService(serviceProviderService, argument);
        if (callSite is EmptyIEnumerableCallSite emptyIEnumerableCallSite)
            return VisitEmptyIEnumerable(emptyIEnumerableCallSite, argument);
        if (callSite is ServiceScopeService serviceScopeService)
            return VisitServiceScopeService(serviceScopeService, argument);
        throw new NotSupportedException($"Call site type {callSite.GetType()} is not supported");
    }

    protected abstract TResult VisitTransient(TransientCallSite transientCallSite, TArgument argument);

    protected abstract TResult VisitConstructor(ConstructorCallSite constructorCallSite, TArgument argument);

    protected abstract TResult VisitSingleton(SingletonCallSite singletonCallSite, TArgument argument);

    protected abstract TResult VisitScoped(ScopedCallSite scopedCallSite, TArgument argument);

    protected abstract TResult VisitConstant(ConstantCallSite constantCallSite, TArgument argument);

    protected abstract TResult VisitCreateInstance(CreateInstanceCallSite createInstanceCallSite, TArgument argument);

    protected abstract TResult VisitInstanceService(InstanceService instanceCallSite, TArgument argument);

    protected abstract TResult VisitServiceProviderService(ServiceProviderService serviceProviderService, TArgument argument);

    protected abstract TResult VisitEmptyIEnumerable(EmptyIEnumerableCallSite emptyIEnumerableCallSite, TArgument argument);

    protected abstract TResult VisitServiceScopeService(ServiceScopeService serviceScopeService, TArgument argument);

    protected abstract TResult VisitClosedIEnumerable(ClosedIEnumerableCallSite closedIEnumerableCallSite, TArgument argument);

    protected abstract TResult VisitFactoryService(FactoryService factoryService, TArgument argument);
}