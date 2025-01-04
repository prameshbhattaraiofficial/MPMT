using AutoMapper;
using Dapper;
using DocumentFormat.OpenXml.Office2019.Drawing.Model3D;
using Mpmt.Core.Domain.Partners.SendTransactions;
using Mpmt.Core.Dtos.Agent;
using Mpmt.Data.Common;
using Mpmts.Core.Dtos;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Data.Repositories.Agents.AgentTxn
{
    public class AgentTransactionRepository : IAgentTransactionRepository
    {
        private readonly IMapper _mapper;

        public AgentTransactionRepository(IMapper mapper)
        {
            _mapper = mapper;
        }
        public async Task<(AgentTxnModel, SprocMessage)> checkControlNumberAsynce(string controlNumber, string agentCode)
        {
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();

                var param = new DynamicParameters();
                param.Add("@AgentCode", agentCode);
                param.Add("@MtcnNumber", controlNumber);
                param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
                param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                var result = await connection.QueryFirstOrDefaultAsync<AgentTxnModel>("[dbo].[usp_get_cash_payout_detail]", param, commandType: CommandType.StoredProcedure);

                var statusCode = param.Get<int>("@StatusCode");
                var msgType = param.Get<string>("@MsgType");
                var msgText = param.Get<string>("@MsgText");

                return (result, new SprocMessage { StatusCode = statusCode, MsgType = msgType, MsgText = msgText });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> GetProcessIdAsync(string agentCode, string referenceId)
        {
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();

                var param = new DynamicParameters();
                param.Add("@VendorId", agentCode);
                param.Add("@ReferenceId", referenceId);
                param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
                param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                var result = await connection.QueryFirstOrDefaultAsync<string>("[dbo].[usp_get_txn_processid]", param, commandType: CommandType.StoredProcedure);

                var statusCode = param.Get<int>("@StatusCode");
                var msgType = param.Get<string>("@MsgType");
                var msgText = param.Get<string>("@MsgText");

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<(AgentPayOutReceipt, SprocMessage)> payoutTransactionByAgentAysnc(AgentTxnModel request)
        {
            //return null;
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();
                var param = new DynamicParameters();
                param.Add("@RemitTransactionId", request.TransactionId);
                param.Add("@PaymentType", request.PaymentType);
                param.Add("@AgentCode", request.AgentCode);
                param.Add("@ContactNumber", request.ReceiverContactNumber);
                param.Add("@DocumentType", request.IDTypeName);
                param.Add("@DocumentTypeCode", request.IdentificationType);
                param.Add("@DocumentNumber", request.IndentificationNumber);
                param.Add("@DocumentIssueDate", request.IssueDateAD);

                param.Add("@DocumentExpiryDate", request.ExpiryDate);
                param.Add("@DocumentIssueDateNepali", request.Id_IssuedDateBS);
                param.Add("@DocumentExpiryDateNepali", request.Id_ExpiryDateBS);
                param.Add("@DocumentFrontImagePath", request.UploadImagePath);
                param.Add("@DocumentBackImagePath", request.UploadBackImagePath);
                param.Add("@PayoutTransactionType", "AGENTWEBTRANSACTION");          
                param.Add("@LoggedInUser", request.AgentCode);
                param.Add("@IpAddress", request.IpAddress);
                param.Add("@DeviceId", request.DeviceId);
                param.Add("@ProcessId", request.processId);
             
                param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
                param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                var addTransactionDetails = await connection
                    .QueryFirstOrDefaultAsync<AgentPayOutReceipt>("[dbo].[usp_agent_cash_payout]", param, commandType: CommandType.StoredProcedure);

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
    }
}
