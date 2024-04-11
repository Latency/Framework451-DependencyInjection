// ****************************************************************************
// Project:  Microsoft.Extensions.DependencyInjection
// File:     ClosedIEnumerableCallSite.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup;

internal class ClosedIEnumerableCallSite(Type? itemType, IServiceCallSite[] serviceCallSites) : IServiceCallSite
{
    internal Type?              ItemType         { get; } = itemType;
    internal IServiceCallSite[] ServiceCallSites { get; } = serviceCallSites;
}