namespace Mpmt.Core.Dtos.FeeFundRequest;

public class FeeAccount
{

    public string SN { get; set; }
    public int Id { get; set; }
    public string PartnerCode { get; set; }
    public string SourceCurrency { get; set; }
    public string Balance { get; set; }
    public decimal CreditLimit { get; set; }
    public string Ledger { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}
