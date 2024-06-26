// ****************************************************************************
// Project:  Microsoft.Extensions.DependencyInjection.Abstractions
// File:     ActivatorUtilities.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

namespace Microsoft.Extensions.DependencyInjection.Abstractions;

/// <summary>
///     Helper code for the various activator services.
/// </summary>
public static class ActivatorUtilities
{
    /// <summary>
    ///     Instantiate a type with constructor arguments provided directly and/or from an <see cref="IServiceProvider" />.
    /// </summary>
    /// <param name="provider">The service provider used to resolve dependencies</param>
    /// <param name="instanceType">The type to activate</param>
    /// <param name="parameters">Constructor arguments not provided by the <paramref name="provider" />.</param>
    /// <returns>An activated object of type instanceType</returns>
    public static object CreateInstance(IServiceProvider provider, Type instanceType, params object[] parameters) => Internal.ActivatorUtilities.CreateInstance(provider, instanceType, parameters);

    /// <summary>
    ///     Create a delegate that will instantiate a type with constructor arguments provided directly
    ///     and/or from an <see cref="IServiceProvider" />.
    /// </summary>
    /// <param name="instanceType">The type to activate</param>
    /// <param name="argumentTypes">
    ///     The types of objects, in order, that will be passed to the returned function as its second parameter
    /// </param>
    /// <returns>
    ///     A factory that will instantiate instanceType using an <see cref="IServiceProvider" />
    ///     and an argument array containing objects matching the types defined in argumentTypes
    /// </returns>
    public static ObjectFactory CreateFactory(Type instanceType, Type[] argumentTypes) => new(Internal.ActivatorUtilities.CreateFactory(instanceType, argumentTypes));

    /// <summary>
    ///     Instantiate a type with constructor arguments provided directly and/or from an <see cref="IServiceProvider" />.
    /// </summary>
    /// <typeparam name="T">The type to activate</typeparam>
    /// <param name="provider">The service provider used to resolve dependencies</param>
    /// <param name="parameters">Constructor arguments not provided by the <paramref name="provider" />.</param>
    /// <returns>An activated object of type T</returns>
    public static T CreateInstance<T>(IServiceProvider provider, params object[] parameters) => Internal.ActivatorUtilities.CreateInstance<T>(provider, parameters);

    /// <summary>
    ///     Retrieve an instance of the given type from the service provider. If one is not found then instantiate it directly.
    /// </summary>
    /// <typeparam name="T">The type of the service</typeparam>
    /// <param name="provider">The service provider used to resolve dependencies</param>
    /// <returns>The resolved service or created instance</returns>
    public static T GetServiceOrCreateInstance<T>(IServiceProvider provider) => Internal.ActivatorUtilities.GetServiceOrCreateInstance<T>(provider);

    /// <summary>
    ///     Retrieve an instance of the given type from the service provider. If one is not found then instantiate it directly.
    /// </summary>
    /// <param name="provider">The service provider</param>
    /// <param name="type">The type of the service</param>
    /// <returns>The resolved service or created instance</returns>
    public static object GetServiceOrCreateInstance(IServiceProvider provider, Type type) => Internal.ActivatorUtilities.GetServiceOrCreateInstance(provider, type);
}