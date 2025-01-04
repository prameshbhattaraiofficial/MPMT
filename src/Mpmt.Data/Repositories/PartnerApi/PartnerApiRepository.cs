using Dapper;
using Mpmt.Core.Dtos.PartnerApi;
using Mpmt.Data.Common;
using Mpmts.Core.Dtos;
using System.Data;
using System.Net.NetworkInformation;

namespace Mpmt.Data.Repositories.PartnerApi
{
    public class PartnerApiRepository : IPartnerApiRepository
    {
        public async Task<InstrumentLists> GetInstrumentListsAsync(string partnerCode)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@PartnerCode", partnerCode);
            var resultSets = await connection
                .QueryMultipleAsync("[dbo].[usp_get_send_money_form_data]", param: param, commandType: CommandType.StoredProcedure);
            var paymentTypeList = await resultSets.ReadAsync<PaymentTypeItem>();
            var sourceCurrencyList = await resultSets.ReadAsync<SourceCurrencyItem>();
            var destinationCurrencyList = await resultSets.ReadAsync<DestinationCurrencyItem>();
            var countryList = await resultSets.ReadAsync<CountryItem>(); 
            var bankList = await resultSets.ReadAsync<BankItem>();
            var walletTypeList = await resultSets.ReadAsync<WalletTypeItem>();         
            var recipientTypeList = await resultSets.ReadAsync<RecipientTypeItem>();
            var senderDocumentTypeList = await resultSets.ReadAsync<DocumentTypeItem>(); 
            
            var result = new InstrumentLists
            {
                PaymentTypeList = paymentTypeList,
                SourceCurrencyList = sourceCurrencyList,
                DestinationCurrencyList = destinationCurrencyList,
                CountryList = countryList,        
                RecipientBankList = bankList,
                RecipientWalletTypeList = walletTypeList,         
                RecipientTypeList = recipientTypeList,
                SenderDocumentTypeList = senderDocumentTypeList,             
            };

            return result;
        }

        public async Task<TransactionStatusDetails> GetTransactionStatusAsync(string partnerCode, string remitTransactionId)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@RemitTransactionId", remitTransactionId);
            param.Add("@PartnerCode", partnerCode);

            var txnStatusDetails = await connection
                .QueryFirstOrDefaultAsync<TransactionStatusDetails>("[dbo].[usp_get_transaction_statuses]", param, commandType: CommandType.StoredProcedure);

            return txnStatusDetails;
        }

        public async Task<(SprocMessage, TxnChargeDetails)> GetTxnChargeDetailsAsync(GetTxnChargeDetailsParam detailsParam)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@PartnerCode", detailsParam.PartnerCode);
            param.Add("@SourceCurrency", detailsParam.SourceCurrency);
            param.Add("@DestinationCurrency", detailsParam.DestinationCurrency);
            param.Add("@SendingAmount", decimal.Parse(detailsParam.SourceAmount));
            param.Add("@PaymentType", detailsParam.PaymentType);

            param.Add("@ReturnPrimaryId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            var chargeDetails = await connection.QueryFirstOrDefaultAsync<TxnChargeDetails>(
                "[dbo].[usp_get_txn_charge_amt_details]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@ReturnPrimaryId");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return (new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText }, chargeDetails);
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

        public async Task<(SprocMessage, PushTransactionDetails)> PushTransactionAsync(PushTransactionParam pushTxnParam)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@PartnerCode", pushTxnParam.PartnerCode);
            param.Add("@PartnerTransactionId", pushTxnParam.PartnerTransactionId);
            param.Add("@ProcessId", pushTxnParam.ProcessId);
            param.Add("@SourceCurrency", pushTxnParam.SourceCurrency);
            param.Add("@DestinationCurrency", pushTxnParam.DestinationCurrency);
            param.Add("@SendingAmount", pushTxnParam.SendingAmount);
            param.Add("@PaymentType", pushTxnParam.PaymentType);
            //param.Add("@ServiceCharge", pushTxnParam.ServiceCharge);
            //param.Add("@NetSendingAmount", pushTxnParam.NetSendingAmount);
            //param.Add("@ConversionRate", pushTxnParam.ConversionRate);
            //param.Add("@NetRecievingAmountNPR", pushTxnParam.NetRecievingAmountNPR);
            //param.Add("@PartnerServiceCharge", pushTxnParam.PartnerServiceCharge);
            param.Add("@TransactionType", pushTxnParam.TransactionType);
            param.Add("@SenderFirstName", pushTxnParam.SenderFirstName);
            param.Add("@SenderLastName", pushTxnParam.SenderLastName);
            param.Add("@SenderContactNumber", pushTxnParam.SenderContactNumber);
            param.Add("@SenderEmail", pushTxnParam.SenderEmail);
            //param.Add("@SenderCountryCode", pushTxnParam.SenderCountryCode);
            param.Add("@SenderCountry", pushTxnParam.SenderCountry);
            param.Add("@SenderProvince", pushTxnParam.SenderProvince);
            param.Add("@SenderCity", pushTxnParam.SenderCity);
            param.Add("@SenderZipcode", pushTxnParam.SenderZipcode);
            param.Add("@SenderAddress", pushTxnParam.SenderAddress);
            //param.Add("@SenderDocumentTypeId", pushTxnParam.SenderDocumentTypeId);
            param.Add("@SenderDocumentType", pushTxnParam.SenderDocumentType);
            param.Add("@SenderDocumentNumber", pushTxnParam.SenderDocumentNumber);
            //param.Add("@SenderRelationshipId", pushTxnParam.SenderRelationshipId);
            //param.Add("@SenderPurposeId", pushTxnParam.SenderPurposeId);
            //param.Add("@SenderOccupationId", pushTxnParam.SenderOccupationId);
            param.Add("@SenderRelationshipWithRecipient", pushTxnParam.SenderRelationshipWithRecipient);
            param.Add("@SenderOccupation", pushTxnParam.SenderOccupation);
            param.Add("@SenderSourceOfIncome", pushTxnParam.SenderSourceOfIncome);
            param.Add("@SenderPurposeOfRemittance", pushTxnParam.SenderPurposeOfRemittance);
            param.Add("@SenderRemarks", pushTxnParam.SenderRemarks);
            param.Add("@RecipientType", pushTxnParam.RecipientType);
            param.Add("@RecipientFirstName", pushTxnParam.RecipientFirstName);
            param.Add("@RecipientLastName", pushTxnParam.RecipientLastName);
            param.Add("@JointAccountFirstName", pushTxnParam.JointAccountFirstName);
            param.Add("@JointAccountLastName", pushTxnParam.JointAccountLastName);
            param.Add("@BusinessName", pushTxnParam.BusinessName);
            param.Add("@RecipientContactNumber", pushTxnParam.RecipientContactNumber);
            param.Add("@RecipientEmail", pushTxnParam.RecipientEmail);
            param.Add("@RecipientDateOfBirth", pushTxnParam.RecipientDateOfBirth);
            //param.Add("@RecipientCountryCode", pushTxnParam.RecipientCountryCode);
            param.Add("@RecipientCountry", pushTxnParam.RecipientCountry);
            param.Add("@RecipientCity", pushTxnParam.RecipientCity);
            param.Add("@RecipientZipcode", pushTxnParam.RecipientZipcode);
            param.Add("@RecipientAddress", pushTxnParam.RecipientAddress);
            //param.Add("@RecipientRelationshipId", pushTxnParam.RecipientRelationshipId);
            param.Add("@RecipientRelationshipWithSender", pushTxnParam.RecipientRelationshipWithSender);
            param.Add("@BankCode", pushTxnParam.BankCode);
            param.Add("@Branch", pushTxnParam.Branch);
            param.Add("@AccountHolderName", pushTxnParam.AccountHolderName);
            param.Add("@AccountNumber", pushTxnParam.AccountNumber);
            param.Add("@WalletCode", pushTxnParam.WalletCode);
            param.Add("@WalletNumber", pushTxnParam.WalletNumber);
            param.Add("@WalletHolderName", pushTxnParam.WalletHolderName);
            param.Add("@IpAddress", pushTxnParam.IpAddress);
            param.Add("@DeviceId", pushTxnParam.DeviceId);
            param.Add("@LoggedInUser", pushTxnParam.LoggedInUser);
            param.Add("@UserType", pushTxnParam.UserType);

            param.Add("@ReturnPrimaryId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            var pushTxnDetails = await connection
                .QueryFirstOrDefaultAsync<PushTransactionDetails>("[dbo].[usp_push_transaction_details_Api]", param, commandType: CommandType.StoredProcedure);

            var pushTxnStatus = new SprocMessage
            {
                StatusCode = param.Get<int>("@StatusCode"),
                MsgType = param.Get<string>("@MsgType"),
                MsgText = param.Get<string>("@MsgText"),
                IdentityVal = param.Get<int>("@ReturnPrimaryId")
            };

            return (pushTxnStatus, pushTxnDetails);
        }

        public async Task<(SprocMessage, PushTransactionDetails)> PushTransactionDetailsAsync(PushTransactionRequestDetailsParam pushTxnParam)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@PartnerCode", pushTxnParam.PartnerCode);
            param.Add("@PartnerTransactionId", pushTxnParam.PartnerTransactionId);
            param.Add("@ProcessId", pushTxnParam.ProcessId);
            param.Add("@SourceCurrency", pushTxnParam.SourceCurrency);
            param.Add("@DestinationCurrency", pushTxnParam.DestinationCurrency);
            param.Add("@SendingAmount", pushTxnParam.SendingAmount);
            param.Add("@PaymentType", pushTxnParam.PaymentType);
            param.Add("@NetRecievingAmountNPR", pushTxnParam.NetReceivingAmount);
            //param.Add("@ConversionRate", pushTxnParam.ConversionRate);
            //param.Add("@NetRecievingAmountNPR", pushTxnParam.NetRecievingAmountNPR);
            param.Add("@TransactionType", pushTxnParam.TransactionType);
            param.Add("@SenderFirstName", pushTxnParam.SenderFirstName);
            param.Add("@SenderLastName", pushTxnParam.SenderLastName);
            param.Add("@SenderContactNumber", pushTxnParam.SenderContactNumber);
            param.Add("@SenderEmail", pushTxnParam.SenderEmail);
            param.Add("@SenderCountry", pushTxnParam.SenderCountry);
            param.Add("@SenderProvince", pushTxnParam.SenderProvince);
            param.Add("@SenderCity", pushTxnParam.SenderCity);
            param.Add("@SenderZipcode", pushTxnParam.SenderZipcode);
            param.Add("@SenderAddress", pushTxnParam.SenderAddress);
            param.Add("@SenderDocumentType", pushTxnParam.SenderDocumentType);
            param.Add("@SenderDocumentNumber", pushTxnParam.SenderDocumentNumber);
            param.Add("@SenderRelationshipWithRecipient", pushTxnParam.SenderRelationshipWithRecipient);
            param.Add("@SenderOccupation", pushTxnParam.SenderOccupation);
            param.Add("@SenderSourceOfIncome", pushTxnParam.SenderSourceOfIncome);
            param.Add("@SenderPurposeOfRemittance", pushTxnParam.SenderPurposeOfRemittance);
            param.Add("@SenderRemarks", pushTxnParam.SenderRemarks);
            param.Add("@RecipientType", pushTxnParam.RecipientType);
            param.Add("@RecipientFirstName", pushTxnParam.RecipientFirstName);
            param.Add("@RecipientLastName", pushTxnParam.RecipientLastName);
            param.Add("@JointAccountFirstName", pushTxnParam.JointAccountFirstName);
            param.Add("@JointAccountLastName", pushTxnParam.JointAccountLastName);
            param.Add("@BusinessName", pushTxnParam.BusinessName);
            param.Add("@RecipientContactNumber", pushTxnParam.RecipientContactNumber);
            param.Add("@RecipientEmail", pushTxnParam.RecipientEmail);
            param.Add("@RecipientDateOfBirth", pushTxnParam.RecipientDateOfBirth);
            param.Add("@RecipientCountry", pushTxnParam.RecipientCountry);
            param.Add("@RecipientCity", pushTxnParam.RecipientCity);
            param.Add("@RecipientZipcode", pushTxnParam.RecipientZipcode);
            param.Add("@RecipientAddress", pushTxnParam.RecipientAddress);
            param.Add("@RecipientRelationshipWithSender", pushTxnParam.RecipientRelationshipWithSender);
            param.Add("@BankCode", pushTxnParam.BankCode);
            param.Add("@Branch", pushTxnParam.Branch);
            param.Add("@AccountHolderName", pushTxnParam.AccountHolderName);
            param.Add("@AccountNumber", pushTxnParam.AccountNumber);
            param.Add("@WalletCode", pushTxnParam.WalletCode);
            param.Add("@WalletNumber", pushTxnParam.WalletNumber);
            param.Add("@WalletHolderName", pushTxnParam.WalletHolderName);
            param.Add("@IpAddress", pushTxnParam.IpAddress);
            param.Add("@DeviceId", pushTxnParam.DeviceId);
            param.Add("@LoggedInUser", pushTxnParam.LoggedInUser);
            param.Add("@UserType", pushTxnParam.UserType);

            param.Add("@ReturnPrimaryId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            var pushTxnDetails = await connection
                .QueryFirstOrDefaultAsync<PushTransactionDetails>("[dbo].[usp_push_transaction_details_Api_New]", param, commandType: CommandType.StoredProcedure);

            var pushTxnStatus = new SprocMessage
            {
                StatusCode = param.Get<int>("@StatusCode"),
                MsgType = param.Get<string>("@MsgType"),
                MsgText = param.Get<string>("@MsgText"),
                IdentityVal = param.Get<int>("@ReturnPrimaryId")
            };

            return (pushTxnStatus, pushTxnDetails);
        }

        public async Task<SprocMessage> validateReferenceNumber(ValidateAccountRequest request, string partnerCode)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@VendorId", partnerCode);
            param.Add("@ReferenceId", request.ReferenceId);      

            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            var pushTxnDetails = await connection
                .QueryAsync("[dbo].[usp_validate_vendor_referenceid]", param, commandType: CommandType.StoredProcedure);

            var pushTxnStatus = new SprocMessage
            {
                StatusCode = param.Get<int>("@StatusCode"),
                MsgType = param.Get<string>("@MsgType"),
                MsgText = param.Get<string>("@MsgText"),
            };

            return pushTxnStatus;
        }
    }
}
