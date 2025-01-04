using Mpmt.Core.Domain.Partners.Recipient;
using Mpmt.Core.Dtos.Paging;
using Mpmts.Core.Dtos;
using System.Security.Claims;

namespace Mpmt.Services.Partner
{
    public interface IPartnerRecipentServices
    {
        Task<PagedList<RecipientsList>> GetRecipientsAsync(RecipientFilter recipientFilter, ClaimsPrincipal claimsPrincipal);
        Task<RecipientsList> GetRecipientsByIdAsync(int recipientid);
        Task<SprocMessage> UpdateRecipientAsync(RecipientAddUpdate recipientAdd, ClaimsPrincipal claimsPrincipal);
        Task<SprocMessage> AddRecipientAsync(RecipientAddUpdate recipientAdd, ClaimsPrincipal claimsPrincipal);
    }
}
