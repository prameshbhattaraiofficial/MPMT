using Mpmt.Core.Dtos.Notification;
using Mpmt.Core.Dtos.Paging;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.Services.Notification
{
    public interface INotificationService
    {
        Task<IEnumerable<AdminNotificationModel>> GetAdminNotificationAsync();
        Task MarkNotificationAsRead(Guid notificationId);
        Task MarkPartnerNotificationAsRead(Guid notificationId);    
        Task<IEnumerable<PartnerNotificationModel>> GetPartnerNotificationAsync();
        Task<PagedList<NotificationModel>> GetNotificationAsync(NotificationsFilter notifications);
        Task<int> GetAdminNotificationCountAsync();
        Task<int> GetPartnerNotificationCountAsync();
        Task<IEnumerable<NotificationModule>> GetNotificationModuleAsync();
        Task<SprocMessage> AssignModuleRole(int moduleid, int[] roleids);       
        Task<SprocMessage> IUDNotificationAsync(string Message, string ModuleCode, string AdminLink = "", string PartnerLink = "", string PartnerCode = "");
    }
}
