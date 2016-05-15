using System.Collections.Generic;

using Crisp.Interfaces.Shared;
using Crisp.Interfaces.Types;

namespace Crisp.Evaluation
{
    /// <summary>
    /// Represents a special form loading service that statically caches loaded special forms.
    /// </summary>
    public class CachingSpecialFormLoader : SpecialFormLoader
    {
        private static Dictionary<string, ISymbolicExpression> _bindings;

        /// <summary>
        /// Initializes a special form loading service that statically caches loaded special forms.
        /// </summary>
        /// <param name="specialFormDirectoryPathProvider">The special form directory path provider service.</param>
        public CachingSpecialFormLoader(ISpecialFormDirectoryPathProvider specialFormDirectoryPathProvider) 
            : base(specialFormDirectoryPathProvider)
        {
        }

        public override Dictionary<string, ISymbolicExpression> GetBindings()
        {
            return _bindings ?? (_bindings = base.GetBindings());
        }
    }
}
