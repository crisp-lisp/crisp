using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Crisp.Core;
using Crisp.Core.Types;

namespace Crisp.Visualization
{
    public class LispSerializer : ISymbolicExpressionSerializer
    {
        public string Serialize(SymbolicExpression expression)
        {
            if (expression == null)
                return SymbolAtom.Nil.Value;

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
                }

            // Recurse into nodes.
            var node = expression.AsPair();
            return "(" + Serialize(node.Head) + " . " + Serialize(node.Tail) + ")";
        }
    }
}
