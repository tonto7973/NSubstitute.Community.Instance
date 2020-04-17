using System;
using System.Collections.Generic;

namespace NSubstitute.Tests.Stubs
{
    public class TestClassFour<T>
    {
        protected TestClassFour(ISet<T> set)
        {
            Set = set ?? throw new ArgumentNullException(nameof(set));
        }

        public ISet<T> Set { get; }
    }
}
