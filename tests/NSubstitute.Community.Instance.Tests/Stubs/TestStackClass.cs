using System.Linq;

namespace NSubstitute.Tests.Stubs
{
    public class TestStackClass
    {
        public TestStackClass Base { get; }

        public int Age { get; }

        public bool? Solid { get; }

        public TestStackClass(TestStackClass @base, int[] age, bool? solid)
        {
            Base = @base;
            Age = age?.FirstOrDefault() ?? 0;
            Solid = solid;
        }
    }
}
