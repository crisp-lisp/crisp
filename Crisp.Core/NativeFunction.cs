namespace Crisp.Core
{
    public class NativeFunction : SymbolicExpression, IFunction
    {
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

        public NativeFunction(IFunction wrapped)
        {
            this.wrapped = wrapped;
        }
    }
}
