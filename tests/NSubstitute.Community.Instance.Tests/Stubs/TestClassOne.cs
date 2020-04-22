using System;
using System.Collections;

namespace NSubstitute.Tests.Stubs
{
    public class TestClassOne : TestBaseAbstractClassOne
    {
        public readonly Action<int> OnValue;

        public readonly IEnumerable Data;

        protected TestClassOne() : base(null) { }

        public TestClassOne(ITestInterfaceOne testInterface, IEnumerable data, Action<int> onValue = null)
            : base(testInterface)
        {
            OnValue = onValue;
            Data = data;
        }

        public int GetValue()
        {
            var value = TestInterface.Value;
            OnValue?.Invoke(value);
            return value;
        }
    }
}
