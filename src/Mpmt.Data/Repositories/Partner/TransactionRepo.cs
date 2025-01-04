using AutoMapper;
using Dapper;
using Mpmt.Core.Domain.Admin.Reports;
using Mpmt.Core.Dtos.Paging;
using Mpmt.Core.Dtos.ReceiptDownloadModel;
using Mpmt.Core.Dtos.Transaction;
using Mpmt.Data.Common;
using Mpmts.Core.Dtos;
using System.Data;

namespace Mpmt.Data.Repositories.Partner
{
    public class TransactionRepo : ITransactionRepo
    {
        private readonly IMapper _mapper;

        public TransactionRepo(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<ReceiptDetailModel> GetReceiptDetailsById(string txnId)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            try
            {
                var param = new DynamicParameters();
                param.Add("@TransactionId", txnId);

                var data = await connection
                    .QueryFirstOrDefaultAsync<ReceiptDetailModel>("[dbo].[usp_get_receipt_detail_by_id]", param: param, commandType: CommandType.StoredProcedure);
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<TransactionRecipient>> GetRecipientByTxnId(string txnId)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@TransactionId", txnId);

            var data = await connection
                .QueryAsync<TransactionRecipient>("[dbo].[usp_get_recipient_by_txnid]", param: param, commandType: CommandType.StoredProcedure);
            return data;
        }

        public async Task<PagedList<RemitTransactionList>> GetRemitTxnAsync(RemitTransactionFilter txnFilter)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@PartnerCode", txnFilter.PartnerCode);
            param.Add("@StartDate", txnFilter.StartDate);
            param.Add("@EndDate", txnFilter.EndDate);
            param.Add("@SourceCurrency", txnFilter.SourceCurrency);
            param.Add("@DestinationCurrency", txnFilter.DestinationCurrency);
            param.Add("@TransactionId", txnFilter.TransactionId);
            param.Add("@SignType", txnFilter.SignType);
            param.Add("@TransactionType", txnFilter.TransactionType);
            param.Add("@UserType", txnFilter.UserType);
            param.Add("@LoggedInUser", txnFilter.LoggedInUser);
            param.Add("@PageNumber", txnFilter.PageNumber);
            param.Add("@PageSize", txnFilter.PageSize);
            param.Add("@SortingCol", txnFilter.SortBy);
            param.Add("@SortType", txnFilter.SortOrder);
            param.Add("@SearchVal", txnFilter.SearchVal);
            var data = await connection
                .QueryMultipleAsync("[dbo].[usp_get_remit_transactions]", param: param, commandType: CommandType.StoredProcedure);

            var txnlist = await data.ReadAsync<RemitTransactionList>();
            var pagedInfo = await data.ReadFirstAsync<PageInfo>();
            var mappeddata = _mapper.Map<PagedList<RemitTransactionList>>(pagedInfo);
            mappeddata.Items = txnlist;
            return mappeddata;
        }

        public async Task<IEnumerable<TransactionSender>> GetSenderByTxnId(string txnId)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@TransactionId", txnId);

            var data = await connection
                .QueryAsync<TransactionSender>("[dbo].[usp_get_sender_by_txnid]", param: param, commandType: CommandType.StoredProcedure);
            return data;
        }

        public async Task<TransactionDetailsAdmin> GetTransactionParameterByTxnId(string transactionId)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            try
            {
                var param = new DynamicParameters();
                param.Add("@TransactionId", transactionId);

                var data = await connection
                    .QueryFirstOrDefaultAsync<TransactionDetailsAdmin>("[dbo].[usp_get_transaction_details_by_txnid]", param: param, commandType: CommandType.StoredProcedure);
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<TransactionStatus>> GetTransactionStatus(string txnId)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@RemitTransactionId", txnId);

            var data = await connection
                .QueryAsync<TransactionStatus>("[dbo].[usp_get_transaction_status]", param: param, commandType: CommandType.StoredProcedure);
            return data;
        }

        public async Task<TransactionDetailView> GetTxnById(string txnId)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            try
            {
                var param = new DynamicParameters();
                param.Add("@TransactionId", txnId);

                var data = await connection
                    .QueryFirstOrDefaultAsync<TransactionDetailView>("[dbo].[usp_get_transaction_by_txnid]", param: param, commandType: CommandType.StoredProcedure);
                return data;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<ReceiverAccountDetails> ManageDetailByTxnIdAsync(string transactionId)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            try
            {
                var param = new DynamicParameters();
                param.Add("@TransactionId", transactionId);

                var data = await connection
                    .QueryFirstOrDefaultAsync<ReceiverAccountDetails>("[dbo].[usp_get_acc_details_by_txnid]", param: param, commandType: CommandType.StoredProcedure);
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<Paymentdetails>> PaymentDetailsByTxnId(string txnId)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@TransactionId", txnId);

            var data = await connection
                .QueryAsync<Paymentdetails>("[dbo].[usp_get_paymentdetails_bytxnid]", param: param, commandType: CommandType.StoredProcedure);
            return data;
        }

        public async Task<ReceiverCashoutDetails> ReceiverDetailsCashoutByTxnIdAsync(string transactionId)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            try
            {
                var param = new DynamicParameters();
                param.Add("@TransactionId", transactionId);

                var data = await connection
                    .QueryFirstOrDefaultAsync<ReceiverCashoutDetails>("[dbo].[usp_get_receiver_details_cashout_by_txnid]", param: param, commandType: CommandType.StoredProcedure);
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<SprocMessage> UpdateAccountDetails(ReceiverAccountDetails model)
        {
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();
                var param = new DynamicParameters();
                param.Add("@TransactionId", model.TransactionId);
                param.Add("@WalletHolderName", model.WalletHolderName);
                param.Add("@WalletNumber", model.WalletNumber);
                param.Add("@BankCode", model.BankCode);
                param.Add("@Branch", model.Branch);
                param.Add("@PaymentType", model.PaymentType);
                param.Add("@AccountHolderName", model.AccountHolderName);
                param.Add("@AccountNumber", model.AccountNumber);
                param.Add("@Remarks", model.TxnUpdateRemarks);
                param.Add("@LoggedInUser", "Admin");
                param.Add("@UserType", model.UserType);


                param.Add("@ReturnPrimaryId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
                param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                _ = await connection.ExecuteAsync("[dbo].[usp_receiver_account_detail_update]", param, commandType: CommandType.StoredProcedure);

                var identityVal = param.Get<int>("@ReturnPrimaryId");
                var statusCode = param.Get<int>("@StatusCode");
                var msgType = param.Get<string>("@MsgType");
                var msgText = param.Get<string>("@MsgText");

                return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<SprocMessage> UpdateReceiverCashoutDetails(ReceiverCashoutDetails model)
        {
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();
                var param = new DynamicParameters();
                param.Add("@TransactionId", model.TransactionId);
                param.Add("@ReceiverName", model.ReceiverName);
                param.Add("@ContactNumber", model.ContactNumber);
                //param.Add("@DistrictCode", model.DistrictCode);
                //param.Add("@FullAddress", model.FullAddress);
                param.Add("@Remarks", model.Remarks);
                param.Add("@LoggedInUser", "Admin");
                param.Add("@UserType", model.UserType);

                param.Add("@ReturnPrimaryId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
                param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                _ = await connection.ExecuteAsync("[dbo].[usp_receiver_detail_update_cashout]", param, commandType: CommandType.StoredProcedure);

                var identityVal = param.Get<int>("@ReturnPrimaryId");
                var statusCode = param.Get<int>("@StatusCode");
                var msgType = param.Get<string>("@MsgType");
                var msgText = param.Get<string>("@MsgText");

                return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
