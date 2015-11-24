namespace Crisp.Core.Types
{
    /// <summary>
    /// Represents an atomic string expression.
    /// </summary>
    public class StringAtom : SymbolicExpression
    {
        public override bool IsAtomic => true;

        public override SymbolicExpressionType Type => SymbolicExpressionType.String;

        /// <summary>
        /// Gets the value of the expression.
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// Initializes a new instance of a string atomic expression.
        /// </summary>
        /// <param name="value">The value of the expression.</param>
        public StringAtom(string value)
        {
            Value = value;
        }
    }
}
