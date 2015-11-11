using System.Collections.Generic;

namespace Crisp.Core
{
    /// <summary>
    /// Represents a lambda expression and context bound together.
    /// </summary>
    public class Closure : SymbolicExpression, IFunction
    {
        /// <summary>
        /// The lambda that underlies the closure.
        /// </summary>
        private readonly Lambda _lambda;

        /// <summary>
        /// The context captured by the closure.
        /// </summary>
        private readonly Context _context;

        public override bool IsAtomic => false;

        public override SymbolicExpressionType Type => SymbolicExpressionType.Function;

        public string Name => null; // Closures are anonymous.

        public IEvaluator Host { get; set; }

        public SymbolicExpression Apply(SymbolicExpression expression, Context context)
        {
            return _lambda.Apply(expression, _context); // Ignore the context we were passed.
        }

        /// <summary>
        /// Initializes a new instance of a closure.
        /// </summary>
        /// <param name="host">A reference to the function host, usually the executing interpreter.</param>
        /// <param name="parameters">The list of parameters the closure will take.</param>
        /// <param name="body">The body of the closure.</param>
        /// <param name="context">The parent context of the closure.</param>
        public Closure(IEvaluator host, IList<SymbolAtom> parameters, SymbolicExpression body, Context context)
        {
            _lambda = new Lambda(host, parameters, body);
            _context = context;
            Host = host;
        }
    }
}
