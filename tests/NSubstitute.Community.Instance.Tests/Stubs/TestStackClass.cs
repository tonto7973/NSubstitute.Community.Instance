using System.Linq;

namespace NSubstitute.Tests.Stubs
{
    public class TestStackClass(TestStackClass @base, int[] age, bool? solid)
    {
        public TestStackClass Base { get; } = @base;

        public int Age { get; } = age?.FirstOrDefault() ?? 0;

        public bool? Solid { get; } = solid;
    }
}
