using System.Collections.Generic;

namespace Crisp.Core
{
    /// <summary>
    /// Represents a lambda expression.
    /// </summary>
    public class Lambda : SymbolicExpression, IFunction
    {
        /// <summary>
        /// Contains the list of parameters that the lambda takes.
        /// </summary>
        private readonly IList<SymbolAtom> _parameters;

        /// <summary>
        /// Contains the body of the lambda.
        /// </summary>
        private readonly SymbolicExpression _body;

        public override bool IsAtomic => false;

        public override SymbolicExpressionType Type => SymbolicExpressionType.Function;

        public string Name => null; // Lambdas are anonymous.

        public IFunctionHost Host { get; set; }
        
        public SymbolicExpression Apply(SymbolicExpression expression, Context context)
        {
            // Make sure we've got the right number of arguments.
            var arguments = expression.AsPair().Expand();
            if (arguments.Count != _parameters.Count)
            {
                throw new RuntimeException("Attempted to call lambda with wrong number of arguments.");
            }

            // Bind arguments to parameters in context.
            for (var i = 0; i < _parameters.Count; i++)
            {
                context = context.Bind(_parameters[i], arguments[i]);
            }

            return Host.Evaluate(_body, context);
        }

        /// <summary>
        /// Initializes a new instance of a lambda expression.
        /// </summary>
        /// <param name="host">A reference to the function host, usually the executing interpreter.</param>
        /// <param name="parameters">The list of parameters the lambda will take.</param>
        /// <param name="body">The body of the lambda.</param>
        public Lambda(IFunctionHost host, IList<SymbolAtom> parameters, SymbolicExpression body)
        {
            Host = host;
            _parameters = parameters;
            _body = body;
        }
    }
}
