// ****************************************************************************
// Project:  Microsoft.Extensions.DependencyInjection
// File:     ServiceEntry.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup;

internal class ServiceEntry(IService service)
{
    private readonly object _sync = new();

    public IService First { get; private set; } = service;
    public IService Last  { get; private set; } = service;

    public void Add(IService service)
    {
        lock (_sync)
        {
            Last.Next = service;
            Last = service;
        }
    }
}