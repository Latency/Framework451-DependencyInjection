// ****************************************************************************
// Project:  Microsoft.Extensions.DependencyInjection.ConsoleApp
// File:     DependencyInjectionStartup.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

using Microsoft.Extensions.DependencyInjection.Abstractions;

namespace Microsoft.Extensions.DependencyInjection.ConsoleApp;

public class DependencyInjectionStartup
{
    public static void Initialize(Action<IServiceCollection> action)
    {
        IServiceCollection services = new ServiceCollection();
        action(services);
        DIProviderInstance.SetProviderInstance(services);
    }
}