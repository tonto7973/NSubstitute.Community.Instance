using System;
using System.Collections.Generic;
using System.Linq;

namespace NSubstitute.Tests.Stubs
{
    public class TestClassTwo
    {
        public ITestInterfaceOne TestInterface { get; }

        public IEnumerable<int> Values { get; }

        public TestClassTwo(ITestInterfaceOne testInterface, IEnumerable<int> values)
        {
            TestInterface = testInterface ?? throw new ArgumentNullException(nameof(testInterface));
            Values = values ?? throw new ArgumentNullException(nameof(values));
        }

        public TestClassTwo(ITestInterfaceOne testInterface)
        {
            TestInterface = testInterface ?? throw new ArgumentNullException(nameof(testInterface));
        }

        public int CombineValue()
            => Values?.FirstOrDefault() ?? 0 + TestInterface.Value;
    }
}
