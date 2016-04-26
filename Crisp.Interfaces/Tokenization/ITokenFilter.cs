using System.Collections.Generic;

namespace Crisp.Interfaces.Tokenization
{
    public interface ITokenFilter
    {
        /// <summary>
        /// Filters a token list.
        /// </summary>
        /// <param name="tokens">The list of tokens to filter.</param>
        /// <returns></returns>
        IList<IToken> Filter(IList<IToken> tokens);
    }
}
