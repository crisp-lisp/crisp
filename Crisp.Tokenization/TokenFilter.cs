using System.Collections.Generic;
using System.Linq;

using Crisp.Enums;
using Crisp.Interfaces.Tokenization;

namespace Crisp.Tokenization
{
    public class TokenFilter : ITokenFilter
    {
        public IEnumerable<TokenType> Filtered { get; }
        
        public IList<IToken> Filter(IList<IToken> tokens)
        {
            return tokens.Where(t => !Filtered.Contains(t.Type)).ToList();
        }

        /// <summary>
        /// Initializes a new instance of a token filter.
        /// </summary>
        /// <param name="filtered">The token types to be filtered.</param>
        public TokenFilter(IEnumerable<TokenType> filtered)
        {
            Filtered = filtered;
        }
    }
}
