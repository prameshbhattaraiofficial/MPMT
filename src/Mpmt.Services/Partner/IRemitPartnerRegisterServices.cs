using Mpmt.Core.Dtos.Paging;
using Mpmt.Core.Dtos.Partner;
using Mpmt.Core.Dtos.PartnerSignUp;
using Mpmts.Core.Dtos;
using System.Security.Claims;

namespace Mpmt.Services.Partner
{
    public interface IRemitPartnerRegisterServices
    {
        Task<PagedList<RemitPartnerRegister>> GetRemitPartnerAsync(RemitPartnerRegisterFilter request);
        Task<SprocMessage> ApprovedPartnerRequest(RemitPartnerRequest remitPartnerRequest, ClaimsPrincipal claimsPrincipal);
        Task<SprocMessage> RejectPartnerRequest(RemitPartnerRequest remitPartnerRequest, ClaimsPrincipal claimsPrincipal);
        Task<PartnerDetailSignup> GetRegisterPartner(string Email);
        


    }
}
