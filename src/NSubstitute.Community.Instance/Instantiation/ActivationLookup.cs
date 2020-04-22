using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using NSubstitute.Core;

namespace NSubstitute.Instantiation
{
    internal static class ActivationLookup
    {
        private static readonly DefaultForType DefaultValues = new DefaultForType();

        internal static IEnumerable<Activation> For(Type type, object[] constructorArguments)
        {
            IEnumerable<ConstructorInfo> constructors = type
                .GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance | BindingFlags.Instance)
                .Where(ConstructorIsAccessible);

            return FindMatchingConstructors(constructors, constructorArguments);
        }

        private static bool ConstructorIsAccessible(ConstructorInfo constructor)
            => constructor.IsPublic || constructor.IsFamily || constructor.IsFamilyOrAssembly
                || (constructor.IsAssembly && ProxyUtil.IsAccessible(constructor));

        private static IEnumerable<Activation> FindMatchingConstructors(
            this IEnumerable<ConstructorInfo> constructors,
            object[] constructorArguments)
        {
            foreach (ConstructorInfo constructor in constructors)
            {
                if (constructor.TryFindExactMatch(constructorArguments, out Activation exactInstanceInfo))
                    yield return exactInstanceInfo;
                else if (constructor.TryFindPartialMatch(constructorArguments, out Activation partialInstanceInfo))
                    yield return partialInstanceInfo;
            }
        }

        private static bool TryFindExactMatch(this ConstructorInfo constructor, object[] constructorArguments, out Activation instanceInfo)
        {
            ParameterInfo[] parameters = constructor.GetParameters();
            var exactMatchFound = parameters.Length == constructorArguments.Length
                && (parameters.Length == 0 ||
                    parameters
                        .Select((param, i) => param.IsInstanceOf(constructorArguments[i]))
                        .All(x => x));
            instanceInfo = exactMatchFound
                ? new Activation { Arguments = constructorArguments, ConstructorInfo = constructor, Match = ArgumentMatch.Exact }
                : null;
            return exactMatchFound;
        }

        private static bool TryFindPartialMatch(this ConstructorInfo constructor, object[] constructorArguments, out Activation instanceInfo)
        {
            ParameterInfo[] parameters = constructor.GetParameters();
            var paramIndex = 0;
            var newArguments = new object[parameters.Length];
            var args = new List<object>(constructorArguments);
            var exactOrder = true;
            foreach (ParameterInfo param in parameters)
            {
                if (TryUseArgument(param, args, out var arg, ref exactOrder))
                    newArguments[paramIndex] = arg;
                else if (param.HasDefaultValue)
                    newArguments[paramIndex] = param.DefaultValue;
                else if (TryUseSubstitute(param, out var substitute))
                    newArguments[paramIndex] = substitute;
                else
                    newArguments[paramIndex] = DefaultValues.GetDefaultFor(param.ParameterType);
                paramIndex++;
            }

            var partialMatchFound = args.Count == 0;
            ArgumentMatch match = exactOrder ? ArgumentMatch.Partial : ArgumentMatch.Any;
            instanceInfo = partialMatchFound
                ? new Activation { Arguments = newArguments, ConstructorInfo = constructor, Match = match }
                : null;
            return partialMatchFound;
        }

        private static bool TryUseArgument(ParameterInfo param, List<object> args, out object arg, ref bool exactOrder)
        {
            for (var i = 0; i < args.Count; i++)
            {
                arg = args[i];
                if (param.IsInstanceOf(arg))
                {
                    args.RemoveAt(i);
                    if (i > 0)
                        exactOrder = false;
                    return true;
                }
            }

            arg = null;
            return false;
        }

        private static bool TryUseSubstitute(ParameterInfo param, out object substitute)
        {
            try
            {
                substitute = param.ParameterType.IsInterface
                    ? Substitute.For(new[] { param.ParameterType }, null)
                    : Instance.Of(param.ParameterType);
                return true;
            }
            catch
            {
                substitute = null;
                return false;
            }
        }

        private static bool IsInstanceOf(this ParameterInfo param, object arg) => arg is INullValue nullValue
                ? param.ParameterType.IsAssignableFrom(nullValue.Type)
                : param.ParameterType.IsInstanceOfType(arg);
    }
}
