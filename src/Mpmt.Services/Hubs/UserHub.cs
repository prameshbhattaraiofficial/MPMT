using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Mpmt.Core.Dtos.Roles;
using Mpmt.Data.Repositories.Notification;
using System.Security.Claims;

namespace Mpmt.Services.Hubs
{
    [Authorize]
    public class UserHub : Hub
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly INotificationRepo _notificationRepo;
        private readonly ClaimsPrincipal _loggedInUser;

        public UserHub(IServiceProvider serviceProvider,
             IHttpContextAccessor httpContextAccessor
            )
        {

            _serviceProvider = serviceProvider;
            _notificationRepo = _serviceProvider.GetRequiredService<INotificationRepo>();   
            _httpContextAccessor = httpContextAccessor;
            _loggedInUser = _httpContextAccessor.HttpContext?.User;
        }
       

        public async Task joinGroupAsync()
        {
            var userType = _loggedInUser.FindFirstValue("UserType");
            var UserId = _loggedInUser.FindFirstValue("Id");
            var UserName = _loggedInUser.FindFirstValue(ClaimTypes.Name);
            if (userType != null && userType == "Partner")
            {
                
                var Partnercode = _loggedInUser.FindFirstValue("PartnerCode");
                var connectionstring = await _notificationRepo.GetSignalRconnectionstring($"{userType}:{Partnercode}:{UserId}:{UserName}");
                if( connectionstring == null )
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, Partnercode);
                    await _notificationRepo.AddSignalRconnectionstring($"{userType}:{Partnercode}:{UserId}:{UserName}", $"{Context.ConnectionId}:{Partnercode}");
                }
                else
                {
                    await Groups.RemoveFromGroupAsync(connectionstring, Partnercode);
                    await _notificationRepo.DeleteSignalRconnectionstring($"{userType}:{Partnercode}:{UserId}:{UserName}");
                    await Groups.AddToGroupAsync(Context.ConnectionId, Partnercode);
                    await _notificationRepo.AddSignalRconnectionstring($"{userType}:{Partnercode}:{UserId}:{UserName}", $"{Context.ConnectionId}:{Partnercode}");
                }
                
                

            }
            if(userType != null && userType == "Admin")
            {
                var connectionstring = await _notificationRepo.GetSignalRconnectionstring($"{userType}:{userType}:{UserId}:{UserName}");
                if (connectionstring == null)
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, "Admin");
                    await _notificationRepo.AddSignalRconnectionstring($"{userType}:{userType}:{UserId}:{UserName}", $"{Context.ConnectionId}:Admin");
                }
                else
                {
                    await Groups.RemoveFromGroupAsync(connectionstring, "Admin");
                    await _notificationRepo.DeleteSignalRconnectionstring($"{userType}:{userType}:{UserId}:{UserName}");
                    await Groups.AddToGroupAsync(Context.ConnectionId, "Admin");
                    await _notificationRepo.AddSignalRconnectionstring($"{userType}:{userType}:{UserId}:{UserName}", $"{Context.ConnectionId}:Admin");
                }

            }


        }
        public async Task LeaveGroupAsync()
        {
            var userType = _loggedInUser.FindFirstValue("UserType");
            var UserId = _loggedInUser.FindFirstValue("Id");
            var UserName = _loggedInUser.FindFirstValue(ClaimTypes.Name);
            if (userType != null && userType == "Partner")
            {
                //checkif user already in group 
                var Partnercode = _loggedInUser.FindFirstValue("PartnerCode");
                var connectionstring = await _notificationRepo.GetSignalRconnectionstring($"{userType}:{Partnercode}:{UserId}:{UserName}");
                if (connectionstring != null)
                {
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, Partnercode);
                    await _notificationRepo.DeleteSignalRconnectionstring($"{userType}:{Partnercode}:{UserId}:{UserName}");

                }
               

            }
            if (userType != null && userType == "Admin")
            {
                var connectionstring = await _notificationRepo.GetSignalRconnectionstring($"{userType}:{userType}:{UserId}:{UserName}");
                if (connectionstring != null)
                {
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Admin");
                    await _notificationRepo.DeleteSignalRconnectionstring($"{userType}:{userType}:{UserId}:{UserName}");
                }
            }


        }
        public async Task CountChange()
        {
            var Count = await _notificationRepo.GetAdminNotificationCountAsync("Admin");
            string Message = null;
            await Clients.Groups("Admin").SendAsync("updateTotalCount", Count, Message);
        }
        public async Task PartnerCountChange()
        {
            var Partnercode = _loggedInUser.FindFirstValue("PartnerCode");
            int Count = 0;
            if (Partnercode != null)
            {
                 Count = await _notificationRepo.GetPartnerNotificationCountAsync(Partnercode);
            }
            
            await Clients.Groups(Partnercode).SendAsync("partnerupdateTotalCount", Count);
        }
    }
}
 