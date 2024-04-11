// ****************************************************************************
// Project:  Microsoft.Extensions.DependencyInjection
// File:     IGenericService.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

using Microsoft.Extensions.DependencyInjection.Abstractions;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup;

internal interface IGenericService
{
    ServiceLifetime Lifetime { get; }

    IService? GetService(Type? closedServiceType);
}