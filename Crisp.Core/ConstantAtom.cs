namespace Crisp.Core
{
    /// <summary>
    /// Represents an atomic constant expression.
    /// </summary>
    public class ConstantAtom : SymbolicExpression
    {
        public override bool IsAtomic => true;

        public override SymbolicExpressionType Type => SymbolicExpressionType.Constant;

        /// <summary>
        /// Gets the name of the constant.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Initializes a new instance of an atomic constant expression.
        /// </summary>
        /// <param name="name">The name of the constant.</param>
        public ConstantAtom(string name)
        {
            Name = name;
        }

        public ConstantAtom(SymbolAtom symbol)
        {
            Name = symbol.Name;
        }
    }
}
