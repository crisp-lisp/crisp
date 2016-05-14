using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crisp.Enums;
using Crisp.Interfaces.Types;
using Crisp.Types;
using Packet.Interfaces.Server;

namespace Packet.Server
{
    public class DynamicPageResultValidator : IDynamicPageResultValidator
    {
        public bool Validate(ISymbolicExpression result)
        {
            if (result.Type != SymbolicExpressionType.Pair)
            {
                return false;
            }

            var pair = result.AsPair().Expand(); // Expand list.

            // Check types in list.
            return pair[0].Type == SymbolicExpressionType.String
                   && pair[1].Type == SymbolicExpressionType.Numeric
                   && pair[2].Type == SymbolicExpressionType.String
                   && (pair[3].Type == SymbolicExpressionType.Pair || pair[3].Type == SymbolicExpressionType.Nil);
        }
    }
}
