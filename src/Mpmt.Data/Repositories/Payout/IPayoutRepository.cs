using Mpmt.Core.Dtos.Payout;
using Mpmts.Core.Dtos;

namespace Mpmt.Data.Repositories.Payout
{
    public interface IPayoutRepository
    {
        Task<(SprocMessage, PayoutReferenceInfo)> GetPayoutReferenceInfoAsync(GetPayoutReferenceInfoDto dto);
    }
}
