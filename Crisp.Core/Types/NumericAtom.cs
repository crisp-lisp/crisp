namespace Crisp.Core.Types
{
    /// <summary>
    /// Represents an atomic numeric expression.
    /// </summary>
    public sealed class NumericAtom : Atom<double>
    {
        public override SymbolicExpressionType Type => SymbolicExpressionType.Numeric;
        
        public override double Value { get; protected set; }

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
