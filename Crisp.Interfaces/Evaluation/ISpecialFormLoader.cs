using System.Collections.Generic;

namespace Crisp.Interfaces.Evaluation
{
    /// <summary>
    /// Represents a special form loading service.
    /// </summary>
    public interface ISpecialFormLoader
    {
        /// <summary>
        /// Gets bindings associating all compiled special forms residing in files within the configured directory with
        /// their names.
        /// </summary>
        /// <returns></returns>
        Dictionary<string, ISymbolicExpression> GetBindings();
    }
}
