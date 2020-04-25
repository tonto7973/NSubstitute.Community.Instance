using System;
using System.Collections.Generic;
using System.Text;

namespace NSubstitute.Core
{
    internal static class TypeExtensions
    {
        private static readonly Dictionary<Type, string> BuiltInTypeNames = new Dictionary<Type, string>
        {
            [typeof(void)] = "void",
            [typeof(bool)] = "bool",
            [typeof(byte)] = "byte",
            [typeof(char)] = "char",
            [typeof(decimal)] = "decimal",
            [typeof(double)] = "double",
            [typeof(float)] = "float",
            [typeof(int)] = "int",
            [typeof(long)] = "long",
            [typeof(object)] = "object",
            [typeof(sbyte)] = "sbyte",
            [typeof(short)] = "short",
            [typeof(string)] = "string",
            [typeof(uint)] = "uint",
            [typeof(ulong)] = "ulong",
            [typeof(ushort)] = "ushort",
        };

        internal static string GetDisplayName(this Type type, bool full = false)
        {
            var builder = new StringBuilder();
            if (full)
            {
                builder.Append(type.Namespace);
                builder.Append('.');
            }
            ProcessType(builder, type);
            return builder.ToString();
        }

        private static void ProcessType(StringBuilder builder, Type type)
        {
            if (type.IsGenericType)
            {
                Type[] genericArguments = type.GetGenericArguments();
                ProcessGenericType(builder, type, genericArguments, genericArguments.Length);
            }
            else if (type.IsArray)
            {
                ProcessArrayType(builder, type);
            }
            else if (BuiltInTypeNames.TryGetValue(type, out var builtInName))
            {
                builder.Append(builtInName);
            }
            else if (type.IsGenericParameter)
            {
                builder.Append(type.Name);
            }
            else
            {
                builder.Append(type.Name);
            }
        }

        private static void ProcessArrayType(StringBuilder builder, Type type)
        {
            Type innerType = type;

            while (innerType.IsArray)
            {
                innerType = innerType.GetElementType();
            }

            ProcessType(builder, innerType);

            while (type.IsArray)
            {
                builder.Append('[');
                builder.Append(',', type.GetArrayRank() - 1);
                builder.Append(']');
                type = type.GetElementType();
            }
        }

        private static void ProcessGenericType(StringBuilder builder, Type type, Type[] genericArguments, int length)
        {
            var offset = 0;

            if (type.IsNested)
            {
                offset = type.DeclaringType.GetGenericArguments().Length;
            }

            var genericPartIndex = type.Name.IndexOf('`');

            if (genericPartIndex <= 0)
            {
                builder.Append(type.Name);
                return;
            }

            builder.Append(type.Name, 0, genericPartIndex);
            builder.Append('<');

            for (var i = offset; i < length; i++)
            {
                ProcessType(builder, genericArguments[i]);

                if (i + 1 < length)
                {
                    builder.Append(',');
                }
            }

            builder.Append('>');
        }
    }
}
