using Crisp.Core;
using Crisp.Core.Evaluation;

namespace Crisp.Native
{
    /// <summary>
    /// Represents the basic constant function.
    /// </summary>
    public class QuoteSpecialForm : SpecialForm
    {
        public override string Name => "quote";

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
                    return new ConstantAtom(expression.AsSymbol().Name); // Symbols to constants to avoid evaluation.
                case SymbolicExpressionType.Pair:
                    var pair = expression.AsPair();
                    return new Pair(Quote(pair.Head), Quote(pair.Tail));
            }

            return expression;
        }

        public override SymbolicExpression Apply(SymbolicExpression expression, IEvaluator evaluator)
        {
            expression.ThrowIfNotList(Name); // Takes a list of arguments.

            var arguments = expression.AsPair().Expand();
            arguments.ThrowIfWrongLength(Name, 1); // Must have one argument.

            return Quote(arguments[0]);
        }
    }
}
