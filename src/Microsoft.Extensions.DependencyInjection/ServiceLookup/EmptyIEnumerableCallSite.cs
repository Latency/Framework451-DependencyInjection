// ****************************************************************************
// Project:  Microsoft.Extensions.DependencyInjection
// File:     EmptyIEnumerableCallSite.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup;

internal class EmptyIEnumerableCallSite(Type? serviceType, object serviceInstance) : IServiceCallSite
{
    internal object ServiceInstance { get; } = serviceInstance;
    internal Type?  ServiceType     { get; } = serviceType;
}