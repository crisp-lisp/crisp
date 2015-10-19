namespace Crisp.Core
{
    /// <summary>
    /// Represents an atomic symbolic expression.
    /// </summary>
    public class SymbolAtom : SymbolicExpression
    {
        /// <summary>
        /// The nil special symbol.
        /// </summary>
        public static readonly SymbolAtom Nil = new SymbolAtom("NIL");

        /// <summary>
        /// The true boolean special symbol.
        /// </summary>
        public static readonly SymbolAtom True = new SymbolAtom("T");

        /// <summary>
        /// The false boolean special symbol.
        /// </summary>
        public static readonly SymbolAtom False = new SymbolAtom("F");

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
                return SymbolicExpressionType.Symbol;
            }
        }

        /// <summary>
        /// Gets the name of the symbol.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Initializes a new instance of an an atomic symbolic expression.
        /// </summary>
        /// <param name="name">The name of the symbol.</param>
        public SymbolAtom(string name)
        {
            Name = name;
        }
    }
}
