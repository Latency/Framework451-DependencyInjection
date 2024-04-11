// ****************************************************************************
// Project:  Microsoft.Extensions.DependencyInjection.Abstractions
// File:     ObjectFactory.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

namespace Microsoft.Extensions.DependencyInjection.Abstractions.Internal;

internal delegate object ObjectFactory(IServiceProvider serviceProvider, object[] arguments);