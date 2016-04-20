using System.Collections.Generic;

namespace Crisp.Shared
{
    /// <summary>
    /// Represents a expression evaluator.
    /// </summary>
    public interface IEvaluator
    {
        /// <summary>
        /// Gets the directory containing the interpreter on-disk.
        /// </summary>
        string InterpreterDirectory { get; set; }

        /// <summary>
        /// Gets the directory containing the source file on-disk.
        /// </summary>
        string SourceFileDirectory { get; set; }

        /// <summary>
        /// Evaluates an expression.
        /// </summary>
        /// <param name="expression">The expression to evaluate.</param>
        /// <returns></returns>
        ISymbolicExpression Evaluate(ISymbolicExpression expression);

        /// <summary>
        /// Returns a new, identical evaluator.
        /// </summary>
        /// <returns></returns>
        IEvaluator Derive();

        /// <summary>
        /// Returns a new, identical evaluator with bindings added between names and expressions.
        /// </summary>
        /// <param name="bindings">The bindings to add.</param>
        /// <returns></returns>
        IEvaluator Derive(Dictionary<string, ISymbolicExpression> bindings);

        /// <summary>
        /// Returns a new, identical evaluator with a binding added between a name and expression.
        /// </summary>
        /// <param name="name">The name to bind to the expression.</param>
        /// <param name="expression">The expression to bind to the name.</param>
        /// <returns></returns>
        IEvaluator Derive(string name, ISymbolicExpression expression);

        /// <summary>
        /// Binds names to expressions in this evaluator.
        /// </summary>
        /// <param name="bindings">The bindings to add.</param>
        void Mutate(Dictionary<string, ISymbolicExpression> bindings);

        /// <summary>
        /// Binds a name to an expression in this evaluator.
        /// </summary>
        /// <param name="name">The name to bind to the expression.</param>
        /// <param name="expression">The expression to bind to the name.</param>
        void Mutate(string name, ISymbolicExpression expression);
    }
}
