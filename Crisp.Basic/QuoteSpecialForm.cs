using System.Collections.Generic;

using Crisp.Enums;
using Crisp.Interfaces.Evaluation;
using Crisp.Interfaces.Types;
using Crisp.Types;

namespace Crisp.Basic
{
    /// <summary>
    /// A special form that given an expression, returns that expression as a value.
    /// </summary>
    public class QuoteSpecialForm : SpecialForm
    {
        public override IEnumerable<string> Names => new List<string> {"quote"};

        /// <summary>
        /// Recursively converts all symbols in an expression to constants.
        /// </summary>
        /// <param name="expression">The expression to convert.</param>
        /// <returns></returns>
        private static ISymbolicExpression Quote(ISymbolicExpression expression)
        {
            switch (expression.Type)
            {
                case SymbolicExpressionType.Symbol:
                    return new ConstantAtom(expression.AsSymbol().Value); // Symbols to constants to avoid evaluation.
                case SymbolicExpressionType.Pair:
                    var pair = expression.AsPair();
                    return new Pair(Quote(pair.Head), Quote(pair.Tail));
            }

            return expression;
        }

        public override ISymbolicExpression Apply(ISymbolicExpression expression, IEvaluator evaluator)
        {
            expression.ThrowIfNotList(Name); // Takes a list of arguments.

            var arguments = expression.AsPair().Expand();
            arguments.ThrowIfWrongLength(Name, 1); // Must have one argument.

            return Quote(arguments[0]);
        }
    }
}
