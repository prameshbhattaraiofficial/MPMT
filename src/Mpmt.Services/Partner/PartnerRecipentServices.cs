using Mpmt.Core.Domain.Partners.Recipient;
using Mpmt.Core.Dtos.Paging;
using Mpmt.Data.Repositories.Partner;
using Mpmts.Core.Dtos;
using Org.BouncyCastle.Cms;
using System.Security.Claims;

namespace Mpmt.Services.Partner
{
    public class PartnerRecipentServices : IPartnerRecipentServices
    {
        private readonly IPartnerRecipentRepo _recipentRepo;

        public PartnerRecipentServices(IPartnerRecipentRepo recipentRepo)
        {
            _recipentRepo = recipentRepo;
        }

        public async Task<SprocMessage> AddRecipientAsync(RecipientAddUpdate recipientAdd, ClaimsPrincipal claimsPrincipal)
        {
            recipientAdd.OperationMode = "A";
            recipientAdd.LoggedInuser = claimsPrincipal?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            recipientAdd.UserType = claimsPrincipal?.Claims.FirstOrDefault(x => x.Type == "UserType")?.Value;
            var Response = await _recipentRepo.AddUpdateRecipientAsync(recipientAdd);
            return Response;
        }

        public async Task<PagedList<RecipientsList>> GetRecipientsAsync(RecipientFilter recipientFilter, ClaimsPrincipal claimsPrincipal)
        {
            var PartnerCode = claimsPrincipal?.Claims.FirstOrDefault(x => x.Type == "PartnerCode")?.Value;
            var data = await _recipentRepo.GetRecipientsAsync(recipientFilter, PartnerCode);
            return data;

        }

        public async Task<RecipientsList> GetRecipientsByIdAsync(int recipientid)
        {
            var data = await _recipentRepo.GetRecipientsByIdAsync(recipientid);
            return data;
        }

        public async Task<SprocMessage> UpdateRecipientAsync(RecipientAddUpdate recipientAdd, ClaimsPrincipal claimsPrincipal)
        {
            recipientAdd.OperationMode = "U";
            recipientAdd.LoggedInuser = claimsPrincipal?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            recipientAdd.UserType = claimsPrincipal?.Claims.FirstOrDefault(x => x.Type == "UserType")?.Value;
            var Response = await _recipentRepo.AddUpdateRecipientAsync(recipientAdd);
            return Response;
        }
    }
}
