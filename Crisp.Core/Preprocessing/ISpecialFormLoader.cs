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
        /// Gets all compiled special forms residing in files within the configured directory.
        /// </summary>
        /// <returns></returns>
        Dictionary<SymbolAtom, SymbolicExpression> GetBindings();
    }
}
