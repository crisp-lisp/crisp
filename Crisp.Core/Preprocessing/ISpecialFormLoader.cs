using System.Collections.Generic;

using Crisp.Core.Types;

namespace Crisp.Core.Preprocessing
{
    /// <summary>
    /// Represents a special form loader, capable of loading compiled special forms from files inside directories and 
    /// returning their bindings.
    /// </summary>
    public interface ISpecialFormLoader
    {
        /// <summary>
        /// Gets all compiled special forms residing in files within the directory at the specified path.
        /// </summary>
        /// <param name="directory">The directory to scan for special forms.</param>
        /// <returns></returns>
        Dictionary<SymbolAtom, SymbolicExpression> GetBindings(string directory);
    }
}
