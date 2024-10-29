using System;
using System.Reflection;
using NSubstitute.Core;

namespace NSubstitute.Instantiation
{
    internal class LazySubstitute(ConstructorInfo constructor, Type type)
    {
        private readonly Lazy<object> _instance = new(() => Activate(constructor, type));

        public object Value => _instance.Value;

        private static object Activate(ConstructorInfo constructor, Type type)
            => type.IsInterface
                ? Substitute.For([type], [])
                : ActivateInstance(constructor, type);

        private static object ActivateInstance(ConstructorInfo constructor, Type type)
        {
            Type declaringType = constructor.DeclaringType;
            if (declaringType == type)
                throw new NotSupportedException(SR.Format(SR.CannotCreateInstanceOfRecursiveType, declaringType.GetDisplayName(full: true), constructor.GetDisplayName()));
            return Instance.Of(type);
        }
    }
}
