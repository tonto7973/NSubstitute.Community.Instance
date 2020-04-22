namespace NSubstitute.Tests.Stubs
{
    public class TestClassFive
    {
        public TestSealedClass Dependency { get; }

        public TestClassFive(TestSealedClass dependency)
        {
            Dependency = dependency;
        }
    }
}
