// ****************************************************************************
// Project:  Microsoft.Extensions.DependencyInjection
// File:     ServiceScope.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

using Microsoft.Extensions.DependencyInjection.Abstractions;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup;

internal class ServiceScope(ServiceProvider scopedProvider) : IServiceScope
{
    public IServiceProvider ServiceProvider => scopedProvider;

    public void Dispose() => scopedProvider.Dispose();
}