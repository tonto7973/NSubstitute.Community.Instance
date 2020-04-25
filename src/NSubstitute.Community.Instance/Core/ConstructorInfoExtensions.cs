using System.Reflection;
using System.Text;

namespace NSubstitute.Core
{
    internal static class ConstructorInfoExtensions
    {
        internal static string GetDisplayName(this ConstructorInfo constructor)
        {
            ParameterInfo[] parameters = constructor.GetParameters();

            var builder = new StringBuilder();
            builder.Append(constructor.Name);
            builder.Append('(');

            for (int i = 0, length = parameters.Length; i < length; i++)
            {
                builder.Append(parameters[i].ParameterType.GetDisplayName());
                if (i + 1 < length)
                {
                    builder.Append(',');
                }
            }

            builder.Append(')');
            return builder.ToString();
        }
    }
}
