using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using NSubstitute.Core;

namespace NSubstitute.Instantiation
{
    internal static class ActivationExtensions
    {
        internal static object Invoke(this Activation activation)
        {
            var constructorArguments = activation
                .Arguments
                .Select(arg => arg is INullValue ? null : arg)
                .ToArray();

            try
            {
                return activation.ConstructorInfo.DeclaringType.IsAbstract
                    ? activation.InvokeAbstract(constructorArguments)
                    : activation.InvokeConcrete(constructorArguments);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        private static object InvokeAbstract(this Activation activation, object[] constructorArguments)
            => SubstitutionContext
                    .Current
                    .SubstituteFactory
                    .CreatePartial(
                        new[] { activation.ConstructorInfo.DeclaringType },
                        constructorArguments);

        private static object InvokeConcrete(this Activation activation, object[] constructorArguments)
            => activation
                    .ConstructorInfo
                    .Invoke(constructorArguments);
    }
}
