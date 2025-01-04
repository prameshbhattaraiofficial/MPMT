using Dapper;
using Mpmt.Core.Domain.Public.Feedbacks;
using Mpmt.Data.Common;
using Mpmts.Core.Dtos;
using System.Data;

namespace Mpmt.Data.Repositories.Public
{
    public class FeedbackRepository : IFeedbackRepository
    {
        public async Task<SprocMessage> InsertPublicFeedbackAsync(PublicFeedback publicFeedback)
        {
            const string operationMode = "I";
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@Operation", operationMode);
            param.Add("@FullName", publicFeedback.FullName);
            param.Add("@Email", publicFeedback.Email);
            param.Add("@ContactNo", publicFeedback.ContactNo);
            param.Add("@Subject", publicFeedback.Subject);
            param.Add("@Message", publicFeedback.Message);
            param.Add("@IpAddress", publicFeedback.IpAddress);
            param.Add("@IsReviewed", publicFeedback.IsReviewed);
            param.Add("@UpdatedBy", publicFeedback.UpdatedBy);

            param.Add("@ReturnPrimaryId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[usp_iud_remit_public_feedbacks]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@ReturnPrimaryId");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }
    }
}
