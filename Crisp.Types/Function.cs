using Crisp.Enums;
using Crisp.Interfaces;
using Crisp.Interfaces.Evaluation;
using Crisp.Interfaces.Types;
using Crisp.Shared;

namespace Crisp.Types
{
    /// <summary>
    /// Represents a function that can be applied to an expression.
    /// </summary>
    public abstract class Function : SymbolicExpression
    {
        public override bool IsAtomic => false;

        public override SymbolicExpressionType Type => SymbolicExpressionType.Function;
        
        /// <summary>
        /// Gets whether or not argument evaluation should be skipped for this function.
        /// </summary>
        public abstract bool SkipArgumentEvaluation { get; }

        /// <summary>
        /// Applies the function to an expression.
        /// </summary>
        /// <param name="expression">The expression to apply the function to.</param>
        /// <param name="evaluator">The evaluator to use to evaluate the expression.</param>
        /// <returns></returns>
        public abstract ISymbolicExpression Apply(ISymbolicExpression expression, IEvaluator evaluator);
    }
}
