using Mpmt.Core.Dtos.Paging;
using Mpmt.Core.Dtos.PartnerApplications;

namespace Mpmt.Services.Services.PartnerApplications
{
    public interface IPartnerApplicationsService
    {
        Task<PagedList<PartnerApplicationsModel>> GetPartnerApplicationsAsync(PartnerApplicationsFilter requestFilter);
        Task<PagedList<PublicFeedbacksModel>> GetPublicFeedbacksAsync(PublicFeedbacksFilter requestFilter);
    }
}
