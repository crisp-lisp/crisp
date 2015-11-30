using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Crisp.Core.Types;

namespace Crisp.Core.Preprocessing
{
    public class SpecialFormLoader : ISpecialFormLoader
    {
        private readonly ISpecialFormDirectoryPathProvider _specialFormDirectoryPathProvider;

        /// <summary>
        /// Gets whether or not a type is a special form type for loading.
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

        public Dictionary<SymbolAtom, SymbolicExpression> GetBindings()
        {
            // Prepare list of definitions to pass back.
            var definitions = new Dictionary<SymbolAtom, SymbolicExpression>();

            // Loop through each file in target directory.
            foreach (var file in Directory.GetFiles(_specialFormDirectoryPathProvider.GetPath(), "*.dll"))
            {
                // Load assembly and find special form types.
                var assembly = Assembly.LoadFrom(file);
                var types = assembly.GetTypes().Where(IsSpecialFormType);

                // Loop through each special form and add definition.
                foreach (var type in types)
                {
                    var function = (SpecialForm)Activator.CreateInstance(assembly.GetType(type.ToString()));
                    definitions.Add(new SymbolAtom(function.Name), function);
                }
            }

            return definitions;
        }

        public SpecialFormLoader(ISpecialFormDirectoryPathProvider specialFormDirectoryPathProvider)
        {
            _specialFormDirectoryPathProvider = specialFormDirectoryPathProvider;
        }
    }
}
