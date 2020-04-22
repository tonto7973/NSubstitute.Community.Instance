using System;
using System.Linq;
using NSubstitute.Core;
using NSubstitute.Instantiation;

namespace NSubstitute
{
    /// <summary>
    /// Provides functionality to instantiate a type with dependencies automatically substituted.
    /// For example: <c>Instance.Of&lt;SomeClass&gt;()</c>
    /// </summary>
    public static class Instance
    {
        private static readonly INameForSubstitute NameForSubstitute = new NameForSubstitute();

        /// <summary>
        /// Creates an instance of a type with dependencies automatically substituted.
        /// </summary>
        /// <typeparam name="TType">The type of a class to instantiate.</typeparam>
        /// <param name="dependencies">Optional dependencies used to instantiate the type. These will not be substituted.</param>
        /// <returns>An instance of the type.</returns>
        /// <remarks>Interfaces are not supported.</remarks>
        public static TType Of<TType>(params object[] dependencies)
            where TType : class
        {
            return (TType)Of(typeof(TType), dependencies);
        }

        /// <summary>
        /// Creates an instance of a type with dependencies automatically substituted.
        /// </summary>
        /// <param name="type">The type of a class to instantiate.</param>
        /// <param name="dependencies">Optional dependencies used to instantiate the type. These will not be substituted.</param>
        /// <returns>An instance of the type.</returns>
        /// <remarks>Interfaces are not supported.</remarks>
        public static object Of(Type type, params object[] dependencies)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (type.IsInterface)
                throw new MemberAccessException(SR.Format(SR.CannotCreateInstanceOfInterface, type.FullName));

            dependencies = dependencies ?? Array.Empty<object>();

            var firstNullValue = Array.IndexOf(dependencies, null);
            if (firstNullValue > -1)
                throw new ArgumentNullException($"{nameof(dependencies)}[{firstNullValue}]", SR.Format(SR.DependencyCannotBeNull));

            Activation activation = ActivationLookup
                .For(type, dependencies)
                .OrderBy(a => a.ConstructorInfo.IsPublic)
                .ThenBy(a => a.Match == ArgumentMatch.Exact)
                .ThenBy(a => a.ConstructorInfo.GetParameters().Length)
                .ThenBy(a => a.Match)
                .FirstOrDefault();

            if (activation != null)
                return activation.Invoke();

            var formattedExceptionMessage = dependencies.Length > 0
                ? SR.Format(SR.CannotFindMatchingAccessibleConstructor, type.FullName, NameForSubstitute.GetNamesFor(dependencies))
                : SR.Format(SR.CannotFindAccessibleConstructor, type.FullName);

            throw new MissingMethodException(formattedExceptionMessage);
        }

        /// <summary>
        /// Creates a null substitute of a type.
        /// </summary>
        /// <typeparam name="TType">The type to substitute as null.</typeparam>
        /// <returns>Null substitute for the type.</returns>
        public static INullValue Null<TType>()
            => new NullValue(typeof(TType));
    }
}
