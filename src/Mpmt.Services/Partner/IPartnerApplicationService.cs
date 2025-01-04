using Mpmt.Core.Dtos;
using Mpmt.Core.Models.Partners.Applications;

namespace Mpmt.Services.Partner
{
    public interface IPartnerApplicationService
    {
        Task<MpmtResult> InsertAsync(PartnerApplicationRequest request);
    }
}
