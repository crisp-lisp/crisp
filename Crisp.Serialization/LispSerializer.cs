using System.Globalization;

using Crisp.Enums;
using Crisp.Interfaces.Serialization;
using Crisp.Interfaces.Types;
using Crisp.Types;

namespace Crisp.Serialization
{
    /// <summary>
    /// Represents a symbolic expression serializer working to standard Lisp-like notation.
    /// </summary>
    public class LispSerializer : ISymbolicExpressionSerializer
    {
        public string Serialize(ISymbolicExpression expression)
        {
            // Base case with atoms.
            if (expression.IsAtomic)
                switch (expression.Type)
                {
                    case SymbolicExpressionType.Numeric:
                        return expression.AsNumeric().Value.ToString(CultureInfo.InvariantCulture);
                    case SymbolicExpressionType.String:
                        return "\"" + expression.AsString().Value + "\"";
                    case SymbolicExpressionType.Symbol:
                        return expression.AsSymbol().Value;
                    case SymbolicExpressionType.Constant:
                        return expression.AsConstant().Value;
                    case SymbolicExpressionType.Boolean:
                        return expression.AsBoolean().Value.ToString();
                        case SymbolicExpressionType.Nil:
                    return "nil";
                }

            // Recurse into nodes.
            var node = expression.AsPair();
            return "(" + Serialize(node.Head) + " . " + Serialize(node.Tail) + ")";
        }
    }
}
