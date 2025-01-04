using Dapper;
using Mpmt.Core.Dtos.Logging;
using Mpmt.Data.Common;
using System.Data;

namespace Mpmt.Data.Repositories.Logging
{
    public class AgentApiLogRepository : IAgentApiLogRepository
    {
        public async Task LogInsertAsync(VendorApiLogParam log)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@LogId", log.LogId);
            param.Add("@TransactionId", log.TransactionId);
            param.Add("@TrackerId", log.TrackerId);
            param.Add("@RequestInput", log.RequestInput);
            param.Add("@ResponseOutput", log.ResponseOutput);
            param.Add("@ResponseHttpStatus", log.ResponseHttpStatus);
            param.Add("@RequestUrl", log.RequestUrl);
            param.Add("@RequestHeaders", log.RequestHeaders);
            //param.Add("@RequestToken", log.RequestToken);
            param.Add("@VendorRequestInput", log.VendorRequestInput);
            param.Add("@VendorResponseOutput", log.VendorResponseOutput);
            param.Add("@VendorRequestURL", log.VendorRequestURL);
            param.Add("@VendorRequestHeaders", log.VendorRequestHeaders);
            param.Add("@VendorResponseHttpStatus", log.VendorResponseHttpStatus);
            param.Add("@VendorResponseStatus", log.VendorResponseStatus);
            param.Add("@VendorResponseState", log.VendorResponseState);
            param.Add("@VendorResponseMessage", log.VendorResponseMessage);
            param.Add("@VendorTransactionId", log.VendorTransactionId);
            param.Add("@VendorTrackerId", log.VendorTrackerId);
            param.Add("@VendorException", log.VendorException);
            param.Add("@VendorExceptionStackTrace", log.VendorExceptionStackTrace);
            param.Add("@VendorId", log.VendorId);
            param.Add("@VendorType", log.VendorType);
            param.Add("@VendorRequestInput2", log.VendorRequestInput2);
            param.Add("@VendorResponseOutput2", log.VendorResponseOutput2);
            param.Add("@VendorRequestURL2", log.VendorRequestURL2);
            param.Add("@VendorRequestHeaders2", log.VendorRequestHeaders2);
            param.Add("@VendorResponseHttpStatus2", log.VendorResponseHttpStatus2);
            param.Add("@VendorResponseStatus2", log.VendorResponseStatus2);
            param.Add("@VendorResponseState2", log.VendorResponseState2);
            param.Add("@VendorResponseMessage2", log.VendorResponseMessage2);
            param.Add("@VendorTransactionId2", log.VendorTransactionId2);
            param.Add("@VendorTrackerId2", log.VendorTrackerId2);
            param.Add("@VendorException2", log.VendorException2);
            param.Add("@VendorExceptionStackTrace2", log.VendorExceptionStackTrace2);
            param.Add("@VendorId2", log.VendorId2);
            param.Add("@VendorType2", log.VendorType2);
            param.Add("@VendorRequestInput3", log.VendorRequestInput3);
            param.Add("@VendorResponseOutput3", log.VendorResponseOutput3);
            param.Add("@VendorRequestURL3", log.VendorRequestURL3);
            param.Add("@VendorRequestHeaders3", log.VendorRequestHeaders3);
            param.Add("@VendorResponseHttpStatus3", log.VendorResponseHttpStatus3);
            param.Add("@VendorResponseStatus3", log.VendorResponseStatus3);
            param.Add("@VendorResponseState3", log.VendorResponseState3);
            param.Add("@VendorResponseMessage3", log.VendorResponseMessage3);
            param.Add("@VendorTransactionId3", log.VendorTransactionId3);
            param.Add("@VendorTrackerId3", log.VendorTrackerId3);
            param.Add("@VendorException3", log.VendorException3);
            param.Add("@VendorExceptionStackTrace3", log.VendorExceptionStackTrace3);
            param.Add("@VendorId3", log.VendorId3);
            param.Add("@VendorType3", log.VendorType3);
            param.Add("@ClientCode", log.ClientCode);
            param.Add("@PartnerCode", log.PartnerCode);
            param.Add("@AgentCode", log.AgentCode);
            param.Add("@MemberId", log.MemberId);
            param.Add("@MemberUserName", log.MemberUserName);
            param.Add("@MemberName", log.MemberName);
            param.Add("@DeviceCode", log.DeviceCode);
            param.Add("@IpAddress", log.IpAddress);
            param.Add("@Platform", log.Platform);
            param.Add("@MachineName", log.MachineName);
            param.Add("@Environment", log.Environment);

            await connection.ExecuteAsync("[dbo].[usp_insert_agent_api_log]", param, commandType: CommandType.StoredProcedure);
        }

        public async Task LogUpdateAsync(VendorApiLogParam log)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@LogId", log.LogId);
            param.Add("@TransactionId", log.TransactionId);
            param.Add("@TrackerId", log.TrackerId);
            param.Add("@RequestInput", log.RequestInput);
            param.Add("@ResponseOutput", log.ResponseOutput);
            param.Add("@ResponseHttpStatus", log.ResponseHttpStatus);
            param.Add("@RequestUrl", log.RequestUrl);
            param.Add("@RequestHeaders", log.RequestHeaders);
            param.Add("@VendorRequestInput", log.VendorRequestInput);
            param.Add("@VendorResponseOutput", log.VendorResponseOutput);
            param.Add("@VendorRequestURL", log.VendorRequestURL);
            param.Add("@VendorRequestHeaders", log.VendorRequestHeaders);
            param.Add("@VendorResponseHttpStatus", log.VendorResponseHttpStatus);
            param.Add("@VendorResponseStatus", log.VendorResponseStatus);
            param.Add("@VendorResponseState", log.VendorResponseState);
            param.Add("@VendorResponseMessage", log.VendorResponseMessage);
            param.Add("@VendorTransactionId", log.VendorTransactionId);
            param.Add("@VendorTrackerId", log.VendorTrackerId);
            param.Add("@VendorException", log.VendorException);
            param.Add("@VendorExceptionStackTrace", log.VendorExceptionStackTrace);
            param.Add("@VendorId", log.VendorId);
            param.Add("@VendorType", log.VendorType);
            param.Add("@VendorRequestInput2", log.VendorRequestInput2);
            param.Add("@VendorResponseOutput2", log.VendorResponseOutput2);
            param.Add("@VendorRequestURL2", log.VendorRequestURL2);
            param.Add("@VendorRequestHeaders2", log.VendorRequestHeaders2);
            param.Add("@VendorResponseHttpStatus2", log.VendorResponseHttpStatus2);
            param.Add("@VendorResponseStatus2", log.VendorResponseStatus2);
            param.Add("@VendorResponseState2", log.VendorResponseState2);
            param.Add("@VendorResponseMessage2", log.VendorResponseMessage2);
            param.Add("@VendorTransactionId2", log.VendorTransactionId2);
            param.Add("@VendorTrackerId2", log.VendorTrackerId2);
            param.Add("@VendorException2", log.VendorException2);
            param.Add("@VendorExceptionStackTrace2", log.VendorExceptionStackTrace2);
            param.Add("@VendorId2", log.VendorId2);
            param.Add("@VendorType2", log.VendorType2);
            param.Add("@VendorRequestInput3", log.VendorRequestInput3);
            param.Add("@VendorResponseOutput3", log.VendorResponseOutput3);
            param.Add("@VendorRequestURL3", log.VendorRequestURL3);
            param.Add("@VendorRequestHeaders3", log.VendorRequestHeaders3);
            param.Add("@VendorResponseHttpStatus3", log.VendorResponseHttpStatus3);
            param.Add("@VendorResponseStatus3", log.VendorResponseStatus3);
            param.Add("@VendorResponseState3", log.VendorResponseState3);
            param.Add("@VendorResponseMessage3", log.VendorResponseMessage3);
            param.Add("@VendorTransactionId3", log.VendorTransactionId3);
            param.Add("@VendorTrackerId3", log.VendorTrackerId3);
            param.Add("@VendorException3", log.VendorException3);
            param.Add("@VendorExceptionStackTrace3", log.VendorExceptionStackTrace3);
            param.Add("@VendorId3", log.VendorId3);
            param.Add("@VendorType3", log.VendorType3);
            param.Add("@ClientCode", log.ClientCode);
            param.Add("@PartnerCode", log.PartnerCode);
            param.Add("@AgentCode", log.AgentCode);
            param.Add("@MemberId", log.MemberId);
            param.Add("@MemberUserName", log.MemberUserName);
            param.Add("@MemberName", log.MemberName);
            param.Add("@DeviceCode", log.DeviceCode);
            param.Add("@IpAddress", log.IpAddress);
            param.Add("@Platform", log.Platform);
            param.Add("@MachineName", log.MachineName);
            param.Add("@Environment", log.Environment);

            await connection.ExecuteAsync("[dbo].[usp_update_agent_api_log]", param, commandType: CommandType.StoredProcedure);
        }

        public async Task LogUpdateResponseAsync(VendorApiLogParam log)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@LogId", log.LogId);
            param.Add("@TransactionId", log.TransactionId);
            param.Add("@TrackerId", log.TrackerId);
            param.Add("@ResponseOutput", log.ResponseOutput);
            param.Add("@ResponseHttpStatus", log.ResponseHttpStatus);
            param.Add("@VendorTransactionId", log.VendorTransactionId);
            param.Add("@VendorTrackerId", log.VendorTrackerId);
            param.Add("@VendorId", log.VendorId);
            param.Add("@VendorType", log.VendorType);
            param.Add("@VendorTransactionId2", log.VendorTransactionId2);
            param.Add("@VendorTrackerId2", log.VendorTrackerId2);
            param.Add("@VendorId2", log.VendorId2);
            param.Add("@VendorType2", log.VendorType2);
            param.Add("@VendorTransactionId3", log.VendorTransactionId3);
            param.Add("@VendorTrackerId3", log.VendorTrackerId3);
            param.Add("@VendorId3", log.VendorId3);
            param.Add("@VendorType3", log.VendorType3);

            await connection.ExecuteAsync("[dbo].[usp_update_api_response_log]", param, commandType: CommandType.StoredProcedure);
        }

        public async Task LogUpdateVendorApiExceptionAsync(VendorApiLogParam log)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@LogId", log.LogId);
            param.Add("@VendorException", log.VendorException);
            param.Add("@VendorExceptionStackTrace", log.VendorExceptionStackTrace);

            await connection.ExecuteAsync("[dbo].[usp_update_agent_api_exception_log]", param, commandType: CommandType.StoredProcedure);
        }

        public async Task LogUpdateVendorApiException2Async(VendorApiLogParam log)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@LogId", log.LogId);
            param.Add("@VendorException2", log.VendorException2);
            param.Add("@VendorExceptionStackTrace2", log.VendorExceptionStackTrace2);

            await connection.ExecuteAsync("[dbo].[usp_update_agent_api_exception_log2]", param, commandType: CommandType.StoredProcedure);
        }

        public async Task LogUpdateVendorApiException3Async(VendorApiLogParam log)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@LogId", log.LogId);
            param.Add("@VendorException3", log.VendorException3);
            param.Add("@VendorExceptionStackTrace3", log.VendorExceptionStackTrace3);

            await connection.ExecuteAsync("[dbo].[usp_update_agent_api_exception_log3]", param, commandType: CommandType.StoredProcedure);
        }

        public async Task LogUpdateVendorApiResponseAsync(VendorApiLogParam log)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@LogId", log.LogId);
            param.Add("@VendorRequestInput", log.VendorRequestInput);
            param.Add("@VendorResponseOutput", log.VendorResponseOutput);
            param.Add("@VendorRequestURL", log.VendorRequestURL);
            param.Add("@VendorRequestHeaders", log.VendorRequestHeaders);
            param.Add("@VendorResponseHttpStatus", log.VendorResponseHttpStatus);
            param.Add("@VendorResponseStatus", log.VendorResponseStatus);
            param.Add("@VendorResponseState", log.VendorResponseState);
            param.Add("@VendorResponseMessage", log.VendorResponseMessage);
            param.Add("@VendorTransactionId", log.VendorTransactionId);
            param.Add("@VendorTrackerId", log.VendorTrackerId);
            param.Add("@VendorException", log.VendorException);
            param.Add("@VendorExceptionStackTrace", log.VendorExceptionStackTrace);
            param.Add("@VendorId", log.VendorId);
            param.Add("@VendorType", log.VendorType);

            await connection.ExecuteAsync("[dbo].[usp_update_agent_api_response_log]", param, commandType: CommandType.StoredProcedure);
        }

        public async Task LogUpdateVendorApiResponse2Async(VendorApiLogParam log)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@LogId", log.LogId);
            param.Add("@VendorRequestInput2", log.VendorRequestInput2);
            param.Add("@VendorResponseOutput2", log.VendorResponseOutput2);
            param.Add("@VendorRequestURL2", log.VendorRequestURL2);
            param.Add("@VendorRequestHeaders2", log.VendorRequestHeaders2);
            param.Add("@VendorResponseHttpStatus2", log.VendorResponseHttpStatus2);
            param.Add("@VendorResponseStatus2", log.VendorResponseStatus2);
            param.Add("@VendorResponseState2", log.VendorResponseState2);
            param.Add("@VendorResponseMessage2", log.VendorResponseMessage2);
            param.Add("@VendorTransactionId2", log.VendorTransactionId2);
            param.Add("@VendorTrackerId2", log.VendorTrackerId2);
            param.Add("@VendorException2", log.VendorException2);
            param.Add("@VendorExceptionStackTrace2", log.VendorExceptionStackTrace2);
            param.Add("@VendorId2", log.VendorId2);
            param.Add("@VendorType2", log.VendorType2);

            await connection.ExecuteAsync("[dbo].[usp_update_agent_api_response_log2]", param, commandType: CommandType.StoredProcedure);
        }

        public async Task LogUpdateVendorApiResponse3Async(VendorApiLogParam log)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@LogId", log.LogId);
            param.Add("@VendorRequestInput3", log.VendorRequestInput3);
            param.Add("@VendorResponseOutput3", log.VendorResponseOutput3);
            param.Add("@VendorRequestURL3", log.VendorRequestURL3);
            param.Add("@VendorRequestHeaders3", log.VendorRequestHeaders3);
            param.Add("@VendorResponseHttpStatus3", log.VendorResponseHttpStatus3);
            param.Add("@VendorResponseStatus3", log.VendorResponseStatus3);
            param.Add("@VendorResponseState3", log.VendorResponseState3);
            param.Add("@VendorResponseMessage3", log.VendorResponseMessage3);
            param.Add("@VendorTransactionId3", log.VendorTransactionId3);
            param.Add("@VendorTrackerId3", log.VendorTrackerId3);
            param.Add("@VendorException3", log.VendorException3);
            param.Add("@VendorExceptionStackTrace3", log.VendorExceptionStackTrace3);
            param.Add("@VendorId3", log.VendorId3);
            param.Add("@VendorType3", log.VendorType3);

            await connection.ExecuteAsync("[dbo].[usp_update_agent_api_response_log3]", param, commandType: CommandType.StoredProcedure);
        }

    }
}
