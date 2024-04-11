// ****************************************************************************
// Project:  Microsoft.Extensions.DependencyInjection.Abstractions
// File:     ServiceDescriptor.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************
// ReSharper disable InheritdocInvalidUsage
// ReSharper disable UnusedMember.Global

using System.Diagnostics;

namespace Microsoft.Extensions.DependencyInjection.Abstractions;

[DebuggerDisplay("Lifetime = {Lifetime}, ServiceType = {ServiceType}, ImplementationType = {ImplementationType}")]
public class ServiceDescriptor
{
    /// <summary>
    ///     Initializes a new instance of <see cref="ServiceDescriptor" /> with the specified
    ///     <paramref name="implementationType" />.
    /// </summary>
    /// <param name="serviceType">The <see cref="Type" /> of the service.</param>
    /// <param name="implementationType">The <see cref="Type" /> implementing the service.</param>
    /// <param name="lifetime">The <see cref="ServiceLifetime" /> of the service.</param>
    public ServiceDescriptor(Type? serviceType, Type? implementationType, ServiceLifetime lifetime) : this(serviceType, lifetime)
    {
        if (serviceType == null)
            throw new ArgumentNullException(nameof(serviceType));

        ImplementationType = implementationType ?? throw new ArgumentNullException(nameof(implementationType));
    }

    /// <summary>
    ///     Initializes a new instance of <see cref="ServiceDescriptor" /> with the specified <paramref name="instance" />
    ///     as a <see cref="ServiceLifetime.Singleton" />.
    /// </summary>
    /// <param name="serviceType">The <see cref="Type" /> of the service.</param>
    /// <param name="instance">The instance implementing the service.</param>
    public ServiceDescriptor(Type? serviceType, object? instance) : this(serviceType, ServiceLifetime.Singleton)
    {
        if (serviceType == null)
            throw new ArgumentNullException(nameof(serviceType));

        ImplementationInstance = instance ?? throw new ArgumentNullException(nameof(instance));
    }

    /// <summary>
    ///     Initializes a new instance of <see cref="ServiceDescriptor" /> with the specified <paramref name="factory" />.
    /// </summary>
    /// <param name="serviceType">The <see cref="Type" /> of the service.</param>
    /// <param name="factory">A factory used for creating service instances.</param>
    /// <param name="lifetime">The <see cref="ServiceLifetime" /> of the service.</param>
    public ServiceDescriptor(Type? serviceType, Func<IServiceProvider, object>? factory, ServiceLifetime lifetime) : this(serviceType, lifetime)
    {
        if (serviceType == null)
            throw new ArgumentNullException(nameof(serviceType));

        ImplementationFactory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    private ServiceDescriptor(Type? serviceType, ServiceLifetime lifetime)
    {
        Lifetime    = lifetime;
        ServiceType = serviceType;
    }

    /// <inheritdoc />
    public ServiceLifetime Lifetime { get; }

    /// <inheritdoc />
    public Type? ServiceType { get; }

    /// <inheritdoc />
    public Type? ImplementationType { get; }

    /// <inheritdoc />
    public object? ImplementationInstance { get; }

    /// <inheritdoc />
    public Func<IServiceProvider, object>? ImplementationFactory { get; }

    internal Type? GetImplementationType()
    {
        if (ImplementationType != null)
            return ImplementationType;

        if (ImplementationInstance != null)
            return ImplementationInstance.GetType();

        if (ImplementationFactory != null)
        {
            var typeArguments = ImplementationFactory.GetType().GenericTypeArguments;

            Debug.Assert(typeArguments.Length == 2);

            return typeArguments[1];
        }

        Debug.Assert(false, "ImplementationType, ImplementationInstance or ImplementationFactory must be non null");
        return null;
    }

    public static ServiceDescriptor Transient<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService => Describe<TService, TImplementation>(ServiceLifetime.Transient);

    public static ServiceDescriptor Transient(Type? service, Type? implementationType) => service == null
                                                                                              ? throw new ArgumentNullException(nameof(service))
                                                                                              : implementationType == null
                                                                                                  ? throw new ArgumentNullException(nameof(implementationType))
                                                                                                  : Describe(service, implementationType, ServiceLifetime.Transient);

    public static ServiceDescriptor Transient<TService, TImplementation>(Func<IServiceProvider, TImplementation>? implementationFactory)
        where TService : class
        where TImplementation : class, TService => implementationFactory == null
                                                       ? throw new ArgumentNullException(nameof(implementationFactory))
                                                       : Describe(typeof(TService), implementationFactory, ServiceLifetime.Transient);

    public static ServiceDescriptor Transient<TService>(Func<IServiceProvider, TService>? implementationFactory)
        where TService : class => implementationFactory == null
                                      ? throw new ArgumentNullException(nameof(implementationFactory))
                                      : Describe(typeof(TService), implementationFactory, ServiceLifetime.Transient);

    public static ServiceDescriptor Transient(Type? service, Func<IServiceProvider, object>? implementationFactory) => service == null
                                                                                                                           ? throw new ArgumentNullException(nameof(service))
                                                                                                                           : implementationFactory == null
                                                                                                                               ? throw new ArgumentNullException(nameof(implementationFactory))
                                                                                                                               : Describe(service, implementationFactory, ServiceLifetime.Transient);

    public static ServiceDescriptor Scoped<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService => Describe<TService, TImplementation>(ServiceLifetime.Scoped);

    public static ServiceDescriptor Scoped(Type? service, Type? implementationType) => Describe(service, implementationType, ServiceLifetime.Scoped);

    public static ServiceDescriptor Scoped<TService, TImplementation>(Func<IServiceProvider, TImplementation>? implementationFactory)
        where TService : class
        where TImplementation : class, TService => implementationFactory == null
                                                       ? throw new ArgumentNullException(nameof(implementationFactory))
                                                       : Describe(typeof(TService), implementationFactory, ServiceLifetime.Scoped);

    public static ServiceDescriptor Scoped<TService>(Func<IServiceProvider, TService>? implementationFactory)
        where TService : class => implementationFactory == null
                                      ? throw new ArgumentNullException(nameof(implementationFactory))
                                      : Describe(typeof(TService), implementationFactory, ServiceLifetime.Scoped);

    public static ServiceDescriptor Scoped(Type? service, Func<IServiceProvider, object>? implementationFactory) => service == null
                                                                                                                        ? throw new ArgumentNullException(nameof(service))
                                                                                                                        : implementationFactory == null
                                                                                                                            ? throw new ArgumentNullException(nameof(implementationFactory))
                                                                                                                            : Describe(service, implementationFactory, ServiceLifetime.Scoped);

    public static ServiceDescriptor Singleton<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService => Describe<TService, TImplementation>(ServiceLifetime.Singleton);

    public static ServiceDescriptor Singleton(Type? service, Type? implementationType) => service == null
                                                                                              ? throw new ArgumentNullException(nameof(service))
                                                                                              : implementationType == null
                                                                                                  ? throw new ArgumentNullException(nameof(implementationType))
                                                                                                  : Describe(service, implementationType, ServiceLifetime.Singleton);

    public static ServiceDescriptor Singleton<TService, TImplementation>(Func<IServiceProvider, TImplementation>? implementationFactory)
        where TService : class
        where TImplementation : class, TService => implementationFactory == null
                                                       ? throw new ArgumentNullException(nameof(implementationFactory))
                                                       : Describe(typeof(TService), implementationFactory, ServiceLifetime.Singleton);

    public static ServiceDescriptor Singleton<TService>(Func<IServiceProvider, TService>? implementationFactory)
        where TService : class => implementationFactory == null
                                      ? throw new ArgumentNullException(nameof(implementationFactory))
                                      : Describe(typeof(TService), implementationFactory, ServiceLifetime.Singleton);

    public static ServiceDescriptor Singleton(Type? serviceType, Func<IServiceProvider, object>? implementationFactory) => serviceType == null
                                                                                                                               ? throw new ArgumentNullException(nameof(serviceType))
                                                                                                                               : implementationFactory == null
                                                                                                                                   ? throw new ArgumentNullException(nameof(implementationFactory))
                                                                                                                                   : Describe(serviceType, implementationFactory, ServiceLifetime.Singleton);

    public static ServiceDescriptor Singleton<TService>(TService? implementationInstance)
        where TService : class => implementationInstance == null
                                      ? throw new ArgumentNullException(nameof(implementationInstance))
                                      : Singleton(typeof(TService), implementationInstance);

    public static ServiceDescriptor Singleton(Type? serviceType, object? implementationInstance) => serviceType == null
                                                                                                        ? throw new ArgumentNullException(nameof(serviceType))
                                                                                                        : implementationInstance == null
                                                                                                            ? throw new ArgumentNullException(nameof(implementationInstance))
                                                                                                            : new(serviceType, implementationInstance);

    private static ServiceDescriptor Describe<TService, TImplementation>(ServiceLifetime lifetime)
        where TService : class
        where TImplementation : class, TService => Describe(typeof(TService), typeof(TImplementation), lifetime);

    public static ServiceDescriptor Describe(Type? serviceType, Type? implementationType, ServiceLifetime lifetime) => new(serviceType, implementationType, lifetime);

    public static ServiceDescriptor Describe(Type? serviceType, Func<IServiceProvider, object>? implementationFactory, ServiceLifetime lifetime) => new(serviceType, implementationFactory, lifetime);
}