using Dapper;
using Mpmt.Core.Dtos.Logging;
using Mpmt.Data.Common;
using System.Data;

namespace Mpmt.Data.Repositories.Logging
{
    public class ExceptionLogRepository : IExceptionLogRepository
    {
        public async Task AddAsync(ExceptionLogParam logParam)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@LogId", logParam.LogId);
            param.Add("@UserName", logParam.UserName);
            param.Add("@UserAgent", logParam.UserAgent);
            param.Add("@RemoteIpAddress", logParam.RemoteIpAddress);
            param.Add("@ControllerName", logParam.ControllerName);
            param.Add("@ActionName", logParam.ActionName);
            param.Add("@QueryString", logParam.QueryString);
            param.Add("@Headers", logParam.Headers);
            param.Add("@RequestUrl", logParam.RequestUrl);
            param.Add("@HttpMethod", logParam.HttpMethod);
            param.Add("@RequestBody", logParam.RequestBody);
            param.Add("@ExceptionType", logParam.ExceptionType);
            param.Add("@ExceptionMessage", logParam.ExceptionMessage);
            param.Add("@ExceptionStackTrace", logParam.ExceptionStackTrace);
            param.Add("@InnerExceptionMessage", logParam.InnerExceptionMessage);
            param.Add("@InnerExceptionStackTrace", logParam.InnerExceptionStackTrace);
            param.Add("@MachineName", logParam.MachineName);
            param.Add("@Environment", logParam.Environment);

            _ = await connection.ExecuteAsync("[dbo].[usp_log_exception]", param, commandType: CommandType.StoredProcedure);
        }
    }
}
