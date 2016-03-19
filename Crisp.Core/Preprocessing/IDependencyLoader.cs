using System.Collections.Generic;

using Crisp.Core.Types;

namespace Crisp.Core.Preprocessing
{
    /// <summary>
    /// Represents a dependency loader, capable of returning the bindings required by a source file.
    /// </summary>
    public interface IDependencyLoader
    {
        /// <summary>
        /// Gets all required bindings for the source file at the specified path.
        /// </summary>
        /// <returns></returns>
        Dictionary<SymbolAtom, SymbolicExpression> GetBindings();
    }
}
