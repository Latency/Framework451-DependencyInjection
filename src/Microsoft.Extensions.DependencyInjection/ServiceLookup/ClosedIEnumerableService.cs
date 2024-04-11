// ****************************************************************************
// Project:  Microsoft.Extensions.DependencyInjection
// File:     ClosedIEnumerableService.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

using Microsoft.Extensions.DependencyInjection.Abstractions;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup;

internal class ClosedIEnumerableService(Type? itemType, ServiceEntry entry) : IService
{
    public IService? Next { get; set; }

    public ServiceLifetime Lifetime => ServiceLifetime.Transient;

    public Type? ServiceType { get; } = itemType;

    public IServiceCallSite CreateCallSite(ServiceProvider provider, ISet<Type?> callSiteChain)
    {
        var list    = new List<IServiceCallSite>();
        var service = entry.First;
        while (service != null)
        {
            list.Add(provider.GetResolveCallSite(service, callSiteChain));
            service = service.Next;
        }

        return new ClosedIEnumerableCallSite(ServiceType, [.. list]);
    }
}