namespace NSubstitute.Tests.Stubs
{
    public sealed class TestSealedClass
    {
        public readonly static TestSealedClass Instance = new();

        private TestSealedClass() { }
    }
}
