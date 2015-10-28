using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Crisp.Core;

namespace Crisp.Native
{
    public class LetNativeFunction : IFunction
    {
        public IFunctionHost Host { get; set; }

        public string Name => "let";

        public SymbolicExpression Apply(SymbolicExpression input, Context context)
        {
            // Ensure arguments are in list form.
            if (input.Type != SymbolicExpressionType.Pair)
            {
                throw new RuntimeException("Native function let requires a list of arguments.");
            }
            
            // Argument list is always a node.
            var arguments = input.AsPair(); 

            // The expression to be returned by the let.
            var exp = arguments.Head;

            // Build a context containing all subsequent pairs.
            var current = arguments.GoTail();
            var newContext = context;
            while (current != null)
            {
                var name = current.GoHead().Head;
                var value = Host.Evaluate(current.GoHead().Tail, context);

                newContext = newContext.Bind(name.AsSymbol(), value);

                current = current.Tail.Type == SymbolicExpressionType.Pair
                    ? current.GoTail()
                    : null;
            }

            // Evaluate expression in our new context.
            var result = Host.Evaluate(exp, newContext);

            return result;
        }
    }
}
