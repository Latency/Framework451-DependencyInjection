// ****************************************************************************
// Project:  Microsoft.Extensions.DependencyInjection.Abstractions
// File:     IServiceScope.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

namespace Microsoft.Extensions.DependencyInjection.Abstractions;

/// <summary>
///     The <see cref="System.IDisposable.Dispose" /> method ends the scope lifetime. Once Dispose
///     is called, any scoped services that have been resolved from
///     <see cref="ServiceProvider" /> will be
///     disposed.
/// </summary>
public interface IServiceScope : IDisposable
{
    /// <summary>
    ///     The <see cref="System.IServiceProvider" /> used to resolve dependencies from the scope.
    /// </summary>
    IServiceProvider ServiceProvider { get; }
}