// ****************************************************************************
// Project:  DependencyInjection.ConsoleApp.Example
// File:     Program.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

using System;
using Microsoft.Extensions.DependencyInjection.Abstractions;
using Microsoft.Extensions.DependencyInjection.ConsoleApp;

namespace DependencyInjection.ConsoleApp.Example;

internal class Program
{
    private static void Main()
    {
        //Init the DI container
        DependencyInjectionStartup.Initialize(services =>
        {
            services.AddSingleton<IDao, Dao>();
        });

        //Get instnace by service type from DI container
        var dao = DIProviderInstance.ProviderInstance.GetRequiredService<IDao>();

        Console.WriteLine(dao.GetWriter());
    }
}