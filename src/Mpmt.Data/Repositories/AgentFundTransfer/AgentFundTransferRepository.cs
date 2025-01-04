using AutoMapper;
using Dapper;
using Mpmt.Core.Dtos.Agent;
using Mpmt.Core.Dtos.AgentFundTransfer;
using Mpmt.Data.Common;
using Mpmts.Core.Dtos;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Data.Repositories.AgentFundTransfer
{
    public class AgentFundTransferRepository : IAgentFundTransferRepository
    {
        private readonly IMapper _mapper;

        public AgentFundTransferRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<(GetAgentFundRequestList, SprocMessage)> AgentFundRequest(AgentFundTransferDto model)
        {
            //return null;
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();
                var param = new DynamicParameters();
                param.Add("@AgentCode", model.AgentCode);
                //param.Add("@FundType", model.FundType);
                param.Add("@TotalAmount", model.totalAmount);
                param.Add("@Remarks", model.Remarks);
                param.Add("@OperationMode", "A");
                param.Add("@LoggedInUser", model.LoggedInUser);
                param.Add("@UserType", model.UserType);
                param.Add("@CommissionAmount", model.CommissionAmount);
                param.Add("@TxnCashAmount", model.ReceivableAmount);
                param.Add("@IsTxnCashRequested", model.isReceivable);
                param.Add("@IsCommissionRequested", model.isCommission);

                param.Add("@ReturnPrimaryId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                // param.Add("@SuperEmail", dbType: DbType.String, direction: ParameterDirection.Output);
                param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
                param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                var addTransactionDetails = await connection
                    .QueryFirstOrDefaultAsync<GetAgentFundRequestList>("[dbo].[usp_agent_fund_request_addupdate]", param, commandType: CommandType.StoredProcedure);

                var addTxnStatus = new SprocMessage
                {
                    StatusCode = param.Get<int>("@StatusCode"),
                    MsgType = param.Get<string>("@MsgType"),
                    MsgText = param.Get<string>("@MsgText")
                };

                return (addTransactionDetails, addTxnStatus);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<SprocMessage> FundRequest(AgentFundTransferDto model)
        {

            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@AgentCode", model.AgentCode);
            param.Add("@FundType", model.FundType);
            param.Add("@Amount", model.TotalAmount);
            param.Add("@Remarks", model.Remarks);
            param.Add("@OperationMode", "A");
            param.Add("@LoggedInUser", model.LoggedInUser);
            param.Add("@UserType", model.UserType);
           
            param.Add("@ReturnPrimaryId", dbType: DbType.Int32, direction: ParameterDirection.Output);
           // param.Add("@SuperEmail", dbType: DbType.String, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            try
            {
                _ = await connection.ExecuteAsync("[dbo].[usp_agent_fund_request_addupdate]", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {

            }

            var identityVal = param.Get<int>("@ReturnPrimaryId");
            //var SuperEmail = param.Get<string>("@SuperEmail");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };

        }

        public async Task<AgentFundTransferDto> GetFundTransferDetailAsync(string agentCode)
        {
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();
                var param = new DynamicParameters();
                param.Add("@AgentCode", agentCode);            
                var result = await connection.QueryFirstOrDefaultAsync<AgentFundTransferDto>("[dbo].[usp_get_agent_balance_summary]", param, commandType: CommandType.StoredProcedure);           
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
