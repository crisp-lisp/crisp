using Crisp.Core.Evaluation;

namespace Crisp.Core
{   
    /// <summary>
    /// Implemented by classes that provide functions to the interpreter.
    /// </summary>
    public interface IFunction
    {
        /// <summary>
        /// Gets the name of the function .
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Contains a reference to the function host, usually the executing interpreter.
        /// </summary>
        IEvaluator Host { get; set; }

        /// <summary>
        /// Applies the function to an expression.
        /// </summary>
        /// <param name="expression">The expression to apply the function to.</param>
        /// <param name="context">The context in which to evaluate the expression.</param>
        /// <returns></returns>
        SymbolicExpression Apply(SymbolicExpression expression, Context context);
    }
}
