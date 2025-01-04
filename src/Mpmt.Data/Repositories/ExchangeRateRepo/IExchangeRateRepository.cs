using Mpmt.Core.Dtos.ConversionRate;
using Mpmt.Core.Dtos.WebCrawler;

namespace Mpmt.Data.Repositories.ExchangeRateRepo;

public interface IExchangeRateRepository
{
    Task UpdateExchangeRates(List<ExchangeRate> rates);
    Task<IEnumerable<ExchangeRateChangedListPartner>> FedanExchangeRates(List<FedanExchangeRate> rates);  
}
