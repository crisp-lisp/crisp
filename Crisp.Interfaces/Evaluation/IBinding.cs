namespace Crisp.Interfaces.Evaluation
{
    /// <summary>
    /// Represents a binding between a name and an expression.
    /// </summary>
    public interface IBinding
    {
        /// <summary>
        /// Gets the name that is bound to the expression.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the unevaluated expression that is bound to the symbol.
        /// </summary>
        ISymbolicExpression Expression { get; }

        /// <summary>
        /// The evaluator to use when the bound expression is lazily evaluated.
        /// </summary>
        IEvaluator Evaluator { get; }

        /// <summary>
        /// Gets the evaluated expression that is bound to the symbol.
        /// </summary>
        ISymbolicExpression Value { get; }
    }

}
