using System;

namespace NSubstitute.Tests.Stubs
{
    public class TestClassThree(TestClassTwo testClassTwo)
    {
        public TestClassTwo TestClassTwo { get; } = testClassTwo ?? throw new ArgumentNullException(nameof(testClassTwo));
    }
}
