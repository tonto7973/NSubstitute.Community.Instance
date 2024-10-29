#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace NSubstitute.Core
{
    public partial class NameForSubstitute : INameForSubstitute
    {
        private static readonly Regex RxSubstitute
            = RxSubstituteTemplate();

        private static readonly Lazy<Assembly> DynamicProxyAssembly
            = new(() => Substitute.For<object>().GetType().Assembly);

        public string GetNameFor(object substitute)
        {
            if (substitute == null)
                return "?";
            if (substitute is INullValue)
                return "null";

            return TryGetNameForProxyType(substitute, out var typeName)
                ? typeName
                : substitute.GetType().GetDisplayName();
        }

        private static bool TryGetNameForProxyType(object substitute, out string typeName)
        {
            Type type = substitute.GetType();

            if (type.FullName.StartsWith("Castle.Proxies.", StringComparison.Ordinal)
                && type.Assembly.Equals(DynamicProxyAssembly.Value))
            {
                typeName = RxSubstitute.Replace(substitute.ToString(), "$1");
                return true;
            }

            typeName = null;
            return false;
        }

        [GeneratedRegex("^Substitute\\.(.*?)\\|[a-z0-9]+$", RegexOptions.Compiled)]
        private static partial Regex RxSubstituteTemplate();
    }
}
