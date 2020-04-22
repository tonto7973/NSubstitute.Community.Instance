namespace NSubstitute.Tests.Stubs
{
    public sealed class TestSealedClass
    {
        public static TestSealedClass Instance = new TestSealedClass();

        private TestSealedClass() { }
    }
}
