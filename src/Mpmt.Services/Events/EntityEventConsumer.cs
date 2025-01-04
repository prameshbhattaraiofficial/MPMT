using Mpmt.Core.Events;
using Mpmt.Services.Authentication;

namespace EventManagement.Services.Cache
{
    public partial class EntityEventConsumer :
        IEventConsumer<EntityUpdatedEvent<SessionAuthExpiration>>
    {
        private readonly IUserAuthSessionService _userAuthSessionService;

        public EntityEventConsumer(IUserAuthSessionService userAuthSessionService)
        {
            _userAuthSessionService = userAuthSessionService;
        }

        public async Task HandleEventAsync(EntityUpdatedEvent<SessionAuthExpiration> eventMessage)
        {
            _ = await _userAuthSessionService.AddToExpirationAsync(eventMessage.Entity);
        }
    }
}
