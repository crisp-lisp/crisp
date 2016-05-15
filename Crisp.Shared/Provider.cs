namespace Crisp.Shared
{
    /// <summary>
    /// Represents a generic value provider service.
    /// </summary>
    /// <typeparam name="T">The type of object this service provides.</typeparam>
    public abstract class Provider<T>
    {
        protected T Value;

        /// <summary>
        /// Initializes a new instance of a generic value provider service.
        /// </summary>
        /// <param name="value">The value that this provider should return.</param>
        protected Provider(T value)
        {
            Value = value;
        }

        /// <summary>
        /// Initializes a new instance of a generic value provider service.
        /// </summary>
        protected Provider()
        {
        } 

        /// <summary>
        /// Gets the value from this provider.
        /// </summary>
        /// <returns></returns>
        public T Get()
        {
            return Value;
        }
    }
}
