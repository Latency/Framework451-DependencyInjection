// ****************************************************************************
// Project:  Microsoft.Extensions.DependencyInjection
// File:     GenericService.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Abstractions;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup;

internal class GenericService(ServiceDescriptor descriptor) : IGenericService
{
    public ServiceLifetime Lifetime => descriptor.Lifetime;

    public IService GetService(Type? closedServiceType)
    {
        Type[] genericArguments         = closedServiceType.GetTypeInfo().GenericTypeArguments;
        var    closedImplementationType = descriptor.ImplementationType?.MakeGenericType(genericArguments);

        var closedServiceDescriptor = new ServiceDescriptor(closedServiceType, closedImplementationType, Lifetime);
        return new Service(closedServiceDescriptor);
    }
}