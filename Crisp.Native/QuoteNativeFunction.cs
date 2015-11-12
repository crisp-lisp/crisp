using Crisp.Core;
using Crisp.Core.Evaluation;

namespace Crisp.Native
{
    /// <summary>
    /// Represents the basic constant function.
    /// </summary>
    public class QuoteNativeFunction : IFunction
    {
        public IEvaluator Host { get; set; }

        public string Name => "quote";

        /// <summary>
        /// Recursively converts all symbols in an expression to constants.
        /// </summary>
        /// <param name="expression">The expression to convert.</param>
        /// <returns></returns>
        private static SymbolicExpression Quote(SymbolicExpression expression)
        {
            switch (expression.Type)
            {
                case SymbolicExpressionType.Symbol:
                    return new ConstantAtom(expression.AsSymbol()); // Symbols to constants to avoid evaluation.
                case SymbolicExpressionType.Pair:
                    var pair = expression.AsPair();
                    return new Pair(Quote(pair.Head), Quote(pair.Tail));
            }

            return expression;
        }

        public SymbolicExpression Apply(SymbolicExpression expression, Context context)
        {
            expression.ThrowIfNotList(Name); // Takes a list of arguments.

            var arguments = expression.AsPair().Expand();
            arguments.ThrowIfWrongLength(Name, 1); // Must have one argument.

            return Quote(arguments[0]);
        }
    }
}
