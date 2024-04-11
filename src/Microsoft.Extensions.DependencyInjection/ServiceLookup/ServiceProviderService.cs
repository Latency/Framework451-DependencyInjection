// ****************************************************************************
// Project:  Microsoft.Extensions.DependencyInjection
// File:     ServiceProviderService.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

using Microsoft.Extensions.DependencyInjection.Abstractions;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup;

internal class ServiceProviderService : IService, IServiceCallSite
{
    public IService? Next { get; set; }

    public ServiceLifetime Lifetime => ServiceLifetime.Transient;

    public Type? ServiceType => typeof(IServiceProvider);

    public IServiceCallSite CreateCallSite(ServiceProvider provider, ISet<Type?> callSiteChain) => this;
}