// ****************************************************************************
// Project:  Microsoft.Extensions.DependencyInjection
// File:     ConstantCallSite.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup;

internal class ConstantCallSite(object? defaultValue) : IServiceCallSite
{
    internal object? DefaultValue { get; } = defaultValue;
}