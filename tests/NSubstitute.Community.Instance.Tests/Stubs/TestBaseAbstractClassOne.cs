namespace NSubstitute.Tests.Stubs
{
    public abstract class TestBaseAbstractClassOne(ITestInterfaceOne testInterface)
    {
        public ITestInterfaceOne TestInterface { get; } = testInterface;
    }
}
