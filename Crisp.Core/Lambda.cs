using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crisp.Core
{
    public class Lambda : SymbolicExpression, IFunction
    {
        public override bool IsAtomic => false;

        public override SymbolicExpressionType Type => SymbolicExpressionType.Function;

        public string Name => null; // Lambdas are anonymous.

        public IFunctionHost Host { get; set; }

        public IList<SymbolAtom> Parameters { get; private set; }

        public SymbolicExpression Body { get; private set; }
        
        public SymbolicExpression Apply(SymbolicExpression expression, Context context)
        {
            var arguments = expression.AsPair().Expand();
            if (arguments.Count != Parameters.Count)
            {
                throw new RuntimeException("Attempted to call lambda with wrong number of arguments.");
            }

            for (var i = 0; i < Parameters.Count; i++)
            {
                context = context.Bind(Parameters[i], arguments[i]);
            }

            return Host.Evaluate(Body, context);
        }

        public Lambda(IFunctionHost host, IList<SymbolAtom> parameters, SymbolicExpression body)
        {
            Host = host;
            Parameters = parameters;
            Body = body;
        }
    }
}
