using System.Collections.Generic;
using System.Linq;

namespace Crisp.Core
{
    /// <summary>
    /// Represents an immutable context in which expressions can be evaluated.
    /// </summary>
    public class Context
    {
        /// <summary>
        /// A list of bindings between symbols and expressions.
        /// </summary>
        private readonly List<Binding> _bindings;

        /// <summary>
        /// Adds a binding to the context.
        /// </summary>
        /// <param name="binding">The binding to add to the context.</param>
        /// <returns></returns>
        private Context Bind(Binding binding)
        {
            // We need an all-new list.
            var newBindings = new List<Binding>(_bindings) {binding};

            return new Context(newBindings); // Return an all-new context.
        }

        /// <summary>
        /// Adds a new binding between a symbol and an expression.
        /// </summary>
        /// <param name="symbol">The symbol to bind to the expression.</param>
        /// <param name="expression">The expression to bind to the symbol.</param>
        /// <returns></returns>
        public Context Bind(SymbolAtom symbol, SymbolicExpression expression)
        {
            return Bind(new Binding(symbol, expression));
        }

        /// <summary>
        /// Returns the binding for a given symbol.
        /// </summary>
        /// <param name="symbol">The symbol to return the binding for.</param>
        /// <returns></returns>
        private Binding Lookup(SymbolAtom symbol)
        {
            return _bindings.Last(b => b.Symbol.Matches(symbol));
        }

        /// <summary>
        /// Returns the bound expression for a given symbol.
        /// </summary>
        /// <param name="symbol">The symbol to return the bound expression for.</param>
        /// <returns></returns>
        public SymbolicExpression LookupValue(SymbolAtom symbol)
        {
            return Lookup(symbol).Expression;
        }

        /// <summary>
        /// Returns whether or not a binding currently exists for a given symbol.
        /// </summary>
        /// <param name="symbol">The symbol to check for.</param>
        /// <returns></returns>
        public bool IsBound(SymbolAtom symbol)
        {
            return _bindings.Any(b => b.Symbol.Matches(symbol));
        }

        /// <summary>
        /// Initializes a new instance of an immutable context in which expressions can be evaluated.
        /// </summary>
        public Context()
        {
            _bindings = new List<Binding>();
        }

        /// <summary>
        /// Initializes a new instance of an immutable context in which expressions can be evaluated.
        /// </summary>
        /// <param name="bindings">A list of bindings from which to initialize the context.</param>
        public Context(List<Binding> bindings) 
        {
            _bindings = bindings;
        }
    }
}
