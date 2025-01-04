using Mpmt.Core.Dtos.ActivityLog;
using Mpmt.Core.Dtos.Paging;
using Mpmt.Data.Repositories.UserActivityLog;

namespace Mpmt.Services.Services.UserActivityLog
{
    /// <summary>
    /// The user activity log.
    /// </summary>
    public class UserActivityLog : IUserActivityLog
    {
        private readonly IUserActivityLogRepo _activityLog;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserActivityLog"/> class.
        /// </summary>
        /// <param name="activityLog">The activity log.</param>
        public UserActivityLog(IUserActivityLogRepo activityLog)
        {
            _activityLog = activityLog;
        }

        /// <summary>
        /// Adds the async.
        /// </summary>
        /// <param name="activityLogParam">The activity log param.</param>
        /// <returns>A Task.</returns>
        public async Task AddAsync(UserActivityLogParam activityLogParam)
        {
            await _activityLog.AddAsync(activityLogParam);
        }

        /// <summary>
        /// Gets the activity log async.
        /// </summary>
        /// <param name="userAtivityLogFilter">The user ativity log filter.</param>
        /// <returns>A Task.</returns>
        public async Task<PagedList<UserActivityLogDetails>> GetActivityLogAsync(UserAtivityLogFilter userAtivityLogFilter)
        {
            var data = await _activityLog.GetActivityLogAsync(userAtivityLogFilter);
            return data;
        }

        public async Task<VendorApiLogDetail> GetRequestResponseApiLogById(string logId)
        {
            var data = await _activityLog.GetRequestResponseApiLogById(logId);
            return data;
        }

        public async Task<VendorApiLogDetail> GetVendorApiLogById(string logId)
        {
            var data = await _activityLog.GetVendorApiLogById(logId);
            return data;
        }

        public async Task<PagedList<VendorApiLogReport>> GetVendorApiLogReport(VendorApiLogFilter filter)
        {
            var data = await _activityLog.GetVendorApiLogReport(filter);
            return data;
        }

        public async Task<VendorApiLogDetail> GetVendorRequestResponseApiLogById(string logId)
        {
            var data = await _activityLog.GetVendorRequestResponseApiLogById(logId);
            return data;
        }

        public async Task<VendorApiLogDetail> GetVendorRequestResponseApiLogById2(string logId)
        {
            var data = await _activityLog.GetVendorRequestResponseApiLogById2(logId);
            return data;
        }

        public async Task<VendorApiLogDetail> GetVendorRequestResponseApiLogById3(string logId)
        {
            var data = await _activityLog.GetVendorRequestResponseApiLogById3(logId);
            return data;
        }
    }
}
