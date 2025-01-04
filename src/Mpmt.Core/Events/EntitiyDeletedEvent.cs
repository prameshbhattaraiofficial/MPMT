namespace Mpmt.Core.Events
{
    public class EntitiyDeletedEvent<T> where T : class
    {
        public EntitiyDeletedEvent(T entity)
        {
            Entity = entity;
        }

        public T Entity { get; set; }
    }
}
