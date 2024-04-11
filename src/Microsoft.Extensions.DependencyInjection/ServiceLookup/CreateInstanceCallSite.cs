// ****************************************************************************
// Project:  Microsoft.Extensions.DependencyInjection
// File:     CreateInstanceCallSite.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

using Microsoft.Extensions.DependencyInjection.Abstractions;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup;

internal class CreateInstanceCallSite(ServiceDescriptor descriptor) : IServiceCallSite
{
    internal ServiceDescriptor Descriptor { get; } = descriptor;
}