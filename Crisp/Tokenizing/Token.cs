namespace Crisp.Tokenizing
{
    /// <summary>
    /// Represents a token.
    /// </summary>
    internal class Token
    {
        /// <summary>
        /// Gets the token type.
        /// </summary>
        public TokenType Type { get; private set; }

        /// <summary>
        /// Gets the sequence of characters in the token.
        /// </summary>
        public string Sequence { get; private set; }

        /// <summary>
        /// Gets the line position of the token in the source.
        /// </summary>
        public int Line { get; private set; }

        /// <summary>
        /// Gets the column position of the token in the source.
        /// </summary>
        public int Column { get; private set; }

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
