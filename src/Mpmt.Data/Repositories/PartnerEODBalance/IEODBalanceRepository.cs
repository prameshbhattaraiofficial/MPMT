using Mpmt.Core.Dtos.EODBalance;

namespace Mpmt.Data.Repositories.PartnerEODBalance;

public interface IEODBalanceRepository
{
    Task<IEnumerable<EODBalance>> GetPartnerEODBalanceAsync();
}