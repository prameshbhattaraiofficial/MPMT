using Mpmt.Core.Dtos.AdminDashboard;
using Mpmt.Core.Dtos.Partner;

namespace Mpmt.Services.Services.AdminDashBoard;

public interface IAdminDashBoardServices
{
    Task<AdminDashboardDetails> GetAdminDashBoard();
    Task<IEnumerable<FrequencyWiseTransaction>> GetTransactionDataFrequencyWise(string frequency);
    Task<IEnumerable<DashboardTransactionStatus>> GetTransactionStatusDashboard(string frequency);
    Task<IEnumerable<DashboardAdminTopPartner>> GetTopPartnerDashboard(string frequency, string filterBy, string orderBy);
    Task<IEnumerable<DashboardApproxDays>> GetPartnerBalanceApproxDaysDashboard(string filterBy, string orderBy); 
    Task<IEnumerable<DashboardAdminTopAgent>> GetTopAgentDashboard(string frequency, string filterBy, string orderBy);
    Task<IEnumerable<DashboardTopAgentLocation>> GetTopAgentLocationDashboard(string frequency, string filterBy, string orderBy);   
    Task<IEnumerable<DashboardAdminPaymentMode>> GetPaymentModeDashboard(string frequency, string filterBy, string orderBy);
    Task<IEnumerable<DashboardAdminPaymentMode>> GetThresholdDataDashboard(string frequency, string filterBy, string orderBy);  
}
