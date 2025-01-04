using Mpmt.Core.Dtos.EODBalance;

namespace Mpmt.Services.Services.PartnerEODBalance;

public interface IEODBalanceService
{
    Task<IEnumerable<EODBalance>> GetPartnerEODBalanceAsync();
}