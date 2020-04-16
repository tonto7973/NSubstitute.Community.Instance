namespace NSubstitute
{
    internal partial class SR
    {
        public static string Format(string value, params object[] args)
            => string.Format(Culture, value, args);
    }
}
