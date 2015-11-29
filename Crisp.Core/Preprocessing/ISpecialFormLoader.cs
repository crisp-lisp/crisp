using System.Collections.Generic;

using Crisp.Core.Types;

namespace Crisp.Core.Preprocessing
{
    /// <summary>
    /// Represents a special form loader.
    /// </summary>
    public interface ISpecialFormLoader
    {
        /// <summary>
        /// Gets all compiled special forms residing at the specified directory path.
        /// </summary>
        /// <param name="directory">The directory to load from.</param>
        /// <returns></returns>
        Dictionary<SymbolAtom, SymbolicExpression> GetBindings(string directory);
    }
}
