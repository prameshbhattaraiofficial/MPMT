namespace Mpmt.Core.Events
{
    public class EntityInsertedEvent<T> where T : class
    {
        public EntityInsertedEvent(T entity)
        {
            Entity = entity;
        }

        public T Entity { get; set; }
    }
}
