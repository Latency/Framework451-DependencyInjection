// ****************************************************************************
// Project:  Microsoft.Extensions.DependencyInjection
// File:     IService.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

using Microsoft.Extensions.DependencyInjection.Abstractions;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup;

internal interface IService
{
    IService? Next { get; set; }

    ServiceLifetime Lifetime { get; }

    Type? ServiceType { get; }

    IServiceCallSite CreateCallSite(ServiceProvider provider, ISet<Type?> callSiteChain);
}