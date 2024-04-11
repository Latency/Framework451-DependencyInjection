// ****************************************************************************
// Project:  Microsoft.Extensions.DependencyInjection.Abstractions
// File:     IServiceCollection.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

namespace Microsoft.Extensions.DependencyInjection.Abstractions;

/// <summary>
///     Specifies the contract for a collection of service descriptors.
/// </summary>
public interface IServiceCollection : IList<ServiceDescriptor>
{ }