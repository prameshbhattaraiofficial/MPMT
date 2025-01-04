using Mpmt.Core.Dtos.ConversionRate;
using Mpmt.Core.Dtos.WebCrawler;

namespace Mpmt.Services.Services.ExchangeRateService;

public interface IExchangeRateService
{
    Task UpdateExchangeRates(List<ExchangeRate> rates);
    Task<IEnumerable<ExchangeRateChangedListPartner>> FedanExchangeRates(List<FedanExchangeRate> rates);  
}
