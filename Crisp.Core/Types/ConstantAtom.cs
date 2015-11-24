namespace Crisp.Core.Types
{
    /// <summary>
    /// Represents an atomic constant expression.
    /// </summary>
    public sealed class ConstantAtom : Atom<string>
    {
        public override SymbolicExpressionType Type => SymbolicExpressionType.Constant;
        
        public override string Value { get; protected set; }

        /// <summary>
        /// Initializes a new instance of an atomic constant expression.
        /// </summary>
        /// <param name="value">The Value of the constant.</param>
        public ConstantAtom(string value)
        {
            Value = value;
        }
    }
}
