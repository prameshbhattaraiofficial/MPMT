using Dapper;
using Mpmt.Core.Dtos.Payout;
using Mpmt.Data.Common;
using Mpmts.Core.Dtos;
using System.Data;

namespace Mpmt.Data.Repositories.Payout
{
    public class PayoutRepository : IPayoutRepository
    {
        public async Task<(SprocMessage, PayoutReferenceInfo)> GetPayoutReferenceInfoAsync(GetPayoutReferenceInfoDto dto)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@RemitTransactionId", dto.RemitTransactionId);
            param.Add("@PaymentType", dto.PaymentType);
            param.Add("@AgentCode", dto.AgentCode);
            param.Add("@IpAddress", dto.IpAddress);
            param.Add("@DeviceId", dto.DeviceId);
            param.Add("@LoggedInUser", dto.LoggedInUser);
            param.Add("@UserType", dto.UserType);
            param.Add("@ReturnPrimaryId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            var payoutRefInfo = await connection
                .QueryFirstOrDefaultAsync<PayoutReferenceInfo>("[dbo].[usp_get_payout_referenceNo]", param, commandType: CommandType.StoredProcedure);

            var status = new SprocMessage
            {
                IdentityVal = param.Get<int>("@ReturnPrimaryId"),
                StatusCode = param.Get<int>("@StatusCode"),
                MsgType = param.Get<string>("@MsgType"),
                MsgText = param.Get<string>("@MsgText")
            };

            return (status, payoutRefInfo);
        }
    }
}
