// ****************************************************************************
// Project:  Microsoft.Extensions.DependencyInjection
// File:     ServiceCollectionContainerBuilderExtensions.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

using Microsoft.Extensions.DependencyInjection.Abstractions;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionContainerBuilderExtensions
{
    /// <summary>
    ///     Creates an <see cref="IServiceProvider" /> containing services from the provided <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> containing service descriptors.</param>
    /// <returns>The<see cref="IServiceProvider" />.</returns>
    public static IServiceProvider BuildServiceProvider(this IServiceCollection services) => BuildServiceProvider(services, false);

    /// <summary>
    ///     Creates an <see cref="IServiceProvider" /> containing services from the provided <see cref="IServiceCollection" />
    ///     optionaly enabling scope validation.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> containing service descriptors.</param>
    /// <param name="validateScopes">
    ///     <c>true</c> to perform check verifying that scoped services never gets resolved from root provider; otherwise
    ///     <c>false</c>.
    /// </param>
    /// <returns>The<see cref="IServiceProvider" />.</returns>
    public static IServiceProvider BuildServiceProvider(this IServiceCollection services, bool validateScopes) => new ServiceProvider(services, validateScopes);
}