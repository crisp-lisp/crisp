using System.Collections.Generic;

using Crisp.Core.Types;

namespace Crisp.Core.Preprocessing
{
    /// <summary>
    /// Represents a special form loader, capable of loading compiled special forms from libraries and returning 
    /// bindings associating them with their names.
    /// </summary>
    public interface ISpecialFormLoader
    {
        /// <summary>
        /// Gets bindings associating all compiled special forms residing in files within the configured directory with
        /// their names.
        /// </summary>
        /// <returns></returns>
        Dictionary<SymbolAtom, SymbolicExpression> GetBindings();
    }
}
