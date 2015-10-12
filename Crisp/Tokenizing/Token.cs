using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crisp.Tokenizing
{
    internal class Token
    {
        public TokenType Type { get; private set; }

        public string Sequence { get; private set; }

        public Token(TokenType type, string sequence)
        {
            Type = type;
            Sequence = sequence;
        }
    }
}
