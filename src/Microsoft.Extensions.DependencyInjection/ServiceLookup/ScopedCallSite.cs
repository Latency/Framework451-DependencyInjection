// ****************************************************************************
// Project:  Microsoft.Extensions.DependencyInjection
// File:     ScopedCallSite.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup;

internal class ScopedCallSite(IService key, IServiceCallSite serviceCallSite) : IServiceCallSite
{
    internal IService         Key             { get; } = key;
    internal IServiceCallSite ServiceCallSite { get; } = serviceCallSite;
}