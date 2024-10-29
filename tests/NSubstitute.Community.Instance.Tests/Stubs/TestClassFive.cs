namespace NSubstitute.Tests.Stubs
{
    public class TestClassFive(TestSealedClass dependency)
    {
        public TestSealedClass Dependency { get; } = dependency;
    }
}
