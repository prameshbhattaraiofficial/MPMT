using Mpmt.Core.Dtos.Paging;
using Mpmt.Core.Dtos.Partner;
using Mpmts.Core.Dtos;

namespace Mpmt.Data.Repositories.Partner
{
    public interface IRemitPartnerRegisterRepo
    {
        Task<PagedList<RemitPartnerRegister>> GetRemitPartnerAsync(RemitPartnerRegisterFilter request);
        Task<SprocMessage> ApprovedRejectPartnerRequest(RemitPartnerRequest remitPartnerRequest);
    }
}
