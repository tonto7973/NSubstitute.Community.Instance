using System;
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace NSubstitute.Instantiation
{
    internal static class ActivationExtensions
    {
        internal static TType Invoke<TType>(this Activation activation)
        {
            if (activation == null)
                throw new ArgumentNullException(nameof(activation));

            try
            {
                return (TType)activation.ConstructorInfo.Invoke(activation.Arguments);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }
    }
}
