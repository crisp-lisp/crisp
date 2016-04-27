using System.Collections.Generic;
using Crisp.Enums;
using Crisp.Interfaces;
using Crisp.Shared;

namespace Crisp.Types
{
    /// <summary>
    /// Contains convenient extension methods for working with symbolic expressions.
    /// </summary>
    public static class SymbolicExpressionExtensions
    {
        /// <summary>
        /// Gets the expression as a pair or throws an exception in case of type mismatch.
        /// </summary>
        /// <param name="expression">The expression to attempt to cast.</param>
        /// <returns></returns>
        public static Pair AsPair(this ISymbolicExpression expression)
        {
            return (Pair)expression;
        }

        /// <summary>
        /// Gets the expression as a string atom or throws an exception in case of type mismatch.
        /// </summary>
        /// <param name="expression">The expression to attempt to cast.</param>
        /// <returns></returns>
        public static StringAtom AsString(this ISymbolicExpression expression)
        {
            return (StringAtom)expression;
        }

        /// <summary>
        /// Gets the expression as a numeric atom or throws an exception in case of type mismatch.
        /// </summary>
        /// <param name="expression">The expression to attempt to cast.</param>
        /// <returns></returns>
        public static NumericAtom AsNumeric(this ISymbolicExpression expression)
        {
            return (NumericAtom)expression;
        }

        /// <summary>
        /// Gets the expression as a symbolic atom or throws an exception in case of type mismatch.
        /// </summary>
        /// <param name="expression">The expression to attempt to cast.</param>
        /// <returns></returns>
        public static SymbolAtom AsSymbol(this ISymbolicExpression expression)
        {
            return (SymbolAtom)expression;
        }

        /// <summary>
        /// Gets the expression as a constant atom or throws an exception in case of type mismatch.
        /// </summary>
        /// <param name="expression">The expression to attempt to cast.</param>
        /// <returns></returns>
        public static ConstantAtom AsConstant(this ISymbolicExpression expression)
        {
            return (ConstantAtom)expression;
        }

        /// <summary>
        /// Gets the expression as a boolean atom or throws an exception in case of type mismatch.
        /// </summary>
        /// <param name="expression">The expression to attempt to cast.</param>
        /// <returns></returns>
        public static BooleanAtom AsBoolean(this ISymbolicExpression expression)
        {
            return (BooleanAtom)expression;
        }

        /// <summary>
        /// Gets the expression as a function or throws an exception in the case of type mismatch.
        /// </summary>
        /// <param name="expression">The expression to attempt to case.</param>
        /// <returns></returns>
        public static Function AsFunction(this ISymbolicExpression expression)
        {
            return (Function)expression;
        }

        /// <summary>
        /// Returns the head of a pair as a pair.
        /// </summary>
        /// <param name="expression">The pair to navigate.</param>
        /// <returns></returns>
        public static Pair GoHead(this Pair expression)
        {
            return expression.AsPair().Head.AsPair();
        }

        /// <summary>
        /// Returns the tail of a pair as a pair.
        /// </summary>
        /// <param name="expression">The pair to navigate.</param>
        /// <returns></returns>
        public static Pair GoTail(this Pair expression)
        {
            return expression.AsPair().Tail.AsPair();
        }

        /// <summary>
        /// Expands nested pairs into a list.
        /// </summary>
        /// <param name="expression">The expression containing nested pairs to expand.</param>
        /// <returns></returns>
        public static IList<ISymbolicExpression> Expand(this Pair expression)
        {
            var list = new List<ISymbolicExpression> {expression.Head};
            
            if (expression.Tail.Type != SymbolicExpressionType.Pair)
            {
                return list;
            }

            // Pull values out of nested pairs.
            var current = expression.GoTail();
            while (current != null)
            {
                list.Add(current.Head);
                current = current.Tail.Type == SymbolicExpressionType.Pair ? current.GoTail() : null;
            }

            return list;
        }

        /// <summary>
        /// Converts a list of symbolic expressions to a proper list formed from pairs.
        /// </summary>
        /// <param name="members">The members of the list.</param>
        /// <param name="index">The index to begin processing the list at.</param>
        /// <returns></returns>
        private static ISymbolicExpression ToProperList(IList<ISymbolicExpression> members, int index)
        {
            if (members.Count == 0)
            {
                return new Nil();
            }
            return new Pair(members[index], index == members.Count - 1 ? new Nil() : ToProperList(members, index + 1));
        }

        /// <summary>
        /// Converts a list of symbolic expressions to a proper list formed from pairs.
        /// </summary>
        /// <param name="members">The members of the list.</param>
        /// <returns></returns>
        public static ISymbolicExpression ToProperList(this IList<ISymbolicExpression> members)
        {
            return ToProperList(members, 0);
        }

        /// <summary>
        /// Throws a <see cref="FunctionApplicationException"/> if the given expression is not a list.
        /// </summary>
        /// <param name="expression">The expression to test.</param>
        /// <param name="functionName">The name of the calling function.</param>
        public static void ThrowIfNotList(this ISymbolicExpression expression, string functionName)
        {
            if (expression.Type != SymbolicExpressionType.Pair)
            {
                throw new FunctionApplicationException($"The function {functionName} takes a parameter list but" +
                                                       " one was not given.");
            }
        }

        /// <summary>
        /// Throws a <see cref="FunctionApplicationException"/> if the given list is not of the specified length.
        /// </summary>
        /// <param name="expression">The expression to test.</param>
        /// <param name="functionName">The name of the calling function.</param>
        /// <param name="numberOfArguments"></param>
        public static void ThrowIfWrongLength(this IList<ISymbolicExpression> expression, string functionName,
            int numberOfArguments)
        {
            if (expression.Count != numberOfArguments)
            {
                throw new FunctionApplicationException($"The function {functionName} takes exactly" +
                                                       $" {numberOfArguments} argument(s).");
            }
        }

        /// <summary>
        /// Throws a <see cref="FunctionApplicationException"/> if the given list is not of the specified length.
        /// </summary>
        /// <param name="expression">The expression to test.</param>
        /// <param name="functionName">The name of the calling function.</param>
        /// <param name="numberOfArguments"></param>
        public static void ThrowIfShorterThanLength(this IList<ISymbolicExpression> expression, string functionName,
            int numberOfArguments)
        {
            if (expression.Count < numberOfArguments)
            {
                throw new FunctionApplicationException($"The function {functionName} must have at least" +
                                                       $" {numberOfArguments} argument(s).");
            }
        }
    }
}
