using Mpmt.Core.Dtos.AdminDashboard;
using Mpmt.Core.Dtos.Partner;
using Mpmt.Data.Repositories.AdminDashBoard;

namespace Mpmt.Services.Services.AdminDashBoard;

public class AdminDashBoardServices : IAdminDashBoardServices
{
    private readonly IAdminDashBoardRepo _dashBoardRepo;

    public AdminDashBoardServices(IAdminDashBoardRepo dashBoardRepo)
    {
        _dashBoardRepo = dashBoardRepo;
    }

    public async Task<AdminDashboardDetails> GetAdminDashBoard()
    {
        var data = await _dashBoardRepo.GetAdminDashBoard();
        return data;
    }

    public async Task<IEnumerable<DashboardApproxDays>> GetPartnerBalanceApproxDaysDashboard(string filterBy, string orderBy)
    {
        var result = await _dashBoardRepo.GetPartnerBalanceApproxDaysDashboard("ADMIN", filterBy, orderBy);
        return result;
    }

    public async Task<IEnumerable<DashboardAdminPaymentMode>> GetPaymentModeDashboard(string frequency, string filterBy, string orderBy)
    {
        var result = await _dashBoardRepo.GetPaymentModeDashboard(frequency, filterBy, orderBy);
        return result;
    }

    public async Task<IEnumerable<DashboardAdminPaymentMode>> GetThresholdDataDashboard(string frequency,string filterBy, string orderBy)
    {
        var result = await _dashBoardRepo.GetThresholdDataDashboard(frequency, filterBy, orderBy);
        return result;
    }

    public async Task<IEnumerable<DashboardAdminTopAgent>> GetTopAgentDashboard(string frequency, string filterBy, string orderBy)
    {
        var result = await _dashBoardRepo.GetTopAgentDashboard(frequency, filterBy, orderBy);
        return result;
    }

    public async Task<IEnumerable<DashboardTopAgentLocation>> GetTopAgentLocationDashboard(string frequency, string filterBy, string orderBy)
    {
        var result = await _dashBoardRepo.GetTopAgentLocationDashboard(frequency, filterBy, orderBy);
        return result;
    }

    public async Task<IEnumerable<DashboardAdminTopPartner>> GetTopPartnerDashboard(string frequency, string filterBy, string orderBy)
    {
        var result = await _dashBoardRepo.GetTopPartnerDashboard(frequency, filterBy, orderBy);
        return result;
    }

    public async Task<IEnumerable<FrequencyWiseTransaction>> GetTransactionDataFrequencyWise(string frequency)
    {
        var result = await _dashBoardRepo.GetTransactionDataFrequencyWise(frequency);
        return result;
    }

    public async Task<IEnumerable<DashboardTransactionStatus>> GetTransactionStatusDashboard(string frequency)
    {
        var result = await _dashBoardRepo.GetTransactionStatusDashboard(frequency);
        return result;
    }
}
