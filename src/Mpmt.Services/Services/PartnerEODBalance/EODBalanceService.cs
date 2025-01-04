using Mpmt.Core.Dtos.EODBalance;
using Mpmt.Data.Repositories.PartnerEODBalance;

namespace Mpmt.Services.Services.PartnerEODBalance;

public class EODBalanceService : IEODBalanceService
{
    private readonly IEODBalanceRepository _EODBalanceRepository;

    public EODBalanceService(IEODBalanceRepository eODBalanceRepository)
    {
        _EODBalanceRepository = eODBalanceRepository;
    }

    public async Task<IEnumerable<EODBalance>> GetPartnerEODBalanceAsync()
    {
        var response = await _EODBalanceRepository.GetPartnerEODBalanceAsync();
        return response;
    }
}