using Crisp.Core;

namespace Crisp.Native
{
    /// <summary>
    /// Represents the basic atomic test function.
    /// </summary>
    public class IfNativeFunction : IFunction
    {
        public IEvaluator Host { get; set; }

        public string Name => "if";

        public SymbolicExpression Apply(SymbolicExpression expression, Context context)
        {
            expression.ThrowIfNotList(Name); // Takes a list of arguments.

            var arguments = expression.AsPair().Expand();
            arguments.ThrowIfWrongLength(Name, 3); // Must have three arguments.

            // Grab true and false bindings from context.
            var t = Host.Evaluate(SymbolAtom.True, context);
            var f = Host.Evaluate(SymbolAtom.False, context);

            // Evaluate predicate.
            var predicate = Host.Evaluate(arguments[0], context);
            if (predicate != t && predicate != f)
            {
                throw new RuntimeException($"The first argument to the function '{Name}' must evaluate to a boolean special atom.");
            }
            
            return predicate == t ? Host.Evaluate(arguments[1], context) 
                : Host.Evaluate(arguments[2], context);
        }
    }
}
