using System.IO;

namespace NSubstitute.Tests.Stubs
{
    internal class TestClassSeven
    {
        public TextWriter TextWriter { get; }

        protected internal TestClassSeven(TextWriter textWriter)
        {
            TextWriter = textWriter;
        }
    }
}
