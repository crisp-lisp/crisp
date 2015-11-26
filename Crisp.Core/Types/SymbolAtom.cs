namespace Crisp.Core.Types
{
    /// <summary>
    /// Represents an atomic symbolic expression.
    /// </summary>
    public sealed class SymbolAtom : Atom<string>
    {
        public override SymbolicExpressionType Type => SymbolicExpressionType.Symbol;
        
        public override string Value { get; protected set; }

        /// <summary>
        /// Gets whether or not another symbol atom matches the name of this one.
        /// </summary>
        /// <param name="other">The other symbol atom.</param>
        /// <returns></returns>
        public bool Matches(SymbolAtom other)
        {
            return Value == other.Value;
        }

        /// <summary>
        /// Initializes a new instance of an atomic symbolic expression.
        /// </summary>
        /// <param name="name">The name of the symbol.</param>
        public SymbolAtom(string name)
        {
            Value = name;
        }
    }
}
