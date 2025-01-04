using Mpmt.Core.Dtos.Paging;

namespace Mpmt.Core.Dtos.ActivityLog;
    
public class VendorApiLogFilter : PagedRequest
{
    public string PartnerCode { get; set; }
    public DateTime StartDate { get; set; } = DateTime.Now;
    public DateTime EndDate { get; set; } = DateTime.Now;
    public string TransactionId { get; set; }
    public string TrackerId { get; set; }
    public string UserType { get; set; }
    public string LoggedInUser { get; set; }
}
