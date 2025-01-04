using Mpmt.Core.Domain.Partners.SendTransactions;
using Mpmt.Core.Dtos.Partner;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.Partner;

public interface IDashBoardServices
{
    Task<PartnerDashBoard> GetPartnerDashBoard(string Partnercode);
    Task<IEnumerable<FrequencyWiseTransaction>> GetTransactionDataFrequencyWise(string frequency);
    Task<IEnumerable<DashboardTransactionStatus>> GetTransactionStatusDashboard(string frequency);    
    Task<IEnumerable<DashboardPartnerSender>> GetPartnerSenderDashboard(string frequency, string filterBy, string orderBy);
    Task<IEnumerable<DashboardWalletBalance>> GetPartnerBalanceDashboard(string filterBy, string orderBy); 
    Task<SendTransferAmountDetailDto> GetTransferAmountDetailsAsync(GetSendTransferAmountDetailRequest request);
    Task<SprocMessage> SendTransferAmount(GetSendTransferAmountDetailRequest request);
    Task<SprocMessage> CheckWalletBalance(GetSendTransferAmountDetailRequest request);  
    Task SendOtpVerification();
}