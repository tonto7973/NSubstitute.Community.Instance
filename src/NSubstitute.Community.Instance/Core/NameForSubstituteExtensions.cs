#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Collections.Generic;
using System.Linq;

namespace NSubstitute.Core
{
    public static class NameForSubstituteExtensions
    {
        public static string GetNamesFor(this INameForSubstitute nameForSubstitute, IEnumerable<object> substitutes)
        {
            ArgumentNullException.ThrowIfNull(nameForSubstitute);

            if (substitutes == null)
                return string.Empty;

            return string.Join(", ", substitutes.Select(nameForSubstitute.GetNameFor));
        }
    }
}
