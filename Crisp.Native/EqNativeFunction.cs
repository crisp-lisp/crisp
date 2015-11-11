using System;
using System.Diagnostics.Eventing.Reader;
using Crisp.Core;

namespace Crisp.Native
{
    /// <summary>
    /// Represents the basic equality test function.
    /// </summary>
    public class EqNativeFunction : IFunction
    {
        public IEvaluator Host { get; set; }

        public string Name => "eq";

        public SymbolicExpression Apply(SymbolicExpression expression, Context context)
        {
            expression.ThrowIfNotList(Name); // Takes a list of arguments.

            var arguments = expression.AsPair().Expand();
            arguments.ThrowIfWrongLength(Name, 2); // Must have two arguments.

            // Evaluate both parameters.
            var x = Host.Evaluate(arguments[0], context);
            var y = Host.Evaluate(arguments[1], context);

            // Grab true and false bindings from context.
            var t = Host.Evaluate(SymbolAtom.True, context);
            var f = Host.Evaluate(SymbolAtom.False, context);
            
            // Different types can never be equal.
            if (x.Type != y.Type)
            {
                return Host.Evaluate(SymbolAtom.False, context);
            }

            switch (x.Type)
            {
                case SymbolicExpressionType.Constant:
                    return x.AsConstant().Name == y.AsConstant().Name ? t : f;
                case SymbolicExpressionType.Numeric:
                    return x.AsNumeric().Value == y.AsNumeric().Value ? t : f;
                case SymbolicExpressionType.String:
                    return x.AsString().Value == y.AsString().Value ? t : f;
                default:
                    return f;
            }
        }
    }
}
