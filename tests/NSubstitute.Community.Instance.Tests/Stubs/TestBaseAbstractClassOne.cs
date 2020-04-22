namespace NSubstitute.Tests.Stubs
{
    public abstract class TestBaseAbstractClassOne
    {
        public ITestInterfaceOne TestInterface { get; }

        protected TestBaseAbstractClassOne(ITestInterfaceOne testInterface)
        {
            TestInterface = testInterface;
        }
    }
}
