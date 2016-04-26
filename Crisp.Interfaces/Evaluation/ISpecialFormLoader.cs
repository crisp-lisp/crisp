using System.Collections.Generic;

namespace Crisp.Interfaces.Evaluation
{
    /// <summary>
    /// Represents a special form loading service.
    /// </summary>
    public interface ISpecialFormLoader
    {
        /// <summary>
        /// Gets a dictionary of loaded special form names against their bound expressions.
        /// </summary>
        /// <returns></returns>
        Dictionary<string, ISymbolicExpression> GetBindings();
    }
}
