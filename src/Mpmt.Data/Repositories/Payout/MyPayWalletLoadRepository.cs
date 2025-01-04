using Dapper;
using Mpmt.Core.Dtos.WalletLoad.MyPay;
using Mpmt.Data.Common;
using Mpmts.Core.Dtos;
using System.Data;

namespace Mpmt.Data.Repositories.Payout
{
    public class MyPayWalletLoadRepository : IMyPayWalletLoadRepository
    {

        public async Task<SprocMessage> LogRemitToWalletPayoutAsync(MyPayWalletPayoutLogParam log)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@PayoutRefereneNo", log.PayoutRefereneNo);
            param.Add("@AgentTransactionId", log.AgentTransactionId);
            param.Add("@Message", log.Message);
            param.Add("@ResponseMessage", log.ResponseMessage);
            param.Add("@Details", log.Details);
            param.Add("@ResponseCode", log.ResponseCode);
            param.Add("@Status", log.Status);
            param.Add("@UserType", log.UserType);
            param.Add("@LoggedInUser", log.LoggedInUser);

            param.Add("@ReturnPrimaryId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[usp_remit_payout_wallet_log]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@ReturnPrimaryId");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { StatusCode = statusCode, MsgType = msgType, MsgText = msgText, IdentityVal = identityVal };
        }

        public async Task<SprocMessage> LogWalletPayoutTxnStatusAsync(MyPayWalletLoadPayoutTxnStatusLogParam log)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@PayoutRefereneNo", log.PayoutRefereneNo);
            param.Add("@TransactionStatus", log.TransactionStatus);
            param.Add("@AgentStatusCode", log.AgentStatusCode);
            param.Add("@Message", log.Message);
            param.Add("@ResponseMessage", log.ResponseMessage);
            param.Add("@Details", log.Details);
            param.Add("@ResponseCode", log.ResponseCode);
            param.Add("@Status", log.Status);
            param.Add("@UserType", log.UserType);
            param.Add("@LoggedInUser", log.LoggedInUser);

            param.Add("@ReturnPrimaryId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[usp_remit_wallet_status_log]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@ReturnPrimaryId");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { StatusCode = statusCode, MsgType = msgType, MsgText = msgText, IdentityVal = identityVal };
        }

        public async Task<SprocMessage> LogWalletUserValidationAsync(MyPayValidateWalletUserLogParam log)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@RemitTransactionId", log.RemitTransactionId);
            param.Add("@AgentCode", log.AgentCode);
            param.Add("@AccountStatus", log.AccountStatus);
            param.Add("@ContactNumber", log.ContactNumber);
            param.Add("@IsAccountValidated", log.IsAccountValidated);
            param.Add("@Message", log.Message);
            param.Add("@ResponseMessage", log.ResponseMessage);
            param.Add("@ResponseCode", log.ResponseCode);
            param.Add("@Status", log.Status);
            param.Add("@LoggedInUser", log.LoggedInUser);
            param.Add("@UserType", log.UserType);

            param.Add("@ReturnPrimaryId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[usp_validate_wallet_user_log]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@ReturnPrimaryId");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { StatusCode = statusCode, MsgType = msgType, MsgText = msgText, IdentityVal = identityVal };
        }
    }
}
