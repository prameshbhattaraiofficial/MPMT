using Mpmt.Core.Dtos.Paging;
using Mpmt.Core.Dtos.PartnerApplications;
using Mpmt.Data.Repositories.PartnerApplications;

namespace Mpmt.Services.Services.PartnerApplications
{
    public class PartnerApplicationsService : IPartnerApplicationsService
    {
        private readonly IPartnerApplicationsRepo _applicationsRepo;

        public PartnerApplicationsService(IPartnerApplicationsRepo applicationsRepo)
        {
            _applicationsRepo = applicationsRepo;
        }

        public async Task<PagedList<PartnerApplicationsModel>> GetPartnerApplicationsAsync(PartnerApplicationsFilter requestFilter)
        {
            var data = await _applicationsRepo.GetPartnerApplicationsAsync(requestFilter);
            return data;
        }

        public async Task<PagedList<PublicFeedbacksModel>> GetPublicFeedbacksAsync(PublicFeedbacksFilter requestFilter)
        {
            var data = await _applicationsRepo.GetPublicFeedbacksAsync(requestFilter);
            return data;
        }
    }
}
