using System.Collections.Generic;

using Crisp.Interfaces.Types;

namespace Crisp.Interfaces.Evaluation
{
    /// <summary>
    /// Represents a expression evaluator.
    /// </summary>
    public interface IEvaluator
    {
        /// <summary>
        /// Gets a list of bindings between names and expressions.
        /// </summary>
        IList<IBinding> Bindings { get; }

        /// <summary>
        /// Gets the path of the directory containing the interpreter executable.
        /// </summary>
        string InterpreterDirectory { get; set; }

        /// <summary>
        /// Gets the path of the directory containing the source file.
        /// </summary>
        string SourceFileDirectory { get; set; }
        
        /// <summary>
        /// Gets the path of the directory that relative paths should begin at.
        /// </summary>
        string WorkingDirectory { get; set; }

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
        /// Returns a new, identical evaluator containing a new set of bindings.
        /// </summary>
        /// <param name="bindings">The bindings to add.</param>
        /// <returns></returns>
        IEvaluator Derive(IList<IBinding> bindings);

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
        /// Adds a new set of bindings to this evaluator.
        /// </summary>
        /// <param name="bindings">The bindings to add.</param>
        void Mutate(IList<IBinding> bindings);

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
