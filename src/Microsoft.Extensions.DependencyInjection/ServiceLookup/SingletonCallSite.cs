// ****************************************************************************
// Project:  Microsoft.Extensions.DependencyInjection
// File:     SingletonCallSite.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup;

internal class SingletonCallSite(IService key, IServiceCallSite serviceCallSite) : ScopedCallSite(key, serviceCallSite);