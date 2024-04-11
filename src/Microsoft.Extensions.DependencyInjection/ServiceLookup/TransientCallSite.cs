// ****************************************************************************
// Project:  Microsoft.Extensions.DependencyInjection
// File:     TransientCallSite.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup;

internal class TransientCallSite(IServiceCallSite service) : IServiceCallSite
{
    internal IServiceCallSite Service { get; } = service;
}