using Mpmt.Core.Dtos.Notification;
using Mpmt.Core.Dtos.Paging;
using Mpmts.Core.Dtos;

namespace Mpmt.Data.Repositories.Notification
{
    public interface INotificationRepo
    {
        Task<IEnumerable<AdminNotificationModel>> GetAdminNotificationAsync(string userName);
        Task MarkNotificationAsRead(string userName, Guid notificationId);
        Task MarkPartnerNotificationAsRead(string partnerCode, Guid notificationId);    
        Task<IEnumerable<PartnerNotificationModel>> GetPartnerNotificationAsync(string partnerCode);
        Task<PagedList<NotificationModel>> GetNotificationAsync(NotificationsFilter notifications);    
        Task<int> GetAdminNotificationCountAsync(string userName);
        Task<IEnumerable<NotificationModule>> GetNotificationModuleAsync();
        Task<SprocMessage> AssignModuleRole(int moduleid, int[] roleids);
        Task<int> GetPartnerNotificationCountAsync(string partnerCode);
        Task<SprocMessage> IUDNotificationAsync(IUDNotification notification);

        Task<string> GetSignalRconnectionstring(string ConnectionIdentifier);
        Task DeleteSignalRconnectionstring(string ConnectionIdentifier);
        Task AddSignalRconnectionstring(string ConnectionIdentifier, string connectionstring);

    }
}
    