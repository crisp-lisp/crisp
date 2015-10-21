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
        private IFunction wrapped;
        
        public IFunctionHost Host { get; set; }

        public override bool IsAtomic
        {
            get
            {
                return false;
            }
        }

        public string Name
        {
            get
            {
                return wrapped.Name;
            }
        }

        public override SymbolicExpressionType Type
        {
            get
            {
                return SymbolicExpressionType.Function;
            }
        }

        public SymbolicExpression Apply(SymbolicExpression input, Context context)
        {
            return wrapped.Apply(input, context);
        }

        /// <summary>
        /// Initializes a new instance of a bindable, natively implemented function.
        /// </summary>
        /// <param name="wrapped">The function to wrap.</param>
        public NativeFunction(IFunction wrapped)
        {
            this.wrapped = wrapped;
        }
    }
}
