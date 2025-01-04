using Mpmt.Core.Domain.Partners.Applications;
using Mpmts.Core.Dtos;

namespace Mpmt.Data.Repositories.Partner
{
    public interface IPartnerApplicationRepository
    {
        Task<SprocMessage> InsertAsync(PartnerApplication application);
    }
}
