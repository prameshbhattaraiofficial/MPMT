using Dapper;
using DocumentFormat.OpenXml.Office2016.Excel;
using Mpmt.Core.Domain.Partners.SendTransactions;
using Mpmt.Core.Dtos.PartnerReport;
using Mpmt.Core.Dtos.Transaction;
using Mpmt.Data.Common;
using Mpmts.Core.Dtos;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Data;

namespace Mpmt.Data.Repositories.Partner
{
    public class PartnerSendTxnsRepository : IPartnerSendTxnsRepository
    {
        public async Task<(SprocMessage, AddTransactionDetailsDto)> AddTransactionAsync(AddTransactionDto request)
        {
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();

                var param = new DynamicParameters();
                param.Add("@PartnerCode", request.PartnerCode);
                param.Add("@ProcessId", request.ProcessId);
                param.Add("@SourceCurrency", request.SourceCurrency);
                param.Add("@DestinationCurrency", request.DestinationCurrency);
                param.Add("@SendingAmount", request.SendingAmount);
                param.Add("@PaymentType", request.PaymentType);
                //param.Add("@ServiceCharge", request.ServiceCharge);
                //param.Add("@NetSendingAmount", request.NetSendingAmount);
                //param.Add("@ConversionRate", request.ConversionRate);
                //param.Add("@NetRecievingAmountNPR", request.NetRecievingAmountNPR);
                //param.Add("@PartnerServiceCharge", request.PartnerServiceCharge);
                param.Add("@TransactionType", request.TransactionType);
                param.Add("@ExistingSender", request.ExistingSender);
                param.Add("@MemberId", request.MemberId);
                param.Add("@SenderFirstName", request.SenderFirstName);
                param.Add("@NoSenderFirstName", request.NoSenderFirstName);
                param.Add("@SenderLastName", request.SenderLastName);
                param.Add("@SenderContactNumber", request.SenderContactNumber);
                param.Add("@SenderEmail", request.SenderEmail);
                param.Add("@SenderCountry", request.SenderCountryCode);
                param.Add("@SenderProvince", request.SenderProvince);
                param.Add("@SenderCity", request.SenderCity);
                param.Add("@SenderZipcode", request.SenderZipcode);
                param.Add("@SenderAddress", request.SenderAddress);
                param.Add("@SenderRelationshipWithRecipient", request.SenderRelationshipId);
                param.Add("@SenderDocumentType", request.DocumentType);
                param.Add("@SenderDocumentNumber", request.DocumentNumber);
                param.Add("@SenderPurposeOfRemittance", request.SenderPurposeId);
                param.Add("@SenderRemarks", request.SenderRemarks);
                param.Add("@SenderOccupation", request.SenderOccupation);
                param.Add("@SenderSourceOfIncome", request.SourceOfIncome);
                param.Add("@ExistingRecipient", request.ExistingRecipient);
                param.Add("@RecipientId", request.RecipientId);
                param.Add("@RecipientType", request.RecipientType);
                param.Add("@RecipientFirstName", request.RecipientFirstName); 
                param.Add("@NoRecipientFirstName", request.NoRecipientFirstName);
                param.Add("@RecipientLastName", request.RecipientLastName);
                param.Add("@JointAccountFirstName", request.JointAccountFirstName);
                param.Add("@NoJointAccountFirstName", request.NoJointAccountFirstName);
                param.Add("@JointAccountLastName", request.JointAccountLastName);
                param.Add("@BusinessName", request.BusinessName);
                param.Add("@RecipientContactNumber", request.RecipientContactNumber);
                param.Add("@RecipientEmail", request.RecipientEmail);
                param.Add("@RecipientDateOfBirth", request.RecipientDateOfBirth);
                param.Add("@RecipientCountry", request.RecipientCountryCode);
                param.Add("@RecipientProvince", request.RecipientProvinceCode);
                param.Add("@RecipientDistrict", request.RecipientDistrictCode);
                param.Add("@RecipientLocalBody", request.RecipientLocalBodyCode);
                param.Add("@RecipientCity", request.RecipientCity);
                param.Add("@RecipientZipcode", request.RecipientZipcode);
                param.Add("@RecipientAddress", request.RecipientAddress);
                param.Add("@RecipientRelationshipWithSender", request.RecipientRelationshipId);
                param.Add("@BankCode", request.BankCode);
                param.Add("@Branch", request.Branch);
                param.Add("@AccountHolderName", request.AccountHolderName);
                param.Add("@AccountNumber", request.AccountNumber);
                param.Add("@WalletCode", request.WalletCode);
                param.Add("@WalletNumber", request.WalletNumber);
                param.Add("@WalletHolderName", request.WalletHolderName);
                param.Add("@IpAddress", request.IpAddress);
                param.Add("@DeviceId", request.DeviceId);
                param.Add("@LoggedInUser", request.LoggedInUser);
                param.Add("@UserType", request.UserType);
                param.Add("@TransactionId", request.TransactionId);

                param.Add("@ReturnPrimaryId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
                param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                var addTransactionDetails = await connection
                    .QueryFirstOrDefaultAsync<AddTransactionDetailsDto>("[dbo].[usp_push_transaction_details]", param, commandType: CommandType.StoredProcedure);

                var addTxnStatus = new SprocMessage
                {
                    StatusCode = param.Get<int>("@StatusCode"),
                    MsgType = param.Get<string>("@MsgType"),
                    MsgText = param.Get<string>("@MsgText"),
                    IdentityVal = param.Get<int>("@ReturnPrimaryId")
                };

                return (addTxnStatus, addTransactionDetails);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<SprocMessage> cancelledTransactionAsysnc(RemitTxnReport model)
        {
            //using var connection = DbConnectionManager.GetDefaultConnection();
            //var param = new DynamicParameters();
            //param.Add("@RemitTransactionId", model.TransactionId);
            //param.Add("@CancelledReason", model.Remarks);
            //param.Add("@LoggedInUser", model.CancelledBy);
            //param.Add("@UserType", model.CancelledUserType);

            //param.Add("@ReturnPrimaryId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            //param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            //param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            //param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            //var addTransactionDetails = await connection
            //                   .QueryFirstOrDefaultAsync("[dbo].[usp_cancel_remit_transaction]", param, commandType: CommandType.StoredProcedure);
            //return addTransactionDetails;
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@RemitTransactionId", model.TransactionId);
            param.Add("@CancelledReason", model.Remarks);
            param.Add("@LoggedInUser", model.CancelledBy);
            param.Add("@UserType", model.CancelledUserType);

            param.Add("@ReturnPrimaryId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[usp_cancel_remit_transaction]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@ReturnPrimaryId");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }

        public async Task<(SprocMessage, TxnProcessIdDto)> GetProcessIdAsync(string vendorId, GetProcessId request)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@VendorId", vendorId);
            param.Add("@ReferenceId", request.ReferenceId);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            var processId = await connection.QueryFirstOrDefaultAsync<TxnProcessIdDto>(
                "[dbo].[usp_get_txn_processid]", param: param, commandType: CommandType.StoredProcedure);

            var sprocMessage = new SprocMessage
            {
                StatusCode = param.Get<int>("@StatusCode"),
                MsgType = param.Get<string>("@MsgType"),
                MsgText = param.Get<string>("@MsgText")
            };

            return (sprocMessage, processId);
        }

        public async Task<(SprocMessage, SendTxnChargeAmountDetailsDto)> GetSendNepaliToOtherChargeAmountDetailsAsync(GetSendTxnChargeAmountDetailsRequest request)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@PartnerCode", request.PartnerCode);
            param.Add("@SourceCurrency", request.DestinationCurrency);
            param.Add("@DestinationCurrency", request.SourceCurrency);
            param.Add("@SendingAmount", decimal.Parse(request.SourceAmount));
            param.Add("@PaymentType", request.PaymentType);

            param.Add("@ReturnPrimaryId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            var chargeGetails = await connection.QueryFirstOrDefaultAsync<SendTxnChargeAmountDetailsDto>(
                "[dbo].[usp_get_conversion_charge_details]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@ReturnPrimaryId");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return (new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText }, chargeGetails);
        }

        public async Task<(SprocMessage, SendTxnChargeAmountDetailsDto)> GetSendTxnChargeAmountDetailsAsync(GetSendTxnChargeAmountDetailsRequest request)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@PartnerCode", request.PartnerCode);
            param.Add("@SourceCurrency", request.SourceCurrency);
            param.Add("@DestinationCurrency", request.DestinationCurrency);
            param.Add("@SendingAmount", decimal.Parse(request.SourceAmount));
            param.Add("@PaymentType", request.PaymentType);

            param.Add("@ReturnPrimaryId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            var chargeGetails = await connection.QueryFirstOrDefaultAsync<SendTxnChargeAmountDetailsDto>(
                "[dbo].[usp_get_txn_charge_amt_details]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@ReturnPrimaryId");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return (new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText }, chargeGetails);
        }

        public async Task<(SprocMessage, AddTransactionDetailsDto)> PushBulkTransactionAsync(BulkTransactionDetailsModel request, BulkTxnBaseModel requestModel)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@PartnerCode", requestModel.PartnerCode);
            param.Add("@ProcessId", requestModel.ProcessId);
            param.Add("@SourceCurrency", request.SourceCurrency);
            param.Add("@DestinationCurrency", request.DestinationCurrency);
            param.Add("@SendingAmount", decimal.Parse(request.SendingAmount));
            param.Add("@PaymentType", request.PaymentType);
            param.Add("@NetRecievingAmountNPR", request.ReceivingAmount);
            param.Add("@TransactionType", requestModel.TransactionType);
            param.Add("@SenderFullName", request.SenderName);
            param.Add("@SenderContactNumber", request.SendeContactNumber);
            param.Add("@SenderEmail", request.SenderEmail);
            param.Add("@SenderCountry", request.SenderCountry);
            param.Add("@SenderCity", request.SenderCity);
            param.Add("@SenderAddress", request.SenderAddress);
            param.Add("@SenderDocumentType", request.SenderDocType);
            param.Add("@SenderDocumentNumber", request.SenderDocNumber);
            param.Add("@SenderRelationshipWithRecipient", request.RelationshipWithBeneficiary);
            param.Add("@SenderOccupation", request.SenderOccupation);
            param.Add("@SenderSourceOfIncome", request.SenderSourceOfIncome);
            param.Add("@SenderPurposeOfRemittance", request.PurposeOfRemittance);
            param.Add("@RecipientFullName", request.BeneficiaryName);
            param.Add("@RecipientContactNumber", request.BeneficiaryContactNumber);
            param.Add("@RecipientEmail", "");

            param.Add("@RecipientDateOfBirth", request.BeneficiaryDOB);
            param.Add("@RecipientCountry", request.BeneficiaryCountry);
            param.Add("@RecipientCity", request.BeneficiaryCity);
            param.Add("@RecipientAddress", request.BeneficiaryAddress);
            param.Add("@RecipientRelationshipWithSender", request.BeneficiaryRelationwithSender);
            param.Add("@BankCode", request.BeneficiaryBankCode);
            param.Add("@Branch", request.BeneficiaryBankBranch);

            param.Add("@AccountHolderName", request.BeneficiaryName);
            param.Add("@AccountNumber", request.BankAccountNo);
            param.Add("@WalletCode", request.WalletCode);
            param.Add("@WalletNumber", request.WalletID);
            param.Add("@WalletHolderName", request.BeneficiaryName);
            param.Add("@IpAddress", requestModel.IpAddress);
            param.Add("@DeviceId", requestModel.DeviceId);

            param.Add("@LoggedInUser", requestModel.LoggedInUser);
            param.Add("@UserType", requestModel.UserType);


            param.Add("@ReturnPrimaryId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            var addTransactionDetails = await connection
                               .QueryFirstOrDefaultAsync<AddTransactionDetailsDto>("[dbo].[usp_push_transaction_details_bulk]", param, commandType: CommandType.StoredProcedure);

            var addTxnStatus = new SprocMessage
            {
                StatusCode = param.Get<int>("@StatusCode"),
                MsgType = param.Get<string>("@MsgType"),
                MsgText = param.Get<string>("@MsgText"),
                IdentityVal = param.Get<int>("@ReturnPrimaryId")
            };

            return (addTxnStatus, addTransactionDetails);

        }
    }
}
