using Mpmt.Core.Domain.Partners.SendTransactions;
using Mpmt.Core.Dtos.Partner;
using Mpmts.Core.Dtos;

namespace Mpmt.Data.Repositories.Partner;

public interface IPartnerDashBoardRepo
{
    Task<PartnerDashBoard> GetPartnerDashBoard(string Partnercode);
    Task<IEnumerable<FrequencyWiseTransaction>> GetTransactionDataFrequencyWise(string frequency, string partnerCode);
    Task<IEnumerable<DashboardTransactionStatus>> GetTransactionStatusDashboard(string frequency, string partnerCode);
    Task<IEnumerable<DashboardPartnerSender>> GetPartnerSenderDashboard(string partnerCode, string frequency, string filterBy, string orderBy);
    Task<IEnumerable<DashboardWalletBalance>> GetPartnerBalanceDashboard(string partnerCode, string filterBy, string orderBy);
    Task<SendTransferAmountDetailDto> GetTransferAmountDetailsAsync(GetSendTransferAmountDetailRequest request);
    Task<SprocMessage> SendTransferAmount(GetSendTransferAmountDetailRequest request);
    Task<SprocMessage> CheckWalletBalance(GetSendTransferAmountDetailRequest request);
    Task<SprocMessage> PartnerDashboardOTPAsync(TokenVerification tokenVerification);
}
