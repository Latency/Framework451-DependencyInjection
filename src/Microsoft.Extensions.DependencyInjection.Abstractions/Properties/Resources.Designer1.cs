// ****************************************************************************
// Project:  Microsoft.Extensions.DependencyInjection.Abstractions
// File:     Resources.Designer1.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

using System.Diagnostics;
using System.Globalization;
using System.Resources;

namespace Microsoft.Extensions.DependencyInjection.Abstractions.Properties;

internal partial class Resources
{
    private static readonly ResourceManager _resourceManager = new("Net451.Microsoft.Extensions.DependencyInjection.Abstractions.Resources", typeof(Resources).Assembly);


    /// <summary>
    ///     No service for type '{0}' has been registered.
    /// </summary>
    internal static string FormatNoServiceRegistered(object? p0) => string.Format(CultureInfo.CurrentCulture, GetString("NoServiceRegistered"), p0);


    /// <summary>
    ///     Implementation type cannot be '{0}' because it is indistinguishable from other services registered for '{1}'.
    /// </summary>
    internal static string FormatTryAddIndistinguishableTypeToEnumerable(object? p0, object? p1) => string.Format(CultureInfo.CurrentCulture, GetString("TryAddIndistinguishableTypeToEnumerable"), p0, p1);


    private static string GetString(string name, params string[]? formatterNames)
    {
        var value = _resourceManager.GetString(name);

        Debug.Assert(value != null);

        if (formatterNames != null)
            for (var i = 0; i < formatterNames.Length; i++)
                value = value!.Replace("{" + formatterNames[i] + "}", "{" + i + "}");

        return value ?? string.Empty;
    }
}