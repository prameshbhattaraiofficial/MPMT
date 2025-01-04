using AutoMapper;
using Microsoft.AspNetCore.Http;
using Mpmt.Core.Domain.Admin.Reports;
using Mpmt.Core.Dtos.Paging;
using Mpmt.Core.Dtos.ReceiptDownloadModel;
using Mpmt.Core.Dtos.Transaction;
using Mpmt.Core.ViewModel.AdminReport;
using Mpmt.Data.Repositories.Partner;
using Mpmts.Core.Dtos;
using System.Security.Claims;

namespace Mpmt.Services.Partner;

public class TransactionServices : ITransactionServices
{
    private readonly ITransactionRepo _transactionRepo;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ClaimsPrincipal _loggedInUser;
    private readonly IMapper _mapper;

    public TransactionServices(ITransactionRepo transactionRepo, IHttpContextAccessor httpContextAccessor, IMapper mapper)
    {
        _transactionRepo = transactionRepo;
        _httpContextAccessor = httpContextAccessor;
        _loggedInUser = httpContextAccessor.HttpContext.User;
        _mapper = mapper;
    }

    public async Task<ReceiptDetailModel> GetReceiptDetailsById(string txnId)
    {
        var data = await _transactionRepo.GetReceiptDetailsById(txnId);
        return data;
    }

    public async Task<IEnumerable<TransactionRecipient>> GetRecipientByTxnId(string txnId)
    {
        var data = await _transactionRepo.GetRecipientByTxnId(txnId);
        return data;
    }

    public async Task<PagedList<RemitTransactionList>> GetRemitTxnAsync(RemitTransactionFilter txnFilter)
    {
        var PartnerCode = _loggedInUser.Claims.FirstOrDefault(x => x.Type == "PartnerCode").Value;
        txnFilter.PartnerCode = PartnerCode;
        var data = await _transactionRepo.GetRemitTxnAsync(txnFilter);
        return data;
    }

    public async Task<IEnumerable<TransactionSender>> GetSenderByTxnId(string txnId)
    {
        var data = await _transactionRepo.GetSenderByTxnId(txnId);
        return data;
    }

    public async Task<TransactionDetailsAdmin> GetTransactionParameterByTxnId(string transactionId)
    {
        var data = await _transactionRepo.GetTransactionParameterByTxnId(transactionId);
        return data;
    }

    public async Task<IEnumerable<TransactionStatus>> GetTransactionStatus(string txnId)
    {
        var data = await _transactionRepo.GetTransactionStatus(txnId);
        return data;
    }

    public async Task<TransactionDetailView> GetTxnById(string txnId)
    {
        var data = await _transactionRepo.GetTxnById(txnId);
        return data;
    }

    public async Task<ReceiverAccountDetails> ManageDetailByTxnIdAsync(string transactionId)
    {
        var data = await _transactionRepo.ManageDetailByTxnIdAsync(transactionId);
        return data;
    }

    public async Task<IEnumerable<Paymentdetails>> PaymentDetailsByTxnIdAsync(string txnId)
    {
        var data = await _transactionRepo.PaymentDetailsByTxnId(txnId);
        return data;
    }

    public async Task<ReceiverCashoutDetails> ReceiverDetailsCashoutByTxnIdAsync(string transactionId)
    {
        var data = await _transactionRepo.ReceiverDetailsCashoutByTxnIdAsync(transactionId);
        return data;
    }

    public async Task<SprocMessage> UpdateAccountDetails(ReceiverAccountDetailsVM viewmodel)
    {
        var manageAccDetail = _mapper.Map<ReceiverAccountDetails>(viewmodel);

        var data = await _transactionRepo.UpdateAccountDetails(manageAccDetail);
        return data;
    }

    public async Task<SprocMessage> UpdateReceiverCashoutDetails(ReceiverAccountDetailsCashoutVM model)
    {
        var manageAccDetail = _mapper.Map<ReceiverCashoutDetails>(model);

        var data = await _transactionRepo.UpdateReceiverCashoutDetails(manageAccDetail);
        return data;
    }
}
