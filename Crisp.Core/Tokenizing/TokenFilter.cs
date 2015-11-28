using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crisp.Core.Tokenizing
{
    public class TokenFilter
    {
        private readonly IEnumerable<TokenType> _filtered;

        public IList<Token> Filter(IList<Token> tokens)
        {
            return tokens.Where(t => !_filtered.Contains(t.Type)).ToList();
        }

        public TokenFilter(IEnumerable<TokenType> filtered)
        {
            _filtered = filtered;
        }
    }
}
