using Mpmt.Core.Dtos.Paging;
using Mpmt.Core.Dtos.PartnerApplications;

namespace Mpmt.Data.Repositories.PartnerApplications
{
    public interface IPartnerApplicationsRepo
    {
        Task<PagedList<PartnerApplicationsModel>> GetPartnerApplicationsAsync(PartnerApplicationsFilter requestFilter);
        Task<PagedList<PublicFeedbacksModel>> GetPublicFeedbacksAsync(PublicFeedbacksFilter requestFilter);
    }
}
    