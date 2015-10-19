namespace Crisp.Core
{
    /// <summary>
    /// Implemented by classes that wish to host native functions, usually interpreters.
    /// </summary>
    public interface INativeFunctionHost
    {
        /// <summary>
        /// Evaluates an expression.
        /// </summary>
        /// <param name="expression">The expression to evaluate.</param>
        /// <returns></returns>
        SymbolicExpression Evaluate(SymbolicExpression expression);
    }
}
