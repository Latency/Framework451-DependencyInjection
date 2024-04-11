// ****************************************************************************
// Project:  Microsoft.Extensions.DependencyInjection
// File:     Service.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************
// ReSharper disable UnusedMember.Local

using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Abstractions;
using Microsoft.Extensions.DependencyInjection.Properties;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup;

internal class Service(ServiceDescriptor descriptor) : IService
{
    public IService? Next { get; set; }

    public ServiceLifetime Lifetime => descriptor.Lifetime;

    public IServiceCallSite CreateCallSite(ServiceProvider provider, ISet<Type?> callSiteChain)
    {
        var constructors = descriptor.ImplementationType.GetTypeInfo().DeclaredConstructors.Where(constructor => constructor.IsPublic).ToArray();

        IServiceCallSite[]? parameterCallSites = null;

        switch (constructors.Length)
        {
            case 0:
                throw new InvalidOperationException(Resources.FormatNoConstructorMatch(descriptor.ImplementationType));
            case 1:
            {
                var constructor = constructors[0];
                var parameters  = constructor.GetParameters();
                if (parameters.Length == 0)
                    return new CreateInstanceCallSite(descriptor);

                parameterCallSites = PopulateCallSites(provider, callSiteChain, parameters, true);

                return new ConstructorCallSite(constructor, parameterCallSites);
            }
        }

        Array.Sort(constructors, (a, b) => b.GetParameters().Length.CompareTo(a.GetParameters().Length));

        ConstructorInfo? bestConstructor               = null;
        HashSet<Type>?   bestConstructorParameterTypes = null;
        foreach (var t in constructors)
        {
            var parameters = t.GetParameters();

            var currentParameterCallSites = PopulateCallSites(provider, callSiteChain, parameters, false);

            if (currentParameterCallSites != null)
            {
                if (bestConstructor == null)
                {
                    bestConstructor    = t;
                    parameterCallSites = currentParameterCallSites;
                }
                else
                {
                    // Since we're visiting constructors in decreasing order of number of parameters,
                    // we'll only see ambiguities or supersets once we've seen a 'bestConstructor'.

                    bestConstructorParameterTypes ??= [..bestConstructor.GetParameters().Select(p => p.ParameterType)];

                    if (!bestConstructorParameterTypes.IsSupersetOf(parameters.Select(p => p.ParameterType)))
                    {
                        // Ambigious match exception
                        var message = string.Join(Environment.NewLine, Resources.FormatAmbigiousConstructorException(descriptor.ImplementationType), bestConstructor, t);
                        throw new InvalidOperationException(message);
                    }
                }
            }
        }

        if (bestConstructor == null)
            throw new InvalidOperationException(Resources.FormatUnableToActivateTypeException(descriptor.ImplementationType));

        Debug.Assert(parameterCallSites != null);
        return parameterCallSites!.Length == 0 ? new CreateInstanceCallSite(descriptor) : new ConstructorCallSite(bestConstructor, parameterCallSites);
    }

    public Type? ServiceType => descriptor.ServiceType;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
    private bool IsSuperset(IEnumerable<Type> left, IEnumerable<Type> right) => new HashSet<Type>(left).IsSupersetOf(right);

    private IServiceCallSite[]? PopulateCallSites(ServiceProvider provider, ISet<Type?> callSiteChain, IReadOnlyList<ParameterInfo> parameters, bool throwIfCallSiteNotFound)
    {
        var parameterCallSites = new IServiceCallSite[parameters.Count];
        for (var index = 0; index < parameters.Count; index++)
        {
            var callSite = provider.GetServiceCallSite(parameters[index].ParameterType, callSiteChain);

            if (callSite == null && parameters[index].HasDefaultValue)
                callSite = new ConstantCallSite(parameters[index].DefaultValue);

            if (callSite == null)
                return throwIfCallSiteNotFound ? throw new InvalidOperationException(Resources.FormatCannotResolveService(parameters[index].ParameterType, descriptor.ImplementationType)) : null;

            parameterCallSites[index] = callSite;
        }

        return parameterCallSites;
    }
}