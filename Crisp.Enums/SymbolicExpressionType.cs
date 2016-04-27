namespace Crisp.Enums
{
    /// <summary>
    /// An enumeration of expression data types.
    /// </summary>
    public enum SymbolicExpressionType
    {
        /// <summary>
        /// Specifies that an expression is a pair structure (cons cell).
        /// </summary>
        Pair,

        /// <summary>
        /// Specifies that an expression is a symbol.
        /// </summary>
        Symbol,
        
        /// <summary>
        /// Specifies that an expression is a constant.
        /// </summary>
        Constant,

        /// <summary>
        /// Specifies that an expression is numeric.
        /// </summary>
        Numeric,

        /// <summary>
        /// Specifies that an expression is a string.
        /// </summary>
        String,

        /// <summary>
        /// Specifies that an expression is a function.
        /// </summary>
        Function,

        /// <summary>
        /// Specifies that an expression is a boolean value.
        /// </summary>
        Boolean,

        /// <summary>
        /// Specifies that an expression is nil.
        /// </summary>
        Nil
    }
}
