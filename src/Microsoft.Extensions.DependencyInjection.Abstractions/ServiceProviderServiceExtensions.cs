// ****************************************************************************
// Project:  Microsoft.Extensions.DependencyInjection.Abstractions
// File:     ServiceProviderServiceExtensions.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

using Microsoft.Extensions.DependencyInjection.Abstractions.Properties;

namespace Microsoft.Extensions.DependencyInjection.Abstractions;

/// <summary>
///     Extension methods for getting services from an <see cref="IServiceProvider" />.
/// </summary>
public static class ServiceProviderServiceExtensions
{
    /// <summary>
    ///     Get service of type <typeparamref name="T" /> from the <see cref="IServiceProvider" />.
    /// </summary>
    /// <typeparam name="T">The type of service object to get.</typeparam>
    /// <param name="provider">The <see cref="IServiceProvider" /> to retrieve the service object from.</param>
    /// <returns>A service object of type <typeparamref name="T" /> or null if there is no such service.</returns>
    public static T GetService<T>(this IServiceProvider provider)
    {
        if (provider == null)
            throw new ArgumentNullException(nameof(provider));

        return (T)provider.GetService(typeof(T));
    }

    /// <summary>
    ///     Get service of type <paramref name="serviceType" /> from the <see cref="IServiceProvider" />.
    /// </summary>
    /// <param name="provider">The <see cref="IServiceProvider" /> to retrieve the service object from.</param>
    /// <param name="serviceType">An object that specifies the type of service object to get.</param>
    /// <returns>A service object of type <paramref name="serviceType" />.</returns>
    /// <exception cref="System.InvalidOperationException">There is no service of type <paramref name="serviceType" />.</exception>
    public static object GetRequiredService(this IServiceProvider provider, Type serviceType)
    {
        if (provider == null)
            throw new ArgumentNullException(nameof(provider));

        if (serviceType == null)
            throw new ArgumentNullException(nameof(serviceType));

        if (provider is ISupportRequiredService requiredServiceSupportingProvider)
            return requiredServiceSupportingProvider.GetRequiredService(serviceType);

        return provider.GetService(serviceType) ?? throw new InvalidOperationException(Resources.FormatNoServiceRegistered(serviceType));
    }

    /// <summary>
    ///     Get service of type <typeparamref name="T" /> from the <see cref="IServiceProvider" />.
    /// </summary>
    /// <typeparam name="T">The type of service object to get.</typeparam>
    /// <param name="provider">The <see cref="IServiceProvider" /> to retrieve the service object from.</param>
    /// <returns>A service object of type <typeparamref name="T" />.</returns>
    /// <exception cref="System.InvalidOperationException">There is no service of type <typeparamref name="T" />.</exception>
    public static T GetRequiredService<T>(this IServiceProvider provider) => provider == null ? throw new ArgumentNullException(nameof(provider)) : (T)provider.GetRequiredService(typeof(T));

    /// <summary>
    ///     Get an enumeration of services of type <typeparamref name="T" /> from the <see cref="IServiceProvider" />.
    /// </summary>
    /// <typeparam name="T">The type of service object to get.</typeparam>
    /// <param name="provider">The <see cref="IServiceProvider" /> to retrieve the services from.</param>
    /// <returns>An enumeration of services of type <typeparamref name="T" />.</returns>
    public static IEnumerable<T> GetServices<T>(this IServiceProvider provider) => provider == null ? throw new ArgumentNullException(nameof(provider)) : provider.GetRequiredService<IEnumerable<T>>();

    /// <summary>
    ///     Get an enumeration of services of type <paramref name="serviceType" /> from the <see cref="IServiceProvider" />.
    /// </summary>
    /// <param name="provider">The <see cref="IServiceProvider" /> to retrieve the services from.</param>
    /// <param name="serviceType">An object that specifies the type of service object to get.</param>
    /// <returns>An enumeration of services of type <paramref name="serviceType" />.</returns>
    public static IEnumerable<object> GetServices(this IServiceProvider provider, Type serviceType)
    {
        if (provider == null)
            throw new ArgumentNullException(nameof(provider));

        if (serviceType == null)
            throw new ArgumentNullException(nameof(serviceType));

        var genericEnumerable = typeof(IEnumerable<>).MakeGenericType(serviceType);
        return (IEnumerable<object>)provider.GetRequiredService(genericEnumerable);
    }

    /// <summary>
    ///     Creates a new <see cref="IServiceScope" /> that can be used to resolve scoped services.
    /// </summary>
    /// <param name="provider">The <see cref="IServiceProvider" /> to create the scope from.</param>
    /// <returns>A <see cref="IServiceScope" /> that can be used to resolve scoped services.</returns>
    public static IServiceScope CreateScope(this IServiceProvider provider) => provider.GetRequiredService<IServiceScopeFactory>().CreateScope();
}