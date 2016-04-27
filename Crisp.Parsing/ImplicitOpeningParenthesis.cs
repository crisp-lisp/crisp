using Crisp.Enums;
using Crisp.Interfaces.Tokenization;

namespace Crisp.Parsing
{
    /// <summary>
    /// Represents an implicit opening parenthesis token added during parsing.
    /// </summary>
    internal class ImplicitOpeningParenthesis : IToken
    {
        public TokenType Type { get; } = TokenType.OpeningParenthesis;

        public string Sequence { get; } = string.Empty;

        public int Line { get; } = -1;

        public int Column { get; } = -1;
    }
}
