// ****************************************************************************
// Project:  Microsoft.Extensions.DependencyInjection
// File:     DefaultServiceProviderFactory.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

using Microsoft.Extensions.DependencyInjection.Abstractions;

namespace Microsoft.Extensions.DependencyInjection;

public class DefaultServiceProviderFactory : IServiceProviderFactory<IServiceCollection>
{
    public IServiceCollection CreateBuilder(IServiceCollection services) => services;

    public IServiceProvider CreateServiceProvider(IServiceCollection containerBuilder) => containerBuilder.BuildServiceProvider();
}