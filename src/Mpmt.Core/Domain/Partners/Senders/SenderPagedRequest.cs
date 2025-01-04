using Mpmt.Core.Dtos.Paging;

namespace Mpmt.Core.Domain.Partners.Senders
{
    public class SenderPagedRequest : PagedRequest
    {
        public string PartnerCode { get; set; }
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public int UserStatus { get; set; }
        public int Export { get; set; }
    }
}
