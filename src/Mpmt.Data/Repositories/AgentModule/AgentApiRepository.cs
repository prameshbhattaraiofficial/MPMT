using Dapper;
using Microsoft.AspNetCore.Http;
using Mpmt.Core.Dtos.Agent;
using Mpmt.Core.Dtos.AgentApi;
using Mpmt.Core.Dtos.PartnerApi;
using Mpmt.Core.Dtos.Roles;
using Mpmt.Data.Common;
using Mpmts.Core.Dtos;
using System.Data;

namespace Mpmt.Data.Repositories.AgentModule
{
    public class AgentApiRepository : IAgentApiRepository
    {
        public async Task<(SprocMessage, PayoutResponse)> AgentWalletValidateLoadAsync(AgentPayoutModel model)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@AgentCode", model.AgentCode);
            param.Add("@Mtcn", model.MTCN);
            param.Add("@AmountNPR", model.Amount);
            param.Add("@SenderName", model.SenderName);
            param.Add("@SenderCountry", model.Country);
            param.Add("@WalletHolderName", model.WalletHolderName);
            param.Add("@IpAddress", model.IPAddress);
            param.Add("@DeviceId", model.DeviceId);

            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            var payoutResponse = await connection.QueryFirstOrDefaultAsync<PayoutResponse>(
                "[dbo].[usp_validate_mtcn_cash_agent]", param, commandType: CommandType.StoredProcedure);

            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return (new SprocMessage { StatusCode = statusCode, MsgType = msgType, MsgText = msgText }, payoutResponse);
        }

        public async Task<(SprocMessage, CashPayoutDetailApi)> GetCashPayoutDetailAsync(GetCashPayoutDetailParam reqParam)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@MtcnNumber", reqParam.MtcnNumber);
            param.Add("@AgentCode", reqParam.AgentCode);
            param.Add("@IpAddress", reqParam.IpAddress);
            param.Add("@DeviceId", reqParam.DeviceId);

            param.Add("@TxnStatusCode", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@TxnStatusRemark", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            using var resultSets = await connection
                .QueryMultipleAsync("[dbo].[usp_get_cash_payout_detail_Api]", param, commandType: CommandType.StoredProcedure);

            //using var resultSets = await connection
            //    .QueryMultipleAsync("[dbo].[usp_get_instrument_list_for_cash_wallet_agent]", param, commandType: CommandType.StoredProcedure);


            var documentTypes = resultSets.IsConsumed
                ? Enumerable.Empty<DocumentTypeApi>()
                : (await resultSets.ReadAsync<DocumentTypeApi>()).Where(x => !string.IsNullOrWhiteSpace(x.DocumentTypeCode));


            var result = resultSets.IsConsumed ? null : await resultSets.ReadSingleOrDefaultAsync<CashPayoutDetailApi>();

            var txnStatusCode = param.Get<string>("@TxnStatusCode");
            var txnStatusRemarks = param.Get<string>("@TxnStatusRemark");

            result = result is null ? new CashPayoutDetailApi { } : result;
            result.PaymentStatusCode = txnStatusCode;
            result.PaymentStatusRemarks = txnStatusRemarks;
            result.DocumentTypeList = documentTypes;
            //result.SenderCountry = countryItems;

            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            var sprocMsg = new SprocMessage { StatusCode = statusCode, MsgType = msgType, MsgText = msgText };

            return (sprocMsg, result);
        }

        public async Task<(SprocMessage, TxnProcessId)> GetTxnProcessIdAsync(string vendorId, string referenceId)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@VendorId", vendorId);
            param.Add("@ReferenceId", referenceId);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            var processId = await connection.QueryFirstOrDefaultAsync<TxnProcessId>(
                "[dbo].[usp_get_txn_processid]", param: param, commandType: CommandType.StoredProcedure);

            var sprocMessage = new SprocMessage
            {
                StatusCode = param.Get<int>("@StatusCode"),
                MsgType = param.Get<string>("@MsgType"),
                MsgText = param.Get<string>("@MsgText")
            };

            return (sprocMessage, processId);
        }

        public async Task<(SprocMessage, PayoutDetailsApi)> RequestPayoutAsync(RequestPayoutParam reqParam)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@RemitTransactionId", reqParam.RemitTransactionId);
            param.Add("@ProcessId", reqParam.ProcessId);
            param.Add("@AgentTransactionId", reqParam.AgentTransactionId);
            param.Add("@PayoutTransactionType", reqParam.PayoutTransactionType);
            param.Add("@PaymentType", reqParam.PaymentType);
            param.Add("@AgentCode", reqParam.AgentCode);
            param.Add("@ContactNumber", reqParam.ContactNumber);
            param.Add("@DocumentTypeCode", reqParam.DocumentTypeCode);
            param.Add("@DocumentNumber", reqParam.DocumentNumber);
            param.Add("@DocumentIssueDate", reqParam.DocumentIssueDate);
            param.Add("@DocumentExpiryDate", reqParam.DocumentExpiryDate);
            param.Add("@DocumentIssueDateNepali", reqParam.DocumentIssueDateNepali);
            param.Add("@DocumentExpiryDateNepali", reqParam.DocumentExpiryDateNepali);
            param.Add("@DocumentFrontImagePath", reqParam.DocumentFrontImagePath);
            param.Add("@DocumentBackImagePath", reqParam.DocumentBackImagePath);
            param.Add("@Username", reqParam.Username);
            param.Add("@UserType", reqParam.UserType);
            param.Add("@LoggedInUser", reqParam.LoggedInUser);
            param.Add("@IpAddress", reqParam.IpAddress);
            param.Add("@DeviceId", reqParam.DeviceId);

            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            var payoutResponse = await connection
                .QueryFirstOrDefaultAsync<PayoutDetailsApi>("[dbo].[usp_agent_cash_payout]", param, commandType: CommandType.StoredProcedure);

            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return (new SprocMessage { StatusCode = statusCode, MsgType = msgType, MsgText = msgText }, payoutResponse);
        }

        public async Task<(SprocMessage, CheckPayoutStatusDetail)> CheckPayoutStatusAsync(string remitTransactionId, string agentCode)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@RemitTransactionId", remitTransactionId);
            param.Add("@AgentCode", agentCode);
            param.Add("@TxnStatusCode", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@TxnStatusRemark", dbType: DbType.String, size: 300, direction: ParameterDirection.Output);

            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            var statusDetails = await connection
                .QuerySingleOrDefaultAsync<CheckPayoutStatusDetail>("[dbo].[usp_chk_cash_payout_status]", param, commandType: CommandType.StoredProcedure);

            var txnStatusCode = param.Get<string>("@TxnStatusCode");
            var txnStatusRemarks = param.Get<string>("@TxnStatusRemark");

            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            statusDetails ??= new CheckPayoutStatusDetail();
            statusDetails.PaymentStatusCode = txnStatusCode;
            statusDetails.PaymentStatusRemarks = txnStatusRemarks;

            return (new SprocMessage { StatusCode = statusCode, MsgType = msgType, MsgText = msgText }, statusDetails);
        }

        public async Task<(SprocMessage,AgentWalletApi)> GetInstrumentDetailForAgentWalletAsync(GetCashPayoutDetailParam reqParam)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@MtcnNumber", reqParam.MtcnNumber);
            param.Add("@AgentCode", reqParam.AgentCode);
            param.Add("@IpAddress", reqParam.IpAddress);
            param.Add("@DeviceId", reqParam.DeviceId);

            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
            param.Add("@TxnStatusCode", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
            param.Add("@TxnStatusRemark", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
            

            var resultSets = await connection
                .QueryMultipleAsync("[dbo].[usp_get_instrument_list_for_cash_wallet_agent]", param: param, commandType: CommandType.StoredProcedure);

            var countryList = await resultSets.ReadAsync<CountryItem>();
            var senderDocumentTypeList = await resultSets.ReadAsync<DocumentTypeItem>();

            var sprocMessage = new SprocMessage
            {
                StatusCode = param.Get<int>("@StatusCode"),
                MsgType = param.Get<string>("@MsgType"),
                MsgText = param.Get<string>("@MsgText")
            };
            var result = new AgentWalletApi
            {              
                CountryList = countryList,
                SenderDocumentTypeList = senderDocumentTypeList,       
                TransactionStatusCode = param.Get<string>("@TxnStatusCode"),
                TransactionRemarks= param.Get<string>("@TxnStatusRemark")
            };
   
            return (sprocMessage, result);
        }
    }
}
