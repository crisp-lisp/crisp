using Crisp.Core.Evaluation;

namespace Crisp.Core
{
    /// <summary>
    /// Represents a bindable, natively implemented function.
    /// </summary>
    public class NativeFunction : SymbolicExpression, IFunction
    {
        /// <summary>
        /// The underlying native function.
        /// </summary>
        private readonly IFunction _wrapped;
        
        public IEvaluator Host { get; set; }

        public override bool IsAtomic => false;

        public string Name => _wrapped.Name;

        public override SymbolicExpressionType Type => SymbolicExpressionType.Function;

        public SymbolicExpression Apply(SymbolicExpression expression, Context context)
        {
            return _wrapped.Apply(expression, context);
        }

        /// <summary>
        /// Initializes a new instance of a bindable, natively implemented function.
        /// </summary>
        /// <param name="wrapped">The function to wrap.</param>
        public NativeFunction(IFunction wrapped)
        {
            _wrapped = wrapped;
        }
    }
}
