using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crisp.Tokenizing
{
    public class TokenizationException : Exception
    {
        public int Position { get; private set; }

        public TokenizationException(string message, int position) 
            : base(message)
        {
            Position = position;
        }
    }
}
