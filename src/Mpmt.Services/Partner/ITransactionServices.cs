using Mpmt.Core.Domain.Admin.Reports;
using Mpmt.Core.Dtos.Paging;
using Mpmt.Core.Dtos.ReceiptDownloadModel;
using Mpmt.Core.Dtos.Transaction;
using Mpmt.Core.ViewModel.AdminReport;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.Partner;

public interface ITransactionServices
{
    Task<IEnumerable<TransactionSender>> GetSenderByTxnId(string txnId);
    Task<IEnumerable<TransactionStatus>> GetTransactionStatus(string txnId);
    Task<TransactionDetailView> GetTxnById(string txnId);
    Task<ReceiptDetailModel> GetReceiptDetailsById(string txnId);
    Task<IEnumerable<Paymentdetails>> PaymentDetailsByTxnIdAsync(string txnId);
    Task<IEnumerable<TransactionRecipient>> GetRecipientByTxnId(string txnId);
    Task<PagedList<RemitTransactionList>> GetRemitTxnAsync(RemitTransactionFilter txnFilter);
    Task<ReceiverAccountDetails> ManageDetailByTxnIdAsync(string transactionId);
    Task<ReceiverCashoutDetails> ReceiverDetailsCashoutByTxnIdAsync(string transactionId);
    Task<SprocMessage> UpdateAccountDetails(ReceiverAccountDetailsVM model);
    Task<SprocMessage> UpdateReceiverCashoutDetails(ReceiverAccountDetailsCashoutVM model);
    Task<TransactionDetailsAdmin> GetTransactionParameterByTxnId(string transactionId);
}
