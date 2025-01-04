using Mpmt.Core.Dtos.Currency;
using Mpmts.Core.Dtos;

namespace Mpmt.Data.Repositories.Currency
{
    /// <summary>
    /// The currency repo.
    /// </summary>
    public interface ICurrencyRepo
    {
        /// <summary>
        /// Adds the currency async.
        /// </summary>
        /// <param name="addCurrency">The add currency.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> AddCurrencyAsync(IUDCurrency addCurrency);
        /// <summary>
        /// Gets the currency by id async.
        /// </summary>
        /// <param name="CurrencyId">The currency id.</param>
        /// <returns>A Task.</returns>
        Task<CurrencyDetails> GetCurrencyByIdAsync(int CurrencyId);
        /// <summary>
        /// Updates the currency async.
        /// </summary>
        /// <param name="updateCurrency">The update currency.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> UpdateCurrencyAsync(IUDCurrency updateCurrency);
        /// <summary>
        /// Removes the currency async.
        /// </summary>
        /// <param name="RemoveCurrency">The remove currency.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> RemoveCurrencyAsync(IUDCurrency RemoveCurrency);
        /// <summary>
        /// Gets the currency async.
        /// </summary>
        /// <returns>A Task.</returns>
        Task<IEnumerable<CurrencyDetails>> GetCurrencyAsync();
    }
}
