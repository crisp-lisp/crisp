namespace Crisp.Core
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
        /// <param name="context">The context in which to evaluate the expression.</param>
        /// <returns></returns>
        SymbolicExpression Evaluate(SymbolicExpression expression, Context context);
    }
}
