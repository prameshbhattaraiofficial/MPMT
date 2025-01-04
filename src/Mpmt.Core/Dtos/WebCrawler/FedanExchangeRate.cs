namespace Mpmt.Core.Dtos.WebCrawler;

public class FedanExchangeRate
{
    public string CurrencyCode { get; set; }
    public string Symbol { get; set; }
    public string Unit { get; set; }
    public string Rate10 { get; set; }
    public string Rate14 { get; set; }

    public FedanExchangeRate()
    {
    }

    public FedanExchangeRate(string currencyCode, string symbol, string unit, string rate10, string rate14)
    {
        CurrencyCode = currencyCode;
        Symbol = symbol;
        Unit = unit;
        Rate10 = rate10;
        Rate14 = rate14;
    }
}

public class ExchangeRateChangedListPartner
{
    public string PartnerCode { get; set; }
    public string PartnerName { get; set; }
    public string Email { get; set; }
    public string Currency { get; set; }    
    public decimal PrevRate { get; set; }    
    public decimal CurrRate { get; set; }    
    public DateTime UpdatedDate { get; set; }       
}
