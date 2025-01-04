namespace Mpmt.Core.Events
{
    public interface IEventConsumer<T>
    {
        Task HandleEventAsync(T eventMessage);
    }
}
