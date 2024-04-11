// ****************************************************************************
// Project:  Microsoft.Extensions.DependencyInjection
// File:     ConcurrentDictionaryExtensions.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

using System.Collections.Concurrent;
using System.Diagnostics;

namespace Microsoft.Extensions.DependencyInjection.Internal;

internal static class ConcurrentDictionaryExtensions
{
    // From https://github.com/dotnet/corefx/issues/394#issuecomment-69494764
    // This lets us pass a state parameter allocation free GetOrAdd
    internal static TValue GetOrAdd<TKey, TValue, TArg>(this ConcurrentDictionary<TKey, TValue>? dictionary, TKey? key, Func<TKey, TArg, TValue>? valueFactory, TArg arg)
    {
        Debug.Assert(dictionary   != null);
        Debug.Assert(key          != null);
        Debug.Assert(valueFactory != null);

        while (true)
        {
            if (dictionary!.TryGetValue(key!, out var value))
                return value;

            value = valueFactory!(key!, arg);
            if (dictionary.TryAdd(key!, value))
                return value;
        }
    }
}