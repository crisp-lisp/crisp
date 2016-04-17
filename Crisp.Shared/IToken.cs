namespace Crisp.Shared
{
    public interface IToken
    {
        /// <summary>
        /// Gets the token type.
        /// </summary>
        TokenType Type { get; }

        /// <summary>
        /// Gets the sequence of characters in the token.
        /// </summary>
        string Sequence { get; }

        /// <summary>
        /// Gets the line position of the token in the source.
        /// </summary>
        int Line { get; }

        /// <summary>
        /// Gets the column position of the token in the source.
        /// </summary>
        int Column { get; }
    }
}
