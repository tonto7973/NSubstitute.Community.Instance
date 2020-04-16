#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace NSubstitute.Core
{
    public interface INameForSubstitute
    {
        string GetNameFor(object substitute);
    }
}
