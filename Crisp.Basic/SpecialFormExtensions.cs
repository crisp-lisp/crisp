using System.Collections.Generic;

using Crisp.Core;

namespace Crisp.Basic
{
    internal static class SpecialFormExtensions
    {
        public static void ThrowIfNotList(this SymbolicExpression expression, string functionName)
        {
            if (expression.Type != SymbolicExpressionType.Pair)
            {
                throw new RuntimeException($"The function {functionName} takes a parameter list but one was not given.");
            }
        }

        public static void ThrowIfWrongLength(this IList<SymbolicExpression> arguments, string functionName,
            int numberOfArguments)
        {
            if (arguments.Count != numberOfArguments)
            {
                throw new RuntimeException($"The function {functionName} takes exactly {numberOfArguments} argument(s).");
            }
        }

        public static void ThrowIfShorterThanLength(this IList<SymbolicExpression> arguments, string functionName,
            int numberOfArguments)
        {
            if (arguments.Count < numberOfArguments)
            {
                throw new RuntimeException($"The function {functionName} must have at least {numberOfArguments} argument(s).");
            }
        }
    }
}
