// ****************************************************************************
// Project:  Microsoft.Extensions.DependencyInjection.ConsoleApp
// File:     DIProviderInstance.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

using Microsoft.Extensions.DependencyInjection.Abstractions;

namespace Microsoft.Extensions.DependencyInjection.ConsoleApp;

// ReSharper disable once InconsistentNaming
public static class DIProviderInstance
{
    private static IServiceCollection? _services;

    private static readonly Lazy<IServiceProvider> LazyInstance = new(() => _services is null ? throw new NullReferenceException("ProviderInstance must be set.") : _services.BuildServiceProvider());

    public static IServiceProvider ProviderInstance => LazyInstance.Value;

    internal static void SetProviderInstance(IServiceCollection serviceCollection) => _services = serviceCollection;
}