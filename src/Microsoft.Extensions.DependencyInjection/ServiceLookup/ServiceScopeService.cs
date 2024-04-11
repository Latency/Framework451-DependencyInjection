// ****************************************************************************
// Project:  Microsoft.Extensions.DependencyInjection
// File:     ServiceScopeService.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

using Microsoft.Extensions.DependencyInjection.Abstractions;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup;

internal class ServiceScopeService : IService, IServiceCallSite
{
    public IService? Next { get; set; }

    public ServiceLifetime Lifetime => ServiceLifetime.Scoped;

    public Type? ServiceType => typeof(IServiceScopeFactory);

    public IServiceCallSite CreateCallSite(ServiceProvider provider, ISet<Type?> callSiteChain) => this;
}