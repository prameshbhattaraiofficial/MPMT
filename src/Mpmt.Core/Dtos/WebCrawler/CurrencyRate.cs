namespace Mpmt.Core.Dtos.WebCrawler;

public class CurrencyRate
{
    public string CurrencyCode { get; set; }
    public int Unit { get; set; }
    public double BuyingRate { get; set; }
    public double Rate10 { get; set; }
    public double Rate14 { get; set; }

    //Empty Constructor Required
    public CurrencyRate()
    {
    }

    public CurrencyRate(string currencyCode, int unit, double buyingRate)
    {
        CurrencyCode = currencyCode;
        Unit = unit;
        BuyingRate = buyingRate;
    }

    public CurrencyRate(string currencyCode, int unit, double buyingRate, double rate10, double rate14)
    {
        CurrencyCode = currencyCode;
        Unit = unit;
        BuyingRate = buyingRate;
        Rate10 = rate10;
        Rate14 = rate14;
    }
}
