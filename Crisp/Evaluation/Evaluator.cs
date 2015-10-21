using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Crisp.Core;

namespace Crisp.Evaluation
{
    /// <summary>
    /// An implementation of an expression evaluator.
    /// </summary>
    internal class Evaluator : IFunctionHost
    {
        private Context baseContext;

        /// <summary>
        /// Gets whether or not a type qualifies as a native function type for loading.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns></returns>
        private bool IsNativeFunctionType(Type type)
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
                    baseContext = baseContext.Bind(new SymbolAtom(function.Name), new NativeFunction(function));
                }
            }
        }
        
        public SymbolicExpression Evaluate(SymbolicExpression expression, Context context)
        {
            if (expression == null || expression.IsAtomic)
                return expression;

            // Is this a function we should apply?
            var node = expression.AsNode();
            if (node.Head.Type == SymbolicExpressionType.Symbol)
            {
                var symbol = node.Head.AsSymbol();
                var function = context.LookupValue(symbol).AsFunction();
                return function.Apply(node.Tail, context);
            }
            else
            {
                // Evaluate sub-expressions.
                return new Node(Evaluate(node.Head, context), 
                    Evaluate(node.Tail, context)); 
            }
        }

        /// <summary>
        /// Evaluates an expression in the base context.
        /// </summary>
        /// <param name="context">The context in which to evaluate the expression.</param>
        /// <returns></returns>
        public SymbolicExpression Evaluate(SymbolicExpression expression)
        {
            return Evaluate(expression, baseContext);
        }

        /// <summary>
        /// Initializes a new instance of an expression evaluator.
        /// </summary>
        /// <param name="directory">The directory path from which to load native function libraries.</param>
        public Evaluator(string directory)
        {
            if (!Directory.Exists(directory))
                throw new DirectoryNotFoundException("Could not load native function libraries because the directory was not found.");

            // Load native functions from directory.
            baseContext = new Context();
            LoadNativeFunctions(directory); 
        }
    }
}
