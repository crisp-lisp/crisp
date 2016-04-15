namespace Crisp.Common
{
    public abstract class Provider<T>
    {
        protected T Value;

        protected Provider(T value)
        {
            Value = value;
        }

        protected Provider()
        {
            
        } 

        public T Get()
        {
            return Value;
        }
    }
}
