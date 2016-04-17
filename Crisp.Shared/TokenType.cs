namespace Crisp.Shared
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
        Whitespace,
        Comment,
        BooleanTrue,
        BooleanFalse,
        Nil,
    }
}
