// ****************************************************************************
// Project:  Microsoft.Extensions.DependencyInjection.Abstractions
// File:     ActivatorUtilities.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace Microsoft.Extensions.DependencyInjection.Abstractions.Internal;

internal static class ActivatorUtilities
{
    private static readonly MethodInfo GetServiceInfo = GetMethodInfo((Expression<Func<IServiceProvider, Type, Type, bool, object?>>)((sp, t, r, c) => GetService(sp, t, r, c)));

    internal static object CreateInstance(IServiceProvider provider, Type instanceType, params object[] parameters)
    {
        var                 num                = -1;
        ConstructorMatcher? constructorMatcher = null;
        if (!instanceType.GetTypeInfo().IsAbstract)
            foreach (var item in from constructor in instanceType.GetTypeInfo().DeclaredConstructors.Where(c => !c.IsStatic && c.IsPublic) select new ConstructorMatcher(constructor))
            {
                var num2 = item.Match(parameters);
                if (num2 != -1 && num < num2)
                {
                    num                = num2;
                    constructorMatcher = item;
                }
            }

        return constructorMatcher == null ? throw new InvalidOperationException($"A suitable constructor for type '{instanceType}' could not be located. Ensure the type is concrete and services are registered for all parameters of a public constructor.") : constructorMatcher.CreateInstance(provider);
    }

    internal static ObjectFactory CreateFactory(Type instanceType, Type[] argumentTypes)
    {
        FindApplicableConstructor(instanceType, argumentTypes, out var constructor, out var parameterMap);
        var parameterExpression  = Expression.Parameter(typeof(IServiceProvider), "provider");
        var parameterExpression2 = Expression.Parameter(typeof(object[]),         "argumentArray");
        return Expression.Lambda<Func<IServiceProvider, object[], object>>(BuildFactoryExpression(constructor!, parameterMap!, parameterExpression, parameterExpression2), parameterExpression, parameterExpression2).Compile().Invoke;
    }

    public static T CreateInstance<T>(IServiceProvider provider, params object[] parameters) => (T)CreateInstance(provider, typeof(T), parameters);

    public static T GetServiceOrCreateInstance<T>(IServiceProvider provider) => (T)GetServiceOrCreateInstance(provider, typeof(T));

    public static object GetServiceOrCreateInstance(IServiceProvider provider, Type type) => provider.GetService(type) ?? CreateInstance(provider, type, new List<object>());

    private static MethodInfo GetMethodInfo<T>(Expression<T> expr) => ((MethodCallExpression)expr.Body).Method;

    private static object? GetService(IServiceProvider sp, Type type, Type requiredBy, bool isDefaultParameterRequired)
    {
        var service = sp.GetService(type);
        return service == null && !isDefaultParameterRequired
            ? throw new InvalidOperationException($"Unable to resolve service for type '{type}' while attempting to activate '{requiredBy}'.")
            : service;
    }

    private static Expression BuildFactoryExpression(ConstructorInfo constructor, IReadOnlyList<int?> parameterMap, Expression serviceProvider, Expression factoryArgumentArray)
    {
        var parameters = constructor.GetParameters();
        var array      = new Expression[parameters.Length];
        for (var i = 0; i < parameters.Length; i++)
        {
            var obj           = parameters[i];
            var parameterType = obj.ParameterType;
            var flag          = ParameterDefaultValue.TryGetDefaultValue(obj, out var value);
            if (parameterMap[i].HasValue)
            {
                array[i] = Expression.ArrayAccess(factoryArgumentArray, Expression.Constant(parameterMap[i]));
            }
            else
            {
                var arguments = new[]
                {
                    serviceProvider,
                    Expression.Constant(parameterType,             typeof(Type)),
                    Expression.Constant(constructor.DeclaringType, typeof(Type)),
                    Expression.Constant(flag)
                };
                array[i] = Expression.Call(GetServiceInfo, arguments);
            }

            if (flag)
            {
                var right = Expression.Constant(value);
                array[i] = Expression.Coalesce(array[i], right);
            }

            array[i] = Expression.Convert(array[i], parameterType);
        }

        return Expression.New(constructor, array);
    }

    private static void FindApplicableConstructor(Type instanceType, Type[] argumentTypes, out ConstructorInfo? matchingConstructor, out int?[]? parameterMap)
    {
        matchingConstructor = null;
        parameterMap        = null;
        foreach (var declaredConstructor in instanceType.GetTypeInfo().DeclaredConstructors)
            if (!declaredConstructor.IsStatic && declaredConstructor.IsPublic && TryCreateParameterMap(declaredConstructor.GetParameters(), argumentTypes, out var array))
            {
                if (matchingConstructor != null)
                    throw new InvalidOperationException($"Multiple constructors accepting all given argument types have been found in type '{instanceType}'. There should only be one applicable constructor.");
                matchingConstructor = declaredConstructor;
                parameterMap        = array;
            }

        if (matchingConstructor is not null)
            return;
        throw new InvalidOperationException($"A suitable constructor for type '{instanceType}' could not be located. Ensure the type is concrete and services are registered for all parameters of a public constructor.");
    }

    private static bool TryCreateParameterMap(ParameterInfo[] constructorParameters, Type[] argumentTypes, out int?[] parameterMap)
    {
        parameterMap = new int?[constructorParameters.Length];
        for (var i = 0; i < argumentTypes.Length; i++)
        {
            var flag     = false;
            var typeInfo = argumentTypes[i].GetTypeInfo();
            var num      = 0;
            while (num < constructorParameters.Length)
            {
                if (parameterMap[num].HasValue || !constructorParameters[num].ParameterType.GetTypeInfo().IsAssignableFrom(typeInfo))
                {
                    num++;
                    continue;
                }

                flag              = true;
                parameterMap[num] = i;
                break;
            }

            if (!flag)
                return false;
        }

        return true;
    }

    private class ConstructorMatcher
    {
        private readonly ConstructorInfo _constructor;

        private readonly ParameterInfo[] _parameters;

        private readonly object?[] _parameterValues;

        private readonly bool[] _parameterValuesSet;

        public ConstructorMatcher(ConstructorInfo constructor)
        {
            _constructor        = constructor;
            _parameters         = _constructor.GetParameters();
            _parameterValuesSet = new bool[_parameters.Length];
            _parameterValues    = new object[_parameters.Length];
        }

        public int Match(object?[] givenParameters)
        {
            var num    = 0;
            var result = 0;
            for (var i = 0; i != givenParameters.Length; i++)
            {
                var obj      = givenParameters[i];
                var typeInfo = obj?.GetType().GetTypeInfo();
                var flag     = false;
                var num2     = num;
                while (!flag && num2 != _parameters.Length)
                {
                    if (!_parameterValuesSet[num2] && _parameters[num2].ParameterType.GetTypeInfo().IsAssignableFrom(typeInfo))
                    {
                        flag                      = true;
                        _parameterValuesSet[num2] = true;
                        _parameterValues[num2]    = givenParameters[i];
                        if (num == num2)
                        {
                            num++;
                            if (num2 == i)
                                result = num2;
                        }
                    }

                    num2++;
                }

                if (!flag)
                    return -1;
            }

            return result;
        }

        public object CreateInstance(IServiceProvider provider)
        {
            for (var i = 0; i != _parameters.Length; i++)
                if (!_parameterValuesSet[i])
                {
                    var service = provider.GetService(_parameters[i].ParameterType);
                    if (service == null)
                    {
                        if (!ParameterDefaultValue.TryGetDefaultValue(_parameters[i], out var obj))
                            throw new InvalidOperationException($"Unable to resolve service for type '{_parameters[i].ParameterType}' while attempting to activate '{_constructor.DeclaringType}'.");
                        _parameterValues[i] = obj;
                    }
                    else
                    {
                        _parameterValues[i] = service;
                    }
                }

            try
            {
                return _constructor.Invoke(_parameterValues);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }
    }
}