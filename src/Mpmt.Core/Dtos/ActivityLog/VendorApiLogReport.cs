namespace Mpmt.Core.Dtos.ActivityLog;
    
public class VendorApiLogReport
{
    public int SN { get; set; }
    public string TransactionDate { get; set; }
    public string TrackerId { get; set; }
    public string LogId { get; set; }
    public string RequestInput { get; set; }
    public string ResponseOutput { get; set; }
    public string ResponseHttpStatus { get; set; }
    public string RequestUrl { get; set; }
    public string DeviceCode { get; set; }
    public string PartnerCode { get; set; }
    public string IpAddress { get; set; }
    public string MachineName { get; set; }
    public string Environment { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}
