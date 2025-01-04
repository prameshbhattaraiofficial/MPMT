using Mpmt.Core.Domain.Partners.Senders;
using Mpmt.Core.Dtos.Paging;
using Mpmts.Core.Dtos;

namespace Mpmt.Data.Repositories.Partner
{
    public interface IPartnerSenderRepository
    {
        Task<PagedList<SenderDto>> GetSendersAsync(SenderPagedRequest request);
        Task<SenderDto> GetSenderByIdAsync(int SenderId,string PartnerCode);
        Task<IEnumerable<ExistingSender>> GetExistingSendersByPartnercodeAsync(string PartnerCode, string MemberId, string FullName);
        Task<IEnumerable<ExistingRecipients>> GetExistingRecipientsByPartnercodeAsync(string MemberId);

        Task<SprocMessage> AddSenderAsync(SenderAddUpdateDto sender);
        Task<SprocMessage> UpdateSenderAsync(SenderAddUpdateDto sender);
        Task<SprocMessage> RemoveSenderAsync(SenderAddUpdateDto sender);
    }
}
