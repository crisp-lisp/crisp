namespace Crisp.Core.Tokenizing
{
    /// <summary>
    /// An enumeration of token types.
    /// </summary>
    public enum TokenType
    {
        None,
        OpeningParenthesis,
        ClosingParenthesis,
        Symbol,
        Numeric,
        String,
        Dot,
        ImportStatement,
        Whitespace,
        Comment,
        BooleanTrue,
        BooleanFalse,
        Nil,
    }
}
