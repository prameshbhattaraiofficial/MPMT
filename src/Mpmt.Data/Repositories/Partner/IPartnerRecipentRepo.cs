using Mpmt.Core.Domain.Partners.Recipient;
using Mpmt.Core.Dtos.Paging;
using Mpmts.Core.Dtos;

namespace Mpmt.Data.Repositories.Partner
{
    public interface IPartnerRecipentRepo
    {
        Task<PagedList<RecipientsList>> GetRecipientsAsync(RecipientFilter recipientFilter, string partnercode = "");
        Task<RecipientsList> GetRecipientsByIdAsync(int recipientid);
        Task<SprocMessage> AddUpdateRecipientAsync(RecipientAddUpdate recipientAdd);
    }
}
