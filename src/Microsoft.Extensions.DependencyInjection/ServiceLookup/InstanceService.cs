// ****************************************************************************
// Project:  Microsoft.Extensions.DependencyInjection
// File:     InstanceService.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

using Microsoft.Extensions.DependencyInjection.Abstractions;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup;

/// <summary>
///     Summary description for InstanceService
/// </summary>
internal class InstanceService : IService, IServiceCallSite
{
    public InstanceService(ServiceDescriptor descriptor) => Descriptor = descriptor;

    internal ServiceDescriptor Descriptor { get; }

    public IService? Next { get; set; }

    public ServiceLifetime Lifetime => Descriptor.Lifetime;

    public Type? ServiceType => Descriptor.ServiceType;

    public IServiceCallSite CreateCallSite(ServiceProvider provider, ISet<Type?> callSiteChain) => this;
}