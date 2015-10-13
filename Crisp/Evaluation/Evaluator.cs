using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Crisp.Core;

namespace Crisp.Evaluation
{
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

        private INativeFunction Lookup(string name)
        {
            return nativeFunctions.First(f => f.Name == name);
        }

        public SymbolicExpression Evaluate(SymbolicExpression expression)
        {
            var val = Lookup(expression.LeftExpression.Value.ToString()).Apply(expression.RightExpression);
            return val;
        }

        public Evaluator(string directory)
        {
            nativeFunctions = new List<INativeFunction>();
            LoadNativeFunctions(directory);
        }
    }
}
