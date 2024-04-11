// ****************************************************************************
// Project:  Microsoft.Extensions.DependencyInjection
// File:     ServiceTable.cs
// Author:   Latency McLaughlin
// Date:     09/19/2023
// ****************************************************************************

using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Abstractions;
using Microsoft.Extensions.DependencyInjection.Properties;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup;

internal class ServiceTable
{
    private readonly Dictionary<Type, List<IGenericService>> _genericServices;
    private readonly Dictionary<Type, ServiceEntry>          _services;
    private readonly object                                  _sync = new();

    public ServiceTable(IEnumerable<ServiceDescriptor> descriptors)
    {
        _services        = [];
        _genericServices = [];

        foreach (var descriptor in descriptors)
        {
            var serviceTypeInfo = descriptor.ServiceType.GetTypeInfo();
            if (serviceTypeInfo.IsGenericTypeDefinition)
            {
                var implementationTypeInfo = descriptor.ImplementationType?.GetTypeInfo();

                if (implementationTypeInfo is not { IsGenericTypeDefinition: true })
                    throw new ArgumentException(Resources.FormatOpenGenericServiceRequiresOpenGenericImplementation(descriptor.ServiceType), nameof(descriptors));

                if (implementationTypeInfo.IsAbstract || implementationTypeInfo.IsInterface)
                    throw new ArgumentException(Resources.FormatTypeCannotBeActivated(descriptor.ImplementationType, descriptor.ServiceType));

                Add(descriptor.ServiceType!, new GenericService(descriptor));
            }
            else if (descriptor.ImplementationInstance != null)
            {
                Add(descriptor.ServiceType!, new InstanceService(descriptor));
            }
            else if (descriptor.ImplementationFactory != null)
            {
                Add(descriptor.ServiceType!, new FactoryService(descriptor));
            }
            else
            {
                Debug.Assert(descriptor.ImplementationType != null);
                var implementationTypeInfo = descriptor.ImplementationType.GetTypeInfo();

                if (implementationTypeInfo.IsGenericTypeDefinition || implementationTypeInfo.IsAbstract || implementationTypeInfo.IsInterface)
                    throw new ArgumentException(Resources.FormatTypeCannotBeActivated(descriptor.ImplementationType, descriptor.ServiceType));

                Add(descriptor.ServiceType!, new Service(descriptor));
            }
        }
    }

    public ConcurrentDictionary<Type, Func<ServiceProvider, object?>> RealizedServices { get; } = new();

    public bool TryGetEntry(Type serviceType, out ServiceEntry entry)
    {
        lock (_sync)
        {
            if (_services.TryGetValue(serviceType, out entry))
                return true;

            // ReSharper disable once InvertIf
            if (serviceType.GetTypeInfo().IsGenericType)
            {
                var openServiceType = serviceType.GetGenericTypeDefinition();

                // ReSharper disable once InvertIf
                if (_genericServices.TryGetValue(openServiceType, out var genericEntry))
                {
                    foreach (var closedService in genericEntry.Select(genericService => genericService.GetService(serviceType)).OfType<IService>())
                        Add(serviceType, closedService);

                    return _services.TryGetValue(serviceType, out entry);
                }
            }
        }

        return false;
    }

    public void Add(Type serviceType, IService service)
    {
        lock (_sync)
        {
            if (_services.TryGetValue(serviceType, out var entry))
                entry.Add(service);
            else
                _services[serviceType] = new(service);
        }
    }

    public void Add(Type serviceType, IGenericService genericService)
    {
        lock (_sync)
        {
            if (!_genericServices.TryGetValue(serviceType, out var genericEntry))
            {
                genericEntry                  = [];
                _genericServices[serviceType] = genericEntry;
            }

            genericEntry.Add(genericService);
        }
    }
}