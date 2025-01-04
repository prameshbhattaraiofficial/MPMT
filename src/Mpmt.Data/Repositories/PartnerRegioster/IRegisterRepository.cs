using Mpmt.Core.Domain.Partners.Register;
using Mpmt.Core.Dtos.PartnerSignUp;
using Mpmts.Core.Dtos;

namespace Mpmt.Data.Repositories.PartnerRegioster
{
    public interface IRegisterRepository
    {
        Task<SprocMessage> RegisterPartner(RegisterPartner partnerregister);
        Task<SprocMessage> ValidateOtpAsync(string Email, string Opt);
        Task<SprocMessage> ResetOtpAsync(string Email, string Opt, DateTime expire);
        Task<PartnerDetailSignup> GetPartnerDetail(string Email);
        Task<PartnerDetailSignup> GetPartnerDetailById(string Email);


    }
}
