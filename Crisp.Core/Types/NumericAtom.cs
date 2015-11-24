namespace Crisp.Core.Types
{
    /// <summary>
    /// Represents an atomic numeric expression.
    /// </summary>
    public class NumericAtom : SymbolicExpression
    {
        public override bool IsAtomic => true;

        public override SymbolicExpressionType Type => SymbolicExpressionType.Numeric;

        /// <summary>
        /// Gets the value of the expression.
        /// </summary>
        public double Value { get; private set; }

        /// <summary>
        /// Initializes a new instance of a numeric atomic expression.
        /// </summary>
        /// <param name="value">The value of the expression.</param>
        public NumericAtom(double value)
        {
            Value = value;
        }
    }
}
