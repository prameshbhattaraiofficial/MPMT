using Microsoft.AspNetCore.Http;
using Mpmt.Core.Dtos.Notification;
using Mpmt.Core.Dtos.Paging;
using Mpmt.Data.Repositories.Notification;
using Mpmt.Services.Services.Common;
using Mpmts.Core.Dtos;
using System.Security.Claims;

namespace Mpmt.Services.Services.Notification
{
  
    public class NotificationService : BaseService, INotificationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly INotificationRepo _notificationRepo;
        private readonly ClaimsPrincipal _loggedInUser;

        public NotificationService(
            IHttpContextAccessor httpContextAccessor,
            INotificationRepo notificationRepo)
        {
            _httpContextAccessor = httpContextAccessor;
            _notificationRepo = notificationRepo;
            _loggedInUser = _httpContextAccessor.HttpContext?.User;
        }

        public async Task<SprocMessage> AssignModuleRole(int moduleid, int[] roleids)
        {
            var response = await _notificationRepo.AssignModuleRole(moduleid, roleids);
            return response;
        }

        public async Task<IEnumerable<AdminNotificationModel>> GetAdminNotificationAsync()
        {
            var UserName = _loggedInUser.FindFirstValue(ClaimTypes.Name);
            var data = await _notificationRepo.GetAdminNotificationAsync(UserName);
            return data;
        }

        public async Task<int> GetAdminNotificationCountAsync()
        {
            var UserName = _loggedInUser.FindFirstValue(ClaimTypes.Name);
            var data = await _notificationRepo.GetAdminNotificationCountAsync(UserName);
            return data;
        }

        public async Task<PagedList<NotificationModel>> GetNotificationAsync(NotificationsFilter notifications)
        {
            var data = await _notificationRepo.GetNotificationAsync(notifications);
            return data;
        }

        public async Task<IEnumerable<NotificationModule>> GetNotificationModuleAsync()
        {
            var data = await _notificationRepo.GetNotificationModuleAsync();
            return data;
        }

        public async Task<IEnumerable<PartnerNotificationModel>> GetPartnerNotificationAsync()
        {
            var PartnerCode = _loggedInUser.FindFirstValue("PartnerCode");
            var data = await _notificationRepo.GetPartnerNotificationAsync(PartnerCode);
            return data;
        }

        public async Task<int> GetPartnerNotificationCountAsync()
        {
            var PartnerCode = _loggedInUser.FindFirstValue("PartnerCode");
            var data = await _notificationRepo.GetPartnerNotificationCountAsync(PartnerCode);
            return data;
        }

        public async Task<SprocMessage> IUDNotificationAsync(string Message, string ModuleCode, string AdminLink = "", string PartnerLink = "", string PartnerCode = "")
        {
            var notification = new IUDNotification();
            var userType = _loggedInUser.FindFirstValue("UserType");
            notification.Event = 'I';
            notification.UserId = _loggedInUser.FindFirstValue("Id");

           
                notification.UserType = userType;
                if (userType == "Partner")
                {
                    notification.PartnerCode = _loggedInUser.FindFirstValue("PartnerCode");
                    
                }
                

            notification.PartnerCode = PartnerCode;
            notification.Message = Message;
            notification.AdminLink = AdminLink;
            notification.PartnerLink = PartnerLink;
            notification.ModuleCode = ModuleCode;
            notification.UserType = userType;
            if (userType == "Partner")
            {
                notification.PartnerCode = _loggedInUser.FindFirstValue("PartnerCode");
            }

            var response = await _notificationRepo.IUDNotificationAsync(notification);
            return response;
        }

        public async Task MarkNotificationAsRead(Guid notificationId)
        {
            var UserName = _loggedInUser.FindFirstValue(ClaimTypes.Name);
            await _notificationRepo.MarkNotificationAsRead(UserName, notificationId);
        }

        public async Task MarkPartnerNotificationAsRead(Guid notificationId)
        {
            var partnerCode = _loggedInUser.FindFirstValue("PartnerCode");
            await _notificationRepo.MarkPartnerNotificationAsRead(partnerCode, notificationId); 
        }
    }
}
