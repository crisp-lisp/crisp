namespace Crisp.Common
{
    /// <summary>
    /// Represents a generic value provider service.
    /// </summary>
    /// <typeparam name="T">The type of object this service provides.</typeparam>
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
