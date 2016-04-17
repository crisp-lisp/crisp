using Crisp.Shared;

namespace Crisp.Types
{
    /// <summary>
    /// Represents an atomic string expression.
    /// </summary>
    public sealed class StringAtom : Atom<string>
    {
        public override SymbolicExpressionType Type => SymbolicExpressionType.String;
        
        public override string Value { get; protected set; }

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
