// ****************************************************************************
// Project:  Microsoft.Extensions.DependencyInjection
// File:     ServiceCollection.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

using System.Collections;
using Microsoft.Extensions.DependencyInjection.Abstractions;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
///     Default implementation of <see cref="IServiceCollection" />.
/// </summary>
public class ServiceCollection : IServiceCollection
{
    private readonly List<ServiceDescriptor> _descriptors = [];

    /// <inheritdoc />
    public int Count => _descriptors.Count;

    /// <inheritdoc />
    public bool IsReadOnly => false;

    public ServiceDescriptor this[int index]
    {
        get => _descriptors[index];
        set => _descriptors[index] = value;
    }

    /// <inheritdoc />
    public void Clear() => _descriptors.Clear();

    /// <inheritdoc />
    public bool Contains(ServiceDescriptor item) => _descriptors.Contains(item);

    /// <inheritdoc />
    public void CopyTo(ServiceDescriptor[] array, int arrayIndex) => _descriptors.CopyTo(array, arrayIndex);

    /// <inheritdoc />
    public bool Remove(ServiceDescriptor item) => _descriptors.Remove(item);

    /// <inheritdoc />
    public IEnumerator<ServiceDescriptor> GetEnumerator() => _descriptors.GetEnumerator();

    void ICollection<ServiceDescriptor>.Add(ServiceDescriptor item) => _descriptors.Add(item);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public int IndexOf(ServiceDescriptor item) => _descriptors.IndexOf(item);

    public void Insert(int index, ServiceDescriptor item) => _descriptors.Insert(index, item);

    public void RemoveAt(int index) => _descriptors.RemoveAt(index);
}