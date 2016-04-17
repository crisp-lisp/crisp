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
        RequireStatement,
        Whitespace,
        Comment,
        BooleanTrue,
        BooleanFalse,
        Nil,
    }
}
