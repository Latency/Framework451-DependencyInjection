// ****************************************************************************
// Project:  Microsoft.Extensions.DependencyInjection.Abstractions
// File:     ServiceCollectionServiceExtensions.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************
// ReSharper disable UnusedMember.Global

namespace Microsoft.Extensions.DependencyInjection.Abstractions;

/// <summary>
///     Extension methods for adding services to an <see cref="IServiceCollection" />.
/// </summary>
public static class ServiceCollectionServiceExtensions
{
    /// <summary>
    ///     Adds a transient service of the type specified in <paramref name="serviceType" /> with an
    ///     implementation of the type specified in <paramref name="implementationType" /> to the
    ///     specified <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
    /// <param name="serviceType">The type of the service to register.</param>
    /// <param name="implementationType">The implementation type of the service.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <seealso cref="ServiceLifetime.Transient" />
    public static IServiceCollection AddTransient(this IServiceCollection services, Type? serviceType, Type? implementationType) => services == null
                                                                                                                                        ? throw new ArgumentNullException(nameof(services))
                                                                                                                                        : serviceType == null
                                                                                                                                            ? throw new ArgumentNullException(nameof(serviceType))
                                                                                                                                            : implementationType == null
                                                                                                                                                ? throw new ArgumentNullException(nameof(implementationType))
                                                                                                                                                : Add(services, serviceType, implementationType, ServiceLifetime.Transient);

    /// <summary>
    ///     Adds a transient service of the type specified in <paramref name="serviceType" /> with a
    ///     factory specified in <paramref name="implementationFactory" /> to the
    ///     specified <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
    /// <param name="serviceType">The type of the service to register.</param>
    /// <param name="implementationFactory">The factory that creates the service.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <seealso cref="ServiceLifetime.Transient" />
    public static IServiceCollection AddTransient(this IServiceCollection services, Type? serviceType, Func<IServiceProvider, object>? implementationFactory) => services == null
                                                                                                                                                                     ? throw new ArgumentNullException(nameof(services))
                                                                                                                                                                     : serviceType == null
                                                                                                                                                                         ? throw new ArgumentNullException(nameof(serviceType))
                                                                                                                                                                         : implementationFactory == null
                                                                                                                                                                             ? throw new ArgumentNullException(nameof(implementationFactory))
                                                                                                                                                                             : Add(services, serviceType, implementationFactory, ServiceLifetime.Transient);

    /// <summary>
    ///     Adds a transient service of the type specified in <typeparamref name="TService" /> with an
    ///     implementation type specified in <typeparamref name="TImplementation" /> to the
    ///     specified <see cref="IServiceCollection" />.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <seealso cref="ServiceLifetime.Transient" />
    public static IServiceCollection AddTransient<TService, TImplementation>(this IServiceCollection services)
        where TService : class
        where TImplementation : class, TService => services == null
                                                       ? throw new ArgumentNullException(nameof(services))
                                                       : services.AddTransient(typeof(TService), typeof(TImplementation));

    /// <summary>
    ///     Adds a transient service of the type specified in <paramref name="serviceType" /> to the
    ///     specified <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
    /// <param name="serviceType">The type of the service to register and the implementation to use.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <seealso cref="ServiceLifetime.Transient" />
    public static IServiceCollection AddTransient(this IServiceCollection services, Type? serviceType) => services == null
                                                                                                              ? throw new ArgumentNullException(nameof(services))
                                                                                                              : serviceType == null ? throw new ArgumentNullException(nameof(serviceType)) : services.AddTransient(serviceType, serviceType);

    /// <summary>
    ///     Adds a transient service of the type specified in <typeparamref name="TService" /> to the
    ///     specified <see cref="IServiceCollection" />.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <seealso cref="ServiceLifetime.Transient" />
    public static IServiceCollection AddTransient<TService>(this IServiceCollection services)
        where TService : class => services == null ? throw new ArgumentNullException(nameof(services)) : services.AddTransient(typeof(TService));

    /// <summary>
    ///     Adds a transient service of the type specified in <typeparamref name="TService" /> with a
    ///     factory specified in <paramref name="implementationFactory" /> to the
    ///     specified <see cref="IServiceCollection" />.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
    /// <param name="implementationFactory">The factory that creates the service.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <seealso cref="ServiceLifetime.Transient" />
    public static IServiceCollection AddTransient<TService>(this IServiceCollection services, Func<IServiceProvider, TService>? implementationFactory)
        where TService : class => services == null
                                      ? throw new ArgumentNullException(nameof(services))
                                      : implementationFactory == null
                                          ? throw new ArgumentNullException(nameof(implementationFactory))
                                          : services.AddTransient(typeof(TService), implementationFactory);

    /// <summary>
    ///     Adds a transient service of the type specified in <typeparamref name="TService" /> with an
    ///     implementation type specified in <typeparamref name="TImplementation" /> using the
    ///     factory specified in <paramref name="implementationFactory" /> to the
    ///     specified <see cref="IServiceCollection" />.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
    /// <param name="implementationFactory">The factory that creates the service.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <seealso cref="ServiceLifetime.Transient" />
    public static IServiceCollection AddTransient<TService, TImplementation>(this IServiceCollection services, Func<IServiceProvider, TImplementation>? implementationFactory)
        where TService : class
        where TImplementation : class, TService => services == null
                                                       ? throw new ArgumentNullException(nameof(services))
                                                       : implementationFactory == null
                                                           ? throw new ArgumentNullException(nameof(implementationFactory))
                                                           : services.AddTransient(typeof(TService), implementationFactory);


    /// <summary>
    ///     Adds a scoped service of the type specified in <paramref name="serviceType" /> with an
    ///     implementation of the type specified in <paramref name="implementationType" /> to the
    ///     specified <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
    /// <param name="serviceType">The type of the service to register.</param>
    /// <param name="implementationType">The implementation type of the service.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <seealso cref="ServiceLifetime.Scoped" />
    public static IServiceCollection AddScoped(this IServiceCollection services, Type? serviceType, Type? implementationType) => services == null
                                                                                                                                     ? throw new ArgumentNullException(nameof(services))
                                                                                                                                     : serviceType == null
                                                                                                                                         ? throw new ArgumentNullException(nameof(serviceType))
                                                                                                                                         : implementationType == null
                                                                                                                                             ? throw new ArgumentNullException(nameof(implementationType))
                                                                                                                                             : Add(services, serviceType, implementationType, ServiceLifetime.Scoped);

    /// <summary>
    ///     Adds a scoped service of the type specified in <paramref name="serviceType" /> with a
    ///     factory specified in <paramref name="implementationFactory" /> to the
    ///     specified <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
    /// <param name="serviceType">The type of the service to register.</param>
    /// <param name="implementationFactory">The factory that creates the service.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <seealso cref="ServiceLifetime.Scoped" />
    public static IServiceCollection AddScoped(this IServiceCollection services, Type? serviceType, Func<IServiceProvider, object>? implementationFactory) => services == null
                                                                                                                                                                  ? throw new ArgumentNullException(nameof(services))
                                                                                                                                                                  : serviceType == null
                                                                                                                                                                      ? throw new ArgumentNullException(nameof(serviceType))
                                                                                                                                                                      : implementationFactory == null
                                                                                                                                                                          ? throw new ArgumentNullException(nameof(implementationFactory))
                                                                                                                                                                          : Add(services, serviceType, implementationFactory, ServiceLifetime.Scoped);

    /// <summary>
    ///     Adds a scoped service of the type specified in <typeparamref name="TService" /> with an
    ///     implementation type specified in <typeparamref name="TImplementation" /> to the
    ///     specified <see cref="IServiceCollection" />.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <seealso cref="ServiceLifetime.Scoped" />
    public static IServiceCollection AddScoped<TService, TImplementation>(this IServiceCollection services)
        where TService : class
        where TImplementation : class, TService => services == null
                                                       ? throw new ArgumentNullException(nameof(services))
                                                       : services.AddScoped(typeof(TService), typeof(TImplementation));

    /// <summary>
    ///     Adds a scoped service of the type specified in <paramref name="serviceType" /> to the
    ///     specified <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
    /// <param name="serviceType">The type of the service to register and the implementation to use.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <seealso cref="ServiceLifetime.Scoped" />
    public static IServiceCollection AddScoped(this IServiceCollection services, Type? serviceType) => services == null
                                                                                                           ? throw new ArgumentNullException(nameof(services))
                                                                                                           : serviceType == null ? throw new ArgumentNullException(nameof(serviceType)) : services.AddScoped(serviceType, serviceType);

    /// <summary>
    ///     Adds a scoped service of the type specified in <typeparamref name="TService" /> to the
    ///     specified <see cref="IServiceCollection" />.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <seealso cref="ServiceLifetime.Scoped" />
    public static IServiceCollection AddScoped<TService>(this IServiceCollection services)
        where TService : class => services == null ? throw new ArgumentNullException(nameof(services)) : services.AddScoped(typeof(TService));

    /// <summary>
    ///     Adds a scoped service of the type specified in <typeparamref name="TService" /> with a
    ///     factory specified in <paramref name="implementationFactory" /> to the
    ///     specified <see cref="IServiceCollection" />.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
    /// <param name="implementationFactory">The factory that creates the service.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <seealso cref="ServiceLifetime.Scoped" />
    public static IServiceCollection AddScoped<TService>(this IServiceCollection services, Func<IServiceProvider, TService>? implementationFactory)
        where TService : class => services == null
                                      ? throw new ArgumentNullException(nameof(services))
                                      : implementationFactory == null
                                          ? throw new ArgumentNullException(nameof(implementationFactory))
                                          : services.AddScoped(typeof(TService), implementationFactory);

    /// <summary>
    ///     Adds a scoped service of the type specified in <typeparamref name="TService" /> with an
    ///     implementation type specified in <typeparamref name="TImplementation" /> using the
    ///     factory specified in <paramref name="implementationFactory" /> to the
    ///     specified <see cref="IServiceCollection" />.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
    /// <param name="implementationFactory">The factory that creates the service.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <seealso cref="ServiceLifetime.Scoped" />
    public static IServiceCollection AddScoped<TService, TImplementation>(this IServiceCollection services, Func<IServiceProvider, TImplementation>? implementationFactory)
        where TService : class
        where TImplementation : class, TService => services == null
                                                       ? throw new ArgumentNullException(nameof(services))
                                                       : implementationFactory == null
                                                           ? throw new ArgumentNullException(nameof(implementationFactory))
                                                           : services.AddScoped(typeof(TService), implementationFactory);


    /// <summary>
    ///     Adds a singleton service of the type specified in <paramref name="serviceType" /> with an
    ///     implementation of the type specified in <paramref name="implementationType" /> to the
    ///     specified <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
    /// <param name="serviceType">The type of the service to register.</param>
    /// <param name="implementationType">The implementation type of the service.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <seealso cref="ServiceLifetime.Singleton" />
    public static IServiceCollection AddSingleton(this IServiceCollection services, Type? serviceType, Type? implementationType) => services == null
                                                                                                                                        ? throw new ArgumentNullException(nameof(services))
                                                                                                                                        : serviceType == null
                                                                                                                                            ? throw new ArgumentNullException(nameof(serviceType))
                                                                                                                                            : implementationType == null
                                                                                                                                                ? throw new ArgumentNullException(nameof(implementationType))
                                                                                                                                                : Add(services, serviceType, implementationType, ServiceLifetime.Singleton);

    /// <summary>
    ///     Adds a singleton service of the type specified in <paramref name="serviceType" /> with a
    ///     factory specified in <paramref name="implementationFactory" /> to the
    ///     specified <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
    /// <param name="serviceType">The type of the service to register.</param>
    /// <param name="implementationFactory">The factory that creates the service.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <seealso cref="ServiceLifetime.Singleton" />
    public static IServiceCollection AddSingleton(this IServiceCollection services, Type? serviceType, Func<IServiceProvider, object>? implementationFactory) => services == null
                                                                                                                                                                     ? throw new ArgumentNullException(nameof(services))
                                                                                                                                                                     : serviceType == null
                                                                                                                                                                         ? throw new ArgumentNullException(nameof(serviceType))
                                                                                                                                                                         : implementationFactory == null
                                                                                                                                                                             ? throw new ArgumentNullException(nameof(implementationFactory))
                                                                                                                                                                             : Add(services, serviceType, implementationFactory, ServiceLifetime.Singleton);

    /// <summary>
    ///     Adds a singleton service of the type specified in <typeparamref name="TService" /> with an
    ///     implementation type specified in <typeparamref name="TImplementation" /> to the
    ///     specified <see cref="IServiceCollection" />.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <seealso cref="ServiceLifetime.Singleton" />
    public static IServiceCollection AddSingleton<TService, TImplementation>(this IServiceCollection services)
        where TService : class
        where TImplementation : class, TService => services == null
                                                       ? throw new ArgumentNullException(nameof(services))
                                                       : services.AddSingleton(typeof(TService), typeof(TImplementation));

    /// <summary>
    ///     Adds a singleton service of the type specified in <paramref name="serviceType" /> to the
    ///     specified <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
    /// <param name="serviceType">The type of the service to register and the implementation to use.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <seealso cref="ServiceLifetime.Singleton" />
    public static IServiceCollection AddSingleton(this IServiceCollection services, Type? serviceType) => services == null
                                                                                                              ? throw new ArgumentNullException(nameof(services))
                                                                                                              : serviceType == null ? throw new ArgumentNullException(nameof(serviceType)) : services.AddSingleton(serviceType, serviceType);

    /// <summary>
    ///     Adds a singleton service of the type specified in <typeparamref name="TService" /> to the
    ///     specified <see cref="IServiceCollection" />.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <seealso cref="ServiceLifetime.Singleton" />
    public static IServiceCollection AddSingleton<TService>(this IServiceCollection services)
        where TService : class => services == null ? throw new ArgumentNullException(nameof(services)) : services.AddSingleton(typeof(TService));

    /// <summary>
    ///     Adds a singleton service of the type specified in <typeparamref name="TService" /> with a
    ///     factory specified in <paramref name="implementationFactory" /> to the
    ///     specified <see cref="IServiceCollection" />.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
    /// <param name="implementationFactory">The factory that creates the service.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <seealso cref="ServiceLifetime.Singleton" />
    public static IServiceCollection AddSingleton<TService>(this IServiceCollection services, Func<IServiceProvider, TService>? implementationFactory)
        where TService : class => services == null
                                      ? throw new ArgumentNullException(nameof(services))
                                      : implementationFactory == null
                                          ? throw new ArgumentNullException(nameof(implementationFactory))
                                          : services.AddSingleton(typeof(TService), implementationFactory);

    /// <summary>
    ///     Adds a singleton service of the type specified in <typeparamref name="TService" /> with an
    ///     implementation type specified in <typeparamref name="TImplementation" /> using the
    ///     factory specified in <paramref name="implementationFactory" /> to the
    ///     specified <see cref="IServiceCollection" />.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
    /// <param name="implementationFactory">The factory that creates the service.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <seealso cref="ServiceLifetime.Singleton" />
    public static IServiceCollection AddSingleton<TService, TImplementation>(this IServiceCollection services, Func<IServiceProvider, TImplementation>? implementationFactory)
        where TService : class
        where TImplementation : class, TService => services == null
                                                       ? throw new ArgumentNullException(nameof(services))
                                                       : implementationFactory == null
                                                           ? throw new ArgumentNullException(nameof(implementationFactory))
                                                           : services.AddSingleton(typeof(TService), implementationFactory);

    /// <summary>
    ///     Adds a singleton service of the type specified in <paramref name="serviceType" /> with an
    ///     instance specified in <paramref name="implementationInstance" /> to the
    ///     specified <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
    /// <param name="serviceType">The type of the service to register.</param>
    /// <param name="implementationInstance">The instance of the service.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <seealso cref="ServiceLifetime.Singleton" />
    public static IServiceCollection AddSingleton(this IServiceCollection services, Type? serviceType, object? implementationInstance)
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services));

        if (serviceType == null)
            throw new ArgumentNullException(nameof(serviceType));

        if (implementationInstance == null)
            throw new ArgumentNullException(nameof(implementationInstance));

        services.Add(new(serviceType, implementationInstance));
        return services;
    }

    /// <summary>
    ///     Adds a singleton service of the type specified in <typeparamref name="TService" /> with an
    ///     instance specified in <paramref name="implementationInstance" /> to the
    ///     specified <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
    /// <param name="implementationInstance">The instance of the service.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <seealso cref="ServiceLifetime.Singleton" />
    public static IServiceCollection AddSingleton<TService>(this IServiceCollection services, TService? implementationInstance)
        where TService : class => services == null
                                      ? throw new ArgumentNullException(nameof(services))
                                      : implementationInstance == null
                                          ? throw new ArgumentNullException(nameof(implementationInstance))
                                          : services.AddSingleton(typeof(TService), implementationInstance);

    private static IServiceCollection Add(IServiceCollection collection, Type? serviceType, Type? implementationType, ServiceLifetime lifetime)
    {
        var descriptor = new ServiceDescriptor(serviceType, implementationType, lifetime);
        collection.Add(descriptor);
        return collection;
    }

    private static IServiceCollection Add(IServiceCollection collection, Type? serviceType, Func<IServiceProvider, object>? implementationFactory, ServiceLifetime lifetime)
    {
        var descriptor = new ServiceDescriptor(serviceType, implementationFactory, lifetime);
        collection.Add(descriptor);
        return collection;
    }
}