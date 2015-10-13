using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crisp.Core
{
    public class SymbolicExpression
    {
        public object Value { get; private set; }

        public SymbolicExpression LeftExpression { get; set; }

        public SymbolicExpression RightExpression { get; set; }

        public bool IsAtomic
        {
            get
            {
                return Value != null;
            }
        }

        public SymbolicExpression(SymbolicExpression leftExpression, SymbolicExpression rightExpression)
        {
            LeftExpression = leftExpression;
            RightExpression = rightExpression;
        }
        
        public SymbolicExpression(object value)
        {
            Value = value;
        }
    }
}
