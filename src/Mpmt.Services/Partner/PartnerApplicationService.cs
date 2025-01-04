using AutoMapper;
using Microsoft.AspNetCore.Http;
using Mpmt.Core.Domain.Partners.Applications;
using Mpmt.Core.Dtos;
using Mpmt.Core.Models.Partners.Applications;
using Mpmt.Data.Repositories.Partner;
using Mpmt.Services.Services.Common;

namespace Mpmt.Services.Partner
{
    public class PartnerApplicationService : BaseService, IPartnerApplicationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IPartnerApplicationRepository _applicationRepository;

        public PartnerApplicationService(
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            IPartnerApplicationRepository applicationRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _applicationRepository = applicationRepository;
        }

        public async Task<MpmtResult> InsertAsync(PartnerApplicationRequest request)
        {
            var application = _mapper.Map<PartnerApplication>(request);
            application.IpAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress.ToString();

            var submitResult = await _applicationRepository.InsertAsync(application);

            return MapSprocMessageToMpmtResult(submitResult);
        }
    }
}
