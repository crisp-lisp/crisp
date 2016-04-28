using Crisp.Interfaces.Types;

namespace Crisp.Interfaces.Serialization
{
    /// <summary>
    /// Represents a symbolic expression serializer.
    /// </summary>
    public interface ISymbolicExpressionSerializer
    {
        /// <summary>
        /// Serializes a symbolic expression to a string representation.
        /// </summary>
        /// <param name="expression">The expression to serialize.</param>
        /// <returns></returns>
        string Serialize(ISymbolicExpression expression);
    }
}
