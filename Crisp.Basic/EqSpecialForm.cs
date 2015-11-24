using Crisp.Core;
using Crisp.Core.Evaluation;
using Crisp.Core.Types;

namespace Crisp.Basic
{
    /// <summary>
    /// Represents the basic equality test function.
    /// </summary>
    public class EqSpecialForm : SpecialForm
    {
        public override string Name => "eq";

        public override SymbolicExpression Apply(SymbolicExpression expression, IEvaluator evaluator)
        {
            expression.ThrowIfNotList(Name); // Takes a list of arguments.

            var arguments = expression.AsPair().Expand();
            arguments.ThrowIfWrongLength(Name, 2); // Must have two arguments.

            // Evaluate both parameters.
            var x = evaluator.Evaluate(arguments[0]);
            var y = evaluator.Evaluate(arguments[1]);

            // Grab true and false bindings from context.
            var t = evaluator.Evaluate(SymbolAtom.True);
            var f = evaluator.Evaluate(SymbolAtom.False);
            
            // Different types can never be equal.
            if (x.Type != y.Type)
            {
                return f;
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
