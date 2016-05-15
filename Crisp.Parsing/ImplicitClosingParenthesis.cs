using Crisp.Enums;
using Crisp.Interfaces.Tokenization;

namespace Crisp.Parsing
{
    /// <summary>
    /// Represents an implicit closing parenthesis token added during parsing.
    /// </summary>
    internal class ImplicitClosingParenthesis : IToken
    {
        public TokenType Type { get; } = TokenType.ClosingParenthesis;

        public string Sequence { get; } = string.Empty;

        public int Line { get; } = -1;

        public int Column { get; } = -1;
    }
}
