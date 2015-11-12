namespace Crisp.Core.Evaluation
{
    /// <summary>
    /// Implemented by classes that wish to evaluate expressions, usually interpreters.
    /// </summary>
    public interface IEvaluator
    {
        /// <summary>
        /// Evaluates an expression.
        /// </summary>
        /// <param name="expression">The expression to evaluate.</param>
        /// <returns></returns>
        SymbolicExpression Evaluate(SymbolicExpression expression);

        /// <summary>
        /// Returns a new evaluator with a binding added between a symbol and expression.
        /// </summary>
        /// <param name="binding">The binding to add to the evaluator.</param>
        /// <returns></returns>
        Evaluator Bind(Binding binding);

        /// <summary>
        /// Returns a new evaluator with a binding added between a symbol and expression.
        /// </summary>
        /// <param name="symbol">The symbol to bind to the expression.</param>
        /// <param name="expression">The expression to bind to the symbol.</param>
        /// <returns></returns>
        Evaluator Bind(SymbolAtom symbol, SymbolicExpression expression);
    }
}
