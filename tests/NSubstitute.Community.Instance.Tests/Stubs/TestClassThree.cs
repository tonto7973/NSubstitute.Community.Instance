using System;

namespace NSubstitute.Tests.Stubs
{
    public class TestClassThree
    {
        public TestClassTwo TestClassTwo { get; }

        public TestClassThree(TestClassTwo testClassTwo)
        {
            TestClassTwo = testClassTwo ?? throw new ArgumentNullException(nameof(testClassTwo));
        }
    }
}
