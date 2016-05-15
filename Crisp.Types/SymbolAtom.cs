using Crisp.Enums;

namespace Crisp.Types
{
    /// <summary>
    /// Represents an atomic symbolic expression.
    /// </summary>
    public sealed class SymbolAtom : Atom<string>
    {
        public override SymbolicExpressionType Type => SymbolicExpressionType.Symbol;
        
        public override string Value { get; protected set; }

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
