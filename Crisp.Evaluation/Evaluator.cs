using System.Collections.Generic;
using System.Linq;

using Crisp.Shared;
using Crisp.Types;

namespace Crisp.Evaluation
{
    /// <summary>
    /// An implementation of an expression evaluator.
    /// </summary>
    public class Evaluator : IEvaluator
    {
        /// <summary>
        /// A list of bindings between symbols and expressions.
        /// </summary>
        private List<Binding> _bindings;

        public string InterpreterDirectory { get; set; }

        public string SourceFileDirectory { get; set; }

        /// <summary>
        /// Initializes a new instance of an expression evaluator.
        /// </summary>
        public Evaluator()
        {
            _bindings = new List<Binding>();
        }

        public IEvaluator Derive()
        {
            return Derive(new Dictionary<string, ISymbolicExpression>());
        }

        public IEvaluator Derive(Dictionary<string, ISymbolicExpression> bindings)
        {
            // We need an all-new list.
            var newBindings = new List<Binding>(_bindings);
            newBindings.AddRange(bindings.Select(b => new Binding(b.Key, b.Value, this)));

            // Return an all-new evaluator.
            return new Evaluator
            {
                _bindings = newBindings,
                InterpreterDirectory = InterpreterDirectory,
                SourceFileDirectory = SourceFileDirectory
            }; 
        }

        public IEvaluator Derive(string name, ISymbolicExpression expression)
        {
            return Derive(new Dictionary<string, ISymbolicExpression>
            {
                {name, expression}
            });
        }

        public void Mutate(Dictionary<string, ISymbolicExpression> bindings)
        {
            _bindings.AddRange(bindings.Select(b => new Binding(b.Key, b.Value, this)));
        }

        public void Mutate(string symbol, ISymbolicExpression expression)
        {
            _bindings.Add(new Binding(symbol, expression, this));
        }

        /// <summary>
        /// Returns the binding for a given symbol.
        /// </summary>
        /// <param name="symbol">The symbol to return the binding for.</param>
        /// <returns></returns>
        private Binding Lookup(SymbolAtom symbol)
        {
            if (!IsBound(symbol))
            {
                throw new EvaluationException($"Use of name {symbol.Value} which is unbound or outside its scope.");
            }
            return _bindings.Last(b => b.Name == symbol.Value);
        }

        /// <summary>
        /// Returns whether or not a binding currently exists for a given symbol.
        /// </summary>
        /// <param name="symbol">The symbol to check for.</param>
        /// <returns></returns>
        private bool IsBound(SymbolAtom symbol)
        {
            return _bindings.Any(b => b.Name == symbol.Value);
        }

        public ISymbolicExpression Evaluate(ISymbolicExpression expression)
        {
            switch (expression.Type)
            {
                case SymbolicExpressionType.Symbol:
                    return Lookup(expression.AsSymbol()).Value;
                case SymbolicExpressionType.Pair:
                    var pair = expression.AsPair();

                    // Might pair be a function application?
                    if (pair.IsExplicitlyBracketed) 
                    {
                        // Evaluate head to get function.
                        var head = Evaluate(pair.Head); 
                        if (head.Type == SymbolicExpressionType.Function)
                        {
                            var function = head.AsFunction();

                            // Don't evaluate arguments to special forms.
                            var args = function.SkipArgumentEvaluation ? pair.Tail : Evaluate(pair.Tail); 
                            return function.Apply(args, this);
                        }  
                    }
                    return new Pair(Evaluate(pair.Head), Evaluate(pair.Tail)); // Evaluate pair.
                default:
                    return expression; // Non-symbol atoms evaluate to themselves.
            }
        }
    }
}
