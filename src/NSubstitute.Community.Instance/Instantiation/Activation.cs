using System.Reflection;

namespace NSubstitute.Instantiation
{
    internal class Activation
    {
        public ConstructorInfo ConstructorInfo { get; set; }

        public object[] Arguments { get; set; }

        public ArgumentMatch Match { get; set; }
    }
}