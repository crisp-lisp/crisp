using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Crisp.Core;

namespace Crisp.Visualization
{
    public class LispSerializer : ISymbolicExpressionSerializer
    {
        public string Serialize(SymbolicExpression expression)
        {
            if (expression == null)
                return SymbolAtom.Nil.Name;

            // Base case with atoms.
            if (expression.IsAtomic)
                switch (expression.Type)
                {
                    case SymbolicExpressionType.Numeric:
                        return expression.AsNumeric().Value.ToString(CultureInfo.InvariantCulture);
                    case SymbolicExpressionType.String:
                        return "\"" + expression.AsString().Value + "\"";
                    case SymbolicExpressionType.Symbol:
                        return expression.AsSymbol().Name;
                    case SymbolicExpressionType.Constant:
                        return expression.AsConstant().Name;
                }

            // Recurse into nodes.
            var node = expression.AsPair();
            return "(" + Serialize(node.Head) + " . " + Serialize(node.Tail) + ")";
        }
    }
}
