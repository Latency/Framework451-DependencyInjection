// ****************************************************************************
// Project:  Microsoft.Extensions.DependencyInjection.Abstractions
// File:     ObjectFactory.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

namespace Microsoft.Extensions.DependencyInjection.Abstractions;

/// <summary>
///     The result of <see cref="ActivatorUtilities.CreateFactory(Type, Type[])" />.
/// </summary>
/// <param name="serviceProvider">The <see cref="IServiceProvider" /> to get service arguments from.</param>
/// <param name="arguments">Additional constructor arguments.</param>
/// <returns>The instantiated type.</returns>
public delegate object ObjectFactory(IServiceProvider serviceProvider, object[] arguments);