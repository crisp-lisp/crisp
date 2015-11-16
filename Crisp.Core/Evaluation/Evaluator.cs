using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Crisp.Core.Evaluation
{
    /// <summary>
    /// An implementation of an expression evaluator.
    /// </summary>
    public class Evaluator : IEvaluator
    {
        #region Binding
        
        /// <summary>
        /// A list of bindings between symbols and expressions.
        /// </summary>
        private readonly List<Binding> _bindings;

        public IEvaluator Bind(Dictionary<SymbolAtom, SymbolicExpression> bindings)
        {
            // We need an all-new list.
            var newBindings = new List<Binding>(_bindings);
            newBindings.AddRange(bindings.Select(b => new Binding(b.Key, b.Value, this)));

            return new Evaluator(newBindings); // Return an all-new evaluator.
        }

        public IEvaluator Bind(SymbolAtom symbol, SymbolicExpression expression)
        {
            return Bind(new Dictionary<SymbolAtom, SymbolicExpression>()
            {
                {symbol, expression}
            });
        }

        public void MutableBind(Dictionary<SymbolAtom, SymbolicExpression> bindings)
        {
            _bindings.AddRange(bindings.Select(b => new Binding(b.Key, b.Value, this)));
        }

        public void MutableBind(SymbolAtom symbol, SymbolicExpression expression)
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
                throw new RuntimeException($"Use of name {symbol.Name} which is unbound or outside its scope.");
            }
            return _bindings.Last(b => b.Symbol.Matches(symbol));
        }

        /// <summary>
        /// Returns whether or not a binding currently exists for a given symbol.
        /// </summary>
        /// <param name="symbol">The symbol to check for.</param>
        /// <returns></returns>
        private bool IsBound(SymbolAtom symbol)
        {
            return _bindings.Any(b => b.Symbol.Matches(symbol));
        }

        #endregion

        /// <summary>
        /// Gets whether or not a type qualifies as a special form type for loading.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns></returns>
        private static bool IsSpecialFormType(Type type)
        {
            // Type must be public, concrete and subclass SpecialForm.
            return type.IsPublic
                && !type.IsAbstract
                && type.BaseType == typeof(SpecialForm);
        }

        /// <summary>
        /// Loads all special form libraries from a directory and binds them against their names.
        /// </summary>
        /// <param name="directory">The directory path to load libraries from.</param>
        /// <returns></returns>
        private void LoadSpecialForms(string directory)
        {
            foreach (var file in Directory.GetFiles(directory, "*.dll"))
            {
                var assembly = Assembly.LoadFrom(file);
                var types = assembly.GetTypes().Where(IsSpecialFormType);

                foreach (var type in types)
                {
                    var function = (SpecialForm)Activator.CreateInstance(assembly.GetType(type.ToString()));
                    MutableBind(new SymbolAtom(function.Name), function);
                }
            }
        }
        
        public SymbolicExpression Evaluate(SymbolicExpression expression)
        {
            switch (expression.Type)
            {
                case SymbolicExpressionType.Symbol:
                    return Lookup(expression.AsSymbol()).Value;
                case SymbolicExpressionType.Pair:
                    var pair = expression.AsPair();

                    // If pair is function application expression.
                    if (pair.IsFunctionApplication) 
                    {
                        // Evaluate head to get function.
                        var head = Evaluate(pair.Head); 
                        if (head.Type != SymbolicExpressionType.Function)
                        {
                            throw new RuntimeException(
                                "The first value in a function application expression must evaluate to a function.");
                        }
                        var function = head.AsFunction();
                        var args = function.SkipArgumentEvaluation ? pair.Tail : Evaluate(pair.Tail); // Don't evaluate arguments to special forms.
                        return function.Apply(args, this);
                    }
                    return new Pair(Evaluate(pair.Head), Evaluate(pair.Tail)); // Evaluate pair.
                default:
                    return expression; // Non-symbol atoms evaluate to themselves.
            }
        }

        /// <summary>
        /// Initializes a new instance of an expression evaluator.
        /// </summary>
        /// <param name="directory">The directory path from which to load special form libraries.</param>
        public Evaluator(string directory)
        {
            if (!Directory.Exists(directory))
            {
                throw new DirectoryNotFoundException(
                    "Could not load special form libraries because the directory was not found.");
            }

            // Initialize evaluator with special symbols.
            _bindings = new List<Binding>();
            MutableBind(SymbolAtom.Nil, new ConstantAtom(SymbolAtom.Nil));
            MutableBind(SymbolAtom.True, new ConstantAtom(SymbolAtom.True));
            MutableBind(SymbolAtom.False, new ConstantAtom(SymbolAtom.False));

            // Load special forms from directory.
            LoadSpecialForms(directory);
        }

        /// <summary>
        /// Initializes a new instance of an expression evaluator.
        /// </summary>
        /// <param name="bindings">A list of bindings from which to initialize the evaluator.</param>
        public Evaluator(List<Binding> bindings)
        {
            _bindings = bindings;
        }
    }
}
