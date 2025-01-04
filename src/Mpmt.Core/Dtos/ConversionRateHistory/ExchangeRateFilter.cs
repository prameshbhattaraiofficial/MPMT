using Mpmt.Core.Dtos.Paging;

namespace Mpmt.Core.Dtos.ConversionRateHistory;

public class ExchangeRateFilter : PagedRequest
{
    public string WalletCurrency { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int Export { get; set; }
    public string Wallet { get; set; }
}   
