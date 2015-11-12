using Crisp.Core;
using Crisp.Core.Evaluation;

namespace Crisp.Native
{
    /// <summary>
    /// Represents the basic atomic test function.
    /// </summary>
    public class AtomNativeFunction : IFunction
    {
        public IEvaluator Host { get; set; }

        public string Name => "atom";

        public SymbolicExpression Apply(SymbolicExpression expression, Context context)
        {
            expression.ThrowIfNotList(Name); // Takes a list of arguments.

            var arguments = expression.AsPair().Expand();
            arguments.ThrowIfWrongLength(Name, 1); // Must have one argument.

            var evaluated = Host.Evaluate(arguments[0], context);

            return evaluated.IsAtomic ? Host.Evaluate(SymbolAtom.True, context) 
                : Host.Evaluate(SymbolAtom.False, context);
        }
    }
}
