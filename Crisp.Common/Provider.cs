namespace Crisp.Common
{
    public abstract class Provider<T>
    {
        protected T Obj;

        protected Provider(T obj)
        {
            Obj = obj;
        }

        protected Provider()
        {
            
        } 

        public T Get()
        {
            return Obj;
        }
    }
}
