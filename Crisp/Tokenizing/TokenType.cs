namespace Crisp.Tokenizing
{
    /// <summary>
    /// An enumeration of token types.
    /// </summary>
    internal enum TokenType
    {
        /// <summary>
        /// A token with no known type, used for testing.
        /// </summary>
        None,

        /// <summary>
        /// An opening parenthesis token.
        /// </summary>
        OpeningParenthesis,

        /// <summary>
        /// A closing parenthesis token.
        /// </summary>
        ClosingParenthesis,

        /// <summary>
        /// A symbol token.
        /// </summary>
        Symbol,
        
        /// <summary>
        /// A numeric (integer or real) token.
        /// </summary>
        Numeric,

        /// <summary>
        /// A quoted string token.
        /// </summary>
        String,

        /// <summary>
        /// A dot notation token.
        /// </summary>
        Dot,
    }
}
