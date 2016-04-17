using System.Collections.Generic;

namespace Crisp.Shared
{
    /// <summary>
    /// Represents a source of token lists.
    /// </summary>
    public interface ITokenListSource
    {
        /// <summary>
        /// Gets the token list.
        /// </summary>
        /// <returns></returns>
        IList<IToken> Get();
    }
}
