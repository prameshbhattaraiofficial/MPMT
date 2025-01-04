using Mpmt.Core.Dtos.Paging;
using Mpmt.Core.Dtos.Partner;
using Mpmt.Core.Dtos.PartnerSignUp;
using Mpmt.Data.Repositories.Partner;
using Mpmt.Data.Repositories.PartnerRegioster;
using Mpmts.Core.Dtos;
using System.Security.Claims;

namespace Mpmt.Services.Partner
{
    public class RemitPartnerRegisterServices : IRemitPartnerRegisterServices
    {
        private readonly IRemitPartnerRegisterRepo _remitPartnerRegister;
        private readonly IRegisterRepository _registerRepository;
        public RemitPartnerRegisterServices(IRemitPartnerRegisterRepo remitPartnerRegister, IRegisterRepository registerRepository)
        {
            _remitPartnerRegister = remitPartnerRegister;
            _registerRepository = registerRepository;
        }

        public async Task<SprocMessage> ApprovedPartnerRequest(RemitPartnerRequest remitPartnerRequest, ClaimsPrincipal claimsPrincipal)
        {
            remitPartnerRequest.OperationMode = "A";
            remitPartnerRequest.LoggedInUser = claimsPrincipal?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            remitPartnerRequest.UserType = claimsPrincipal?.Claims.FirstOrDefault(x => x.Type == "UserType")?.Value;
            var Response = await _remitPartnerRegister.ApprovedRejectPartnerRequest(remitPartnerRequest);
            return Response;
        }

        public async Task<PartnerDetailSignup> GetRegisterPartner(string Email)
        {
            var response = await _registerRepository.GetPartnerDetail(Email);
            return response;

        }

        public async Task<PagedList<RemitPartnerRegister>> GetRemitPartnerAsync(RemitPartnerRegisterFilter request)
        {
            var data = await _remitPartnerRegister.GetRemitPartnerAsync(request);
            return data;
        }

        public async Task<SprocMessage> RejectPartnerRequest(RemitPartnerRequest remitPartnerRequest, ClaimsPrincipal claimsPrincipal)
        {
            remitPartnerRequest.OperationMode = "R";
            remitPartnerRequest.LoggedInUser = claimsPrincipal?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            remitPartnerRequest.UserType = claimsPrincipal?.Claims.FirstOrDefault(x => x.Type == "UserType")?.Value;
            var Response = await _remitPartnerRegister.ApprovedRejectPartnerRequest(remitPartnerRequest);
            return Response;
        }
    }
}
