using System;
using System.Reflection;
using NSubstitute.Core;

namespace NSubstitute.Instantiation
{
    internal class LazySubstitute
    {
        private readonly Lazy<object> _instance;

        public object Value => _instance.Value;

        public LazySubstitute(ConstructorInfo constructor, Type type)
        {
            _instance = new Lazy<object>(() => Activate(constructor, type));
        }

        private static object Activate(ConstructorInfo constructor, Type type)
            => type.IsInterface
                ? Substitute.For(new[] { type }, Array.Empty<object>())
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
