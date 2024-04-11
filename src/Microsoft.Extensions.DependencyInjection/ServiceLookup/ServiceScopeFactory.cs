// ****************************************************************************
// Project:  Microsoft.Extensions.DependencyInjection
// File:     ServiceScopeFactory.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

using Microsoft.Extensions.DependencyInjection.Abstractions;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup;

internal class ServiceScopeFactory(ServiceProvider provider) : IServiceScopeFactory
{
    public IServiceScope CreateScope() => new ServiceScope(new(provider));
}