// ****************************************************************************
// Project:  Microsoft.Extensions.DependencyInjection.Abstractions
// File:     ParameterDefaultValue.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection.Abstractions.Internal;

internal class ParameterDefaultValue
{
    public static bool TryGetDefaultValue(ParameterInfo parameter, out object? defaultValue)
    {
        var flag = true;
        defaultValue = null;
        bool flag2;
        try
        {
            flag2 = parameter.HasDefaultValue;
        }
        catch (FormatException) when (parameter.ParameterType == typeof(DateTime))
        {
            flag2 = true;
            flag  = false;
        }

        if (flag2)
        {
            if (flag)
                defaultValue = parameter.DefaultValue;
            if (defaultValue == null && parameter.ParameterType.IsValueType)
                defaultValue = Activator.CreateInstance(parameter.ParameterType);
        }

        return flag2;
    }
}