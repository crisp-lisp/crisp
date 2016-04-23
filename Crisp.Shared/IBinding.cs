namespace Crisp.Shared
{
    /// <summary>
    /// Represents a binding between a name and an expression.
    /// </summary>
    public interface IBinding
    {
        /// <summary>
        /// The evaluator to use to lazily evaluate the bound expression.
        /// </summary>
        IEvaluator Evaluator { get; }

        /// <summary>
        /// Gets the name that is bound to the expression.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the expression that is bound to the symbol.
        /// </summary>
        ISymbolicExpression Value { get; }
    }

}
