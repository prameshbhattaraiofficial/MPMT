using Mpmt.Core.Dtos.ConversionRate;
using Mpmt.Core.Dtos.ConversionRateHistory;
using Mpmt.Core.Dtos.Paging;
using Mpmt.Core.Dtos.WebCrawler;
using Mpmts.Core.Dtos;

namespace Mpmt.Data.Repositories.ConversionRate
{
    /// <summary>
    /// The conversion rate repo.
    /// </summary>
    public interface IConversionRateRepo
    {
        /// <summary>
        /// Gets the conversion rate async.
        /// </summary>
        /// <param name="conversionRateFilter">The conversion rate filter.</param>
        /// <returns>A Task.</returns>
        Task<IEnumerable<ConversionRateDetails>> GetConversionRateAsync(ConversionRateFilter conversionRateFilter);
        Task<PagedList<ExchangeRateHistoryDetails>> GetExchangeRateHistoryAsync(ExchangeRateFilter exchangeRateFilter);   
        /// <summary>
        /// Gets the conversion rate by id async.
        /// </summary>
        /// <param name="conversionRateId">The conversion rate id.</param>
        /// <returns>A Task.</returns>
        Task<ConversionRateDetails> GetConversionRateByIdAsync(int conversionRateId);
        /// <summary>
        /// Adds the conversion rate async.
        /// </summary>
        /// <param name="addConversionRate">The add conversion rate.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> AddConversionRateAsync(IUDConversionRate addConversionRate);
        /// <summary>
        /// Updates the conversion rate async.
        /// </summary>
        /// <param name="updateConversionRate">The update conversion rate.</param>
        /// <returns>A Task.</returns>
        Task<(SprocMessage, IEnumerable<ExchangeRateChangedListPartner>)> UpdateConversionRateAsync(IUDConversionRate updateConversionRate);
        /// <summary>
        /// Removes the conversion rate async.
        /// </summary>
        /// <param name="removeConversionRate">The remove conversion rate.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> RemoveConversionRateAsync(IUDConversionRate removeConversionRate);
    }
}
