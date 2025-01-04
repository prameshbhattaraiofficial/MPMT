using Mpmt.Core.Dtos.ConversionRate;
using Mpmt.Core.Dtos.WebCrawler;
using Mpmt.Data.Repositories.ExchangeRateRepo;

namespace Mpmt.Services.Services.ExchangeRateService;

public class ExchangeRateService : IExchangeRateService
{
    private readonly IExchangeRateRepository _exchangeRateRepository;

    public ExchangeRateService(IExchangeRateRepository exchangeRateRepository)
    {
        _exchangeRateRepository = exchangeRateRepository;
    }

    public async Task<IEnumerable<ExchangeRateChangedListPartner>> FedanExchangeRates(List<FedanExchangeRate> rates)
    {
        var result = await _exchangeRateRepository.FedanExchangeRates(rates);
        return result;
    }

    public async Task UpdateExchangeRates(List<ExchangeRate> rates)
    {
        await _exchangeRateRepository.UpdateExchangeRates(rates);
    }
}
