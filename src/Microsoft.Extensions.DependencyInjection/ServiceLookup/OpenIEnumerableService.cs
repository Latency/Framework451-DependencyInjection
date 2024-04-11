// ****************************************************************************
// Project:  Microsoft.Extensions.DependencyInjection
// File:     OpenIEnumerableService.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Abstractions;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup;

internal class OpenIEnumerableService(ServiceTable table) : IGenericService
{
    public ServiceLifetime Lifetime => ServiceLifetime.Transient;

    public IService? GetService(Type? closedServiceType)
    {
        var itemType = closedServiceType.GetTypeInfo().GenericTypeArguments[0];

        return table.TryGetEntry(itemType, out var entry) ? new ClosedIEnumerableService(itemType, entry) : null;
    }
}