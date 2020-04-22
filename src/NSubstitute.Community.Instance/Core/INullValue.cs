using System;

namespace NSubstitute.Core
{
    /// <summary>
    /// Represents a null value.
    /// </summary>
    public interface INullValue
    {
        /// <summary>
        /// The type this null value represents.
        /// </summary>
        Type Type { get; }
    }
}
