using System.Collections.Generic;
using System.Linq;
using Crisp.Enums;
using Crisp.Interfaces.Tokenization;
using Crisp.Shared;

namespace Crisp.Tokenization
{
    /// <summary>
    /// A token filter, used to remove <see cref="IToken"/> instances with a particular <see cref="TokenType"/>.
    /// </summary>
    public class TokenFilter : ITokenFilter
    {
        private readonly IEnumerable<TokenType> _filtered;

        /// <summary>
        /// Filters a token list.
        /// </summary>
        /// <param name="tokens">The list of tokens to filter.</param>
        /// <returns></returns>
        public IList<IToken> Filter(IList<IToken> tokens)
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
