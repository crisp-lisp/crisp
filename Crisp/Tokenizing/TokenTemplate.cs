using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Crisp.Tokenizing
{
    internal class TokenTemplate
    {
        public Regex Pattern { get; private set; }

        public TokenType Type { get; private set; }

        public TokenTemplate(Regex pattern, TokenType type)
        {
            Pattern = pattern;
            Type = type;
        }

        public TokenTemplate(string pattern, TokenType type)
            : this(new Regex(pattern), type)
        {

        }
    }
}
