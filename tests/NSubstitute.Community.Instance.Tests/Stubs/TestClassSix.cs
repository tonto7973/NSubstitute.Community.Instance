namespace NSubstitute.Tests.Stubs
{
    public class TestClassSix
    {
        public TestBaseAbstractClassOne AbstractClass { get; }

        public ITestInterfaceOne InterfaceOne { get; }

        internal TestClassSix(TestBaseAbstractClassOne abstractClass, ITestInterfaceOne interfaceOne)
        {
            AbstractClass = abstractClass;
            InterfaceOne = interfaceOne;
        }
    }
}
