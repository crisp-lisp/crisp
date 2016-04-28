using System.Collections.Generic;
using Crisp.Enums;
using Crisp.Interfaces;
using Crisp.Interfaces.Evaluation;
using Crisp.Interfaces.Types;
using Crisp.Shared;
using Crisp.Types;

namespace Crisp.Basic
{
    /// <summary>
    /// A function that given three expressions returns the value of the second if the value of the first is true, 
    /// otherwise returns the value of the third.
    /// </summary>
    public class IfSpecialForm : SpecialForm
    {
        public override IEnumerable<string> Names => new List<string> {"if"};

        public override ISymbolicExpression Apply(ISymbolicExpression expression, IEvaluator evaluator)
        {
            expression.ThrowIfNotList(Name); // Takes a list of arguments.

            var arguments = expression.AsPair().Expand();
            arguments.ThrowIfWrongLength(Name, 3); // Must have three arguments.
            
            var t = new BooleanAtom(true);
            var f = new BooleanAtom(false);

            // Evaluate predicate.
            var predicate = evaluator.Evaluate(arguments[0]);
            if (predicate.Type != SymbolicExpressionType.Boolean)
            {
                throw new FunctionApplicationException($"The first argument to the function '{Name}' must evaluate to a boolean special atom.");
            }
            
            return predicate.Equals(t) ? evaluator.Evaluate(arguments[1]) 
                : evaluator.Evaluate(arguments[2]);
        }
    }
}
