using Mpmt.Core.Dtos.Paging;

namespace Mpmt.Core.Dtos.PartnerReport;

public class RemitSettlementReportFilter : PagedRequest
{
    public string PartnerCode { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string SourceCurrency { get; set; }
    public string DestinationCurrency { get; set; }
    public string TransactionId { get; set; }
    public string PartnerName { get; set; }
    public string SignType { get; set; }
    public string TransactionType { get; set; }
    public string PaymentType { get; set; }
    public string MerchantMobileNo { get; set; }
    public string GatewayTxnId { get; set; }
    public string AgentTrackerId { get; set; }
    public string TrackerId { get; set; }
    public string Type { get; set; }
    public string UserType { get; set; }
    public string LoggedInUser { get; set; }
    public int Export { get; set; }
    public int DateFilterBy { get; set; }
    public string AgentCode { get; set; }
    public string DistrictCode { get; set; }
}
