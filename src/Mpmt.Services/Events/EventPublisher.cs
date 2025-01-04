using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Mpmt.Core.Events;

namespace Mpmt.Services.Events
{
    public class EventPublisher : IEventPublisher
    {
        private readonly ILogger<EventPublisher> _logger;
        private readonly IServiceProvider _serviceProvider;

        public EventPublisher(
            ILogger<EventPublisher> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public virtual async Task PublishAsync<TEvent>(TEvent @event)
        {
            var consumers = _serviceProvider.GetServices<IEventConsumer<TEvent>>();

            try
            {
                var handleEventTasks = consumers.Select(c => Task.Run(() => c.HandleEventAsync(@event)));
                await Task.WhenAll(handleEventTasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
        }
    }
}
