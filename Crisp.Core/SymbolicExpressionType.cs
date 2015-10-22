namespace Crisp.Core
{
    /// <summary>
    /// An enumeration of different expression data types.
    /// </summary>
    public enum SymbolicExpressionType
    {
        /// <summary>
        /// Represents a pair.
        /// </summary>
        Pair,
        Symbol,
        Constant,
        Numeric,
        String,
        Function,
    }
}
