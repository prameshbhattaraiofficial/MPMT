namespace Mpmt.Core.Dtos.ConversionRateHistory;

public class ExchangeRateHistoryDetails
{
    public string SN { get; set; }
    public string Wallet { get; set; }
    public decimal UnitValue { get; set; }
    public decimal BuyingRate { get; set; }
    public decimal SellingRate { get; set; }
    public decimal CurrentRate { get; set; }    
    public string AddedBy { get; set; }
    public string AddedDate { get; set; }
    public string AddedDateNST { get; set; }
    public string UpdatedBy { get; set; }
    public string UpdatedDate { get; set; }
    public string UpdatedDateNST { get; set; }
}
