using AutoMapper;
using Dapper;
using Mpmt.Core.Dtos.ActivityLog;
using Mpmt.Core.Dtos.Paging;
using Mpmt.Data.Common;
using System.Data;

namespace Mpmt.Data.Repositories.UserActivityLog
{
    /// <summary>
    /// The user activity log repo.
    /// </summary>
    public class UserActivityLogRepo : IUserActivityLogRepo
    {
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserActivityLogRepo"/> class.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        public UserActivityLogRepo(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Adds the async.
        /// </summary>
        /// <param name="activityLogParam">The activity log param.</param>
        /// <returns>A Task.</returns>
        public async Task AddAsync(UserActivityLogParam activityLogParam)
        {
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();

                var param = new DynamicParameters();
                param.Add("@UserName", activityLogParam.UserName);
                param.Add("Email", activityLogParam.Email);
                param.Add("@IsCustomer", activityLogParam.IsCustomer);
                param.Add("@UserAgent", activityLogParam.UserAgent);
                param.Add("@RemoteIpAddress", activityLogParam.RemoteIpAddress);
                param.Add("@HttpMethod", activityLogParam.HttpMethod);
                param.Add("@ControllerName", activityLogParam.ControllerName);
                param.Add("@ActionName", activityLogParam.ActionName);
                param.Add("@QueryString", activityLogParam.QueryString);
                param.Add("@IsFormData", activityLogParam.IsFormData);
                param.Add("@RequestBody", activityLogParam.RequestBody);
                param.Add("@Headers", activityLogParam.Headers);
                param.Add("@RequestUrl", activityLogParam.RequestUrl);
                param.Add("@MachineName", activityLogParam.MachineName);
                param.Add("@Environment", activityLogParam.Environment);
                param.Add("@UserAction", activityLogParam.UserAction);
                _ = await connection.ExecuteAsync("[dbo].[usp_log_user_activity]", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the activity log async.
        /// </summary>
        /// <param name="userAtivityLogFilter">The user ativity log filter.</param>
        /// <returns>A Task.</returns>
        public async Task<PagedList<UserActivityLogDetails>> GetActivityLogAsync(UserAtivityLogFilter userAtivityLogFilter)
        {
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();

                var param = new DynamicParameters();
                param.Add("@UserAction", userAtivityLogFilter.UserAction);
                param.Add("@UserName", userAtivityLogFilter.UserName);
                param.Add("Email", userAtivityLogFilter.Email);
                param.Add("@UserType", userAtivityLogFilter.UserType);
                param.Add("@FromDate", userAtivityLogFilter.FromDate);
                param.Add("@ToDate", userAtivityLogFilter.ToDate);
                param.Add("@PageNumber", userAtivityLogFilter.PageNumber);
                param.Add("@PageSize", userAtivityLogFilter.PageSize);
                param.Add("@SortingCol", userAtivityLogFilter.SortBy);
                param.Add("@SortType", userAtivityLogFilter.SortOrder);
                param.Add("@SearchVal", userAtivityLogFilter.SearchVal);
                param.Add("@Export", userAtivityLogFilter.Export);
                var data = await connection
                 .QueryMultipleAsync("[dbo].[usp_get_user_activity_report]", param: param, commandType: CommandType.StoredProcedure);

                var LogList = await data.ReadAsync<UserActivityLogDetails>();
                var pagedInfo = await data.ReadFirstAsync<PageInfo>();
                var mappeddata = _mapper.Map<PagedList<UserActivityLogDetails>>(pagedInfo);
                mappeddata.Items = LogList;
                return mappeddata;

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the request response api log by id.
        /// </summary>
        /// <param name="logId">The log id.</param>
        /// <returns>A Task.</returns>
        public async Task<VendorApiLogDetail> GetRequestResponseApiLogById(string logId)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@LogId", logId);
            return await connection.QueryFirstOrDefaultAsync<VendorApiLogDetail>("[dbo].[usp_get_requestresponseapilog_bylogid]", param, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Gets the vendor api log by id.
        /// </summary>
        /// <param name="logId">The log id.</param>
        /// <returns>A Task.</returns>
        public async Task<VendorApiLogDetail> GetVendorApiLogById(string logId)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@LogId", logId);
            return await connection.QueryFirstOrDefaultAsync<VendorApiLogDetail>("[dbo].[usp_get_vendorapilog_bylogid]", param, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Gets the vendor api log report.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>A Task.</returns>
        public async Task<PagedList<VendorApiLogReport>> GetVendorApiLogReport(VendorApiLogFilter filter)
        {
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();

                var param = new DynamicParameters();
                param.Add("@PartnerCode", filter.PartnerCode);
                param.Add("@StartDate", filter.StartDate);
                param.Add("EndDate", filter.EndDate);
                param.Add("@TransactionId", filter.TransactionId);
                param.Add("@TrackerId", filter.TrackerId);

                param.Add("@UserType", filter.UserType);
                param.Add("@PageNumber", filter.PageNumber);
                param.Add("@PageSize", filter.PageSize);
                param.Add("@SortingCol", filter.SortOrder);
                param.Add("@SortType", filter.SortBy);
                param.Add("@SearchVal", filter.SearchVal);

                var data = await connection
                 .QueryMultipleAsync("[dbo].[usp_get_vendor_api_logs_report]", param: param, commandType: CommandType.StoredProcedure);

                var LogList = await data.ReadAsync<VendorApiLogReport>();
                var pagedInfo = await data.ReadFirstAsync<PageInfo>();
                var mappeddata = _mapper.Map<PagedList<VendorApiLogReport>>(pagedInfo);
                mappeddata.Items = LogList;
                return mappeddata;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the vendor request response api log by id.
        /// </summary>
        /// <param name="logId">The log id.</param>
        /// <returns>A Task.</returns>
        public async Task<VendorApiLogDetail> GetVendorRequestResponseApiLogById(string logId)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@LogId", logId);
            return await connection.QueryFirstOrDefaultAsync<VendorApiLogDetail>("[dbo].[usp_get_Vendorrequestresponseapilog_bylogid]", param, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Gets the vendor request response api log by id2.
        /// </summary>
        /// <param name="logId">The log id.</param>
        /// <returns>A Task.</returns>
        public async Task<VendorApiLogDetail> GetVendorRequestResponseApiLogById2(string logId)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@LogId", logId);
            return await connection.QueryFirstOrDefaultAsync<VendorApiLogDetail>("[dbo].[usp_get_Vendorrequestresponseapilog2_bylogid]", param, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Gets the vendor request response api log by id3.
        /// </summary>
        /// <param name="logId">The log id.</param>
        /// <returns>A Task.</returns>
        public async Task<VendorApiLogDetail> GetVendorRequestResponseApiLogById3(string logId)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@LogId", logId);
            return await connection.QueryFirstOrDefaultAsync<VendorApiLogDetail>("[dbo].[usp_get_Vendorrequestresponseapilog3_bylogid]", param, commandType: CommandType.StoredProcedure);
        }
    }
}
