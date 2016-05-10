using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crisp.Interfaces.Parsing;
using Crisp.Interfaces.Serialization;
using Crisp.Interfaces.Types;

namespace Packet.Server
{
    public class ExpressionTreeSource : IExpressionTreeSource
    {
        private readonly ISymbolicExpression _expression;

        public ExpressionTreeSource(ISymbolicExpression expression)
        {
            _expression = expression;
        }

        public ISymbolicExpression Get()
        {
            return _expression;
        }
    }
}
