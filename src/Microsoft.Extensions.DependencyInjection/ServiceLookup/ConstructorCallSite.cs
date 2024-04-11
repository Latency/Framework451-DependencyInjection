// ****************************************************************************
// Project:  Microsoft.Extensions.DependencyInjection
// File:     ConstructorCallSite.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup;

internal class ConstructorCallSite(ConstructorInfo constructorInfo, IServiceCallSite[]? parameterCallSites) : IServiceCallSite
{
    internal ConstructorInfo    ConstructorInfo     { get; } = constructorInfo;
    internal IServiceCallSite[]? ParameterCallSites { get; } = parameterCallSites;
}