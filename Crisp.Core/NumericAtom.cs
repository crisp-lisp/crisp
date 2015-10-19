namespace Crisp.Core
{
    /// <summary>
    /// Represents an atomic numeric expression.
    /// </summary>
    public class NumericAtom : SymbolicExpression
    {
        public override bool IsAtomic
        {
            get
            {
                return true;
            }
        }

        public override SymbolicExpressionType Type
        {
            get
            {
                return SymbolicExpressionType.Numeric;
            }
        }

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
