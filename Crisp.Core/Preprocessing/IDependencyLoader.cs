using System.Collections.Generic;

using Crisp.Core.Types;

namespace Crisp.Core.Preprocessing
{
    /// <summary>
    /// Represents a dependency loader.
    /// </summary>
    public interface IDependencyLoader
    {
        /// <summary>
        /// Gets all required bindings for the source file at the specified path.
        /// </summary>
        /// <param name="filepath">The path of the source file.</param>
        /// <returns></returns>
        Dictionary<SymbolAtom, SymbolicExpression> GetBindings(string filepath);
    }
}
