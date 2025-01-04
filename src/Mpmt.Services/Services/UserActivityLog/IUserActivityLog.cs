using Mpmt.Core.Dtos.ActivityLog;
using Mpmt.Core.Dtos.Paging;

namespace Mpmt.Services.Services.UserActivityLog
{
    /// <summary>
    /// The user activity log.
    /// </summary>
    public interface IUserActivityLog
    {
        /// <summary>
        /// Adds the async.
        /// </summary>
        /// <param name="activityLogParam">The activity log param.</param>
        /// <returns>A Task.</returns>
        Task AddAsync(UserActivityLogParam activityLogParam);
        /// <summary>
        /// Gets the activity log async.
        /// </summary>
        /// <param name="userAtivityLogFilter">The user ativity log filter.</param>
        /// <returns>A Task.</returns>
        Task<PagedList<UserActivityLogDetails>> GetActivityLogAsync(UserAtivityLogFilter userAtivityLogFilter);
        Task<PagedList<VendorApiLogReport>> GetVendorApiLogReport(VendorApiLogFilter filter);
        Task<VendorApiLogDetail> GetVendorApiLogById(string logId);
        Task<VendorApiLogDetail> GetRequestResponseApiLogById(string logId);
        Task<VendorApiLogDetail> GetVendorRequestResponseApiLogById(string logId);  
        Task<VendorApiLogDetail> GetVendorRequestResponseApiLogById2(string logId);  
        Task<VendorApiLogDetail> GetVendorRequestResponseApiLogById3(string logId);     
    }
}
