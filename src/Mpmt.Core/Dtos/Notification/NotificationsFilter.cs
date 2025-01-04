using Mpmt.Core.Dtos.Paging;

namespace Mpmt.Core.Dtos.Notification
{
    public class NotificationsFilter : PagedRequest
    {
        public string PartnerCode { get; set; }
        public string UserType { get; set; }
        public int Export { get; set; }
    }
}
