namespace NSubstitute.Tests.Stubs
{
    public abstract class TestBaseClassOne
    {
        protected ITestInterfaceOne TestInterface { get; }

        protected TestBaseClassOne(ITestInterfaceOne testInterface)
        {
            TestInterface = testInterface;
        }
    }
}
