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
    internal class Evaluator : INativeFunctionHost
    {
        private IList<INativeFunction> nativeFunctions;

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
                && type.GetInterfaces().Any(i => i == typeof(INativeFunction));
        }

        /// <summary>
        /// Loads all native function libraries from a directory.
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
                    var function = (INativeFunction)Activator.CreateInstance(assembly.GetType(type.ToString()));
                    function.Host = this;
                    nativeFunctions.Add(function);
                }
            }
        }

        /// <summary>
        /// Evaluates an expression.
        /// </summary>
        /// <param name="expression">The expression to evaluate.</param>
        /// <returns></returns>
        public SymbolicExpression Evaluate(SymbolicExpression expression)
        {
            if (expression == null)
                return null;

            if (expression.IsAtomic)
                return expression;

            // Is this a function we should apply?
            if (expression.LeftExpression.IsAtomic)
            {
                var name = expression.LeftExpression.AsSymbol();
                var function = nativeFunctions.First(f => f.Name == name);
                return function.Apply(expression.RightExpression);
            }
            else
            {
                // Evaluate sub-expressions.
                return new SymbolicExpression(
                    Evaluate(expression.LeftExpression), 
                    Evaluate(expression.RightExpression)); 
            }
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
            nativeFunctions = new List<INativeFunction>();
            LoadNativeFunctions(directory); 
        }
    }
}
