using Crisp.Shared;

namespace Crisp.Tokenization
{
    /// <summary>
    /// A token created by the <see cref="Tokenizer"/> class.
    /// </summary>
    internal class Token : IToken
    {
        public TokenType Type { get; }
        
        public string Sequence { get; }
        
        public int Line { get; }
        
        public int Column { get; }

        /// <summary>
        /// Initializes a new instance of a token.
        /// </summary>
        /// <param name="type">The token type.</param>
        /// <param name="sequence">The sequence of characters in the token.</param>
        /// <param name="line">The line position of the token in the source.</param>
        /// <param name="column">The column position of the token in the source.</param>
        public Token(TokenType type, string sequence, int line = -1, int column = -1)
        {
            Type = type;
            Sequence = sequence;
            Line = line;
            Column = column;
        }
    }
}
