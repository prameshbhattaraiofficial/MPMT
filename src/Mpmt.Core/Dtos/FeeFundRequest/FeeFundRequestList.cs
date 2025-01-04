namespace Mpmt.Core.Dtos.FeeFundRequest;

public class FeeFundRequestList
{
    public int Id { get; set; }
    public int Sn { get; set; }
    public string PartnerCode { get; set; }
    public string FundType { get; set; }
    public string SourceCurrency { get; set; }
    public string DestinationCurrency { get; set; }
    public double Amount { get; set; }
    public string Remarks { get; set; }
    public string Sign { get; set; }
    public string TransactionId { get; set; }
    public string RequestStatus { get; set; }
    public DateTime RegisteredDate { get; set; }
    public string VoucherImgPath { get; set; }
}
