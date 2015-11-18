using System.Collections.Generic;
using System.IO;

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
        /// Returns a new evaluator with bindings added between symbols and expressions.
        /// </summary>
        /// <param name="bindings">The bindings to add.</param>
        /// <returns></returns>
        IEvaluator Bind(Dictionary<SymbolAtom, SymbolicExpression> bindings);

        /// <summary>
        /// Returns a new evaluator with a binding added between a symbol and expression.
        /// </summary>
        /// <param name="symbol">The symbol to bind to the expression.</param>
        /// <param name="expression">The expression to bind to the symbol.</param>
        /// <returns></returns>
        IEvaluator Bind(SymbolAtom symbol, SymbolicExpression expression);

        /// <summary>
        /// Binds symbols to expressions in this evaluator.
        /// </summary>
        /// <param name="bindings">The bindings to add.</param>
        void MutableBind(Dictionary<SymbolAtom, SymbolicExpression> bindings);

        /// <summary>
        /// Binds a symbol to an expression in this evaluator.
        /// </summary>
        /// <param name="symbol">The symbol to bind to the expression.</param>
        /// <param name="expression">The expression to bind to the symbol.</param>
        void MutableBind(SymbolAtom symbol, SymbolicExpression expression);
    }
}
