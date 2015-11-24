namespace Crisp.Core.Types
{
    /// <summary>
    /// Represents an atomic symbolic expression.
    /// </summary>
    /// <typeparam name="T">The type of value this atom represents.</typeparam>
    public abstract class Atom<T> : SymbolicExpression
    {
        public override bool IsAtomic => true;

        /// <summary>
        /// Gets the value of the atom.
        /// </summary>
        public abstract T Value { get; protected set;  }
    }
}
