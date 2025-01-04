using Mpmt.Core.Domain.Partners.Senders;
using Mpmt.Core.Dtos;
using Mpmt.Core.Dtos.Paging;
using Mpmt.Core.Dtos.Transaction;
using Mpmt.Core.ViewModel.User;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.Partner
{
    public interface IPartnerSenderService
    {
        Task<PagedList<SenderDto>> GetSenderListAsync(SenderPagedRequest request);
        Task<SenderDto> GetSenderByIdAsync(int senderId,string PartnerCode);
        Task<IEnumerable<ExistingSender>> GetExistingSendersByPartnercode(string PartnerCode, string MemberId = "", string FullName = "");
        Task<IEnumerable<ExistingRecipients>> GetExistingRecipientsByPartnercode(string MemberId);
        Task<MpmtResult> AddSenderAsync(AddUserViewModel sender);
        Task<MpmtResult> UpdateSenderAsync(UpdateUserVM sender);
        Task<SprocMessage> RemoveSenderAsync(int sender);
      
    }
}
