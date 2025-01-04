using Mpmt.Core.Dtos.Paging;

namespace Mpmt.Core.Domain.Partners.Recipient
{
    public class RecipientFilter : PagedRequest
    {
        public int SenderId { get; set; }
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public int UserStatus { get; set; }
        public int Export { get; set; }
    }
}
