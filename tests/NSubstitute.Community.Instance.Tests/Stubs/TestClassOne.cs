using System;
using System.Collections;

namespace NSubstitute.Tests.Stubs
{
    public class TestClassOne : TestBaseClassOne
    {
        private readonly Action<int> _onValue;

        protected TestClassOne() : base(null) { }

        public TestClassOne(ITestInterfaceOne testInterface, IEnumerable data, Action<int> onValue = null)
            : base(testInterface)
        {
            _onValue = onValue;
            System.Diagnostics.Debug.WriteLine($"data={data}");
        }

        public int GetValue()
        {
            var value = TestInterface.Value;
            _onValue?.Invoke(value);
            return value;
        }
    }
}
