using System.Collections.Generic;
using System.Linq;

namespace Crisp.Core.Tokenizing
{
    /// <summary>
    /// An implementation of a token filter.
    /// </summary>
    public class TokenFilter
    {
        private readonly IEnumerable<TokenType> _filtered;

        /// <summary>
        /// Filters a token list.
        /// </summary>
        /// <param name="tokens">The list of tokens to filter.</param>
        /// <returns></returns>
        public IList<Token> Filter(IList<Token> tokens)
        {
            return tokens.Where(t => !_filtered.Contains(t.Type)).ToList();
        }

        /// <summary>
        /// Initializes a new instance of an implementation of a token filter.
        /// </summary>
        /// <param name="filtered">The token types to filter.</param>
        public TokenFilter(IEnumerable<TokenType> filtered)
        {
            _filtered = filtered;
        }
    }
}
