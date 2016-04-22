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
        public IList<IBinding> Bindings { get; private set; }

        public string InterpreterDirectory { get; set; }

        public string SourceFileDirectory { get; set; }

        public string WorkingDirectory { get; set; }

        /// <summary>
        /// Initializes a new instance of an expression evaluator.
        /// </summary>
        public Evaluator()
        {
            Bindings = new List<IBinding>();
        }

        public IEvaluator Derive()
        {
            return Derive(new Dictionary<string, ISymbolicExpression>());
        }

        public IEvaluator Derive(Dictionary<string, ISymbolicExpression> bindings)
        {
            // We need an all-new list.
            var newBindings = new List<IBinding>(Bindings);
            newBindings.AddRange(bindings.Select(b => new Binding(b.Key, b.Value, this)));

            // Return an all-new evaluator.
            return new Evaluator
            {
                Bindings = newBindings,
                InterpreterDirectory = InterpreterDirectory,
                SourceFileDirectory = SourceFileDirectory,
                WorkingDirectory = WorkingDirectory
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
            foreach (var entry in bindings)
            {
                Bindings.Add(new Binding(entry.Key, entry.Value, this));
            }
        }

        public void Mutate(string symbol, ISymbolicExpression expression)
        {
            Bindings.Add(new Binding(symbol, expression, this));
        }

        /// <summary>
        /// Returns the binding for a given symbol.
        /// </summary>
        /// <param name="symbol">The symbol to return the binding for.</param>
        /// <returns></returns>
        private IBinding Lookup(SymbolAtom symbol)
        {
            if (!IsBound(symbol))
            {
                throw new EvaluationException($"Use of name {symbol.Value} which is unbound or outside its scope.");
            }
            return Bindings.Last(b => b.Name == symbol.Value);
        }

        /// <summary>
        /// Returns whether or not a binding currently exists for a given symbol.
        /// </summary>
        /// <param name="symbol">The symbol to check for.</param>
        /// <returns></returns>
        private bool IsBound(SymbolAtom symbol)
        {
            return Bindings.Any(b => b.Name == symbol.Value);
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
