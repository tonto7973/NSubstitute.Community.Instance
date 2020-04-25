namespace NSubstitute.Tests.Stubs
{
    public class TestStackClass
    {
        public TestStackClass Base { get; }
        public int Age { get; }

        public TestStackClass(TestStackClass @base, int age)
        {
            Base = @base;
            Age = age;
        }
    }
}
