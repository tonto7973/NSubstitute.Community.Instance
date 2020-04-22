using System;

namespace NSubstitute.Core
{
    internal sealed class NullValue : INullValue
    {
        public Type Type { get; }

        internal NullValue(Type type)
        {
            Type = type;
        }
    }
}
