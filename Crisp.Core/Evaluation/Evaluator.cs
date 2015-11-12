using System;
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
        private Context _baseContext;

        /// <summary>
        /// Gets whether or not a type qualifies as a native function type for loading.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns></returns>
        private static bool IsNativeFunctionType(Type type)
        {
            // Type must be public, concrete and implement INativeFunction.
            return type.IsPublic
                && !type.IsAbstract
                && type.GetInterfaces().Any(i => i == typeof(IFunction));
        }

        /// <summary>
        /// Loads all native function libraries from a directory and binds them into the base context.
        /// </summary>
        /// <param name="directory">The directory path to load libraries from.</param>
        /// <returns></returns>
        private void LoadNativeFunctions(string directory)
        {
            foreach (var file in Directory.GetFiles(directory, "*.dll"))
            {
                var assembly = Assembly.LoadFrom(file);
                var types = assembly.GetTypes().Where(IsNativeFunctionType);

                foreach (var type in types)
                {
                    var function = (IFunction)Activator.CreateInstance(assembly.GetType(type.ToString()));
                    function.Host = this;
                    _baseContext = _baseContext.Bind(new SymbolAtom(function.Name), new NativeFunction(function));
                }
            }
        }

        /// <summary>
        /// Evaluates an expression.
        /// </summary>
        /// <param name="expression">The expression to evaluate.</param>
        /// <param name="context">The context in which to evaluate the expression.</param>
        /// <returns></returns>
        public SymbolicExpression Evaluate(SymbolicExpression expression, Context context)
        {
            switch (expression.Type)
            {
                case SymbolicExpressionType.Symbol:
                    var symbol = expression.AsSymbol();
                    if (!context.IsBound(symbol))
                        throw new RuntimeException($"Use of name {symbol.Name} which is unbound or outside its scope.");
                    return context.LookupValue(symbol);
                case SymbolicExpressionType.Numeric:
                    return expression.AsNumeric();
                case SymbolicExpressionType.String:
                    return expression.AsString();
                case SymbolicExpressionType.Constant:
                    return expression.AsConstant();
            }
            
            // Is this a function we should apply?
            var node = expression.AsPair();
            if (node.Head.Type == SymbolicExpressionType.Symbol)
            {
                var symbol = node.Head.AsSymbol();
                var function = context.LookupValue(symbol).AsFunction();
                return function.Apply(node.Tail, context);
            }

            // Evaluate sub-expressions.
            return new Pair(Evaluate(node.Head, context), 
                Evaluate(node.Tail, context)); 
        }

        /// <summary>
        /// Evaluates an expression in the base context.
        /// </summary>
        /// <param name="expression">The expression to evaluate.</param>
        /// <returns></returns>
        public SymbolicExpression Evaluate(SymbolicExpression expression)
        {
            return Evaluate(expression, _baseContext);
        }

        /// <summary>
        /// Initializes a new instance of an expression evaluator.
        /// </summary>
        /// <param name="directory">The directory path from which to load native function libraries.</param>
        public Evaluator(string directory)
        {
            if (!Directory.Exists(directory))
                throw new DirectoryNotFoundException("Could not load native function libraries because the directory was not found.");

            // Initialize context with special symbols.
            _baseContext = new Context()
                .Bind(SymbolAtom.Nil, new ConstantAtom(SymbolAtom.Nil))
                .Bind(SymbolAtom.True, new ConstantAtom(SymbolAtom.True))
                .Bind(SymbolAtom.False, new ConstantAtom(SymbolAtom.False));

            // Load native functions from directory.
            LoadNativeFunctions(directory); 
        }
    }
}
