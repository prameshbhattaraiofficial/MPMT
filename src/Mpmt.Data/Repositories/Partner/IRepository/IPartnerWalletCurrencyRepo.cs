using Mpmt.Core.Dtos.Partner;
using Mpmts.Core.Dtos;

namespace Mpmt.Data.Repositories.Partner.IRepository
{
    /// <summary>
    /// The partner wallet currency repo.
    /// </summary>
    public interface IPartnerWalletCurrencyRepo
    {
        /// <summary>
        /// Gets the partner wallet currency.
        /// </summary>
        /// <param name="partnercode">The partnercode.</param>
        /// <returns>A Task.</returns>
        Task<IEnumerable<WalletCurrencyDetails>> GetPartnerWalletCurrency(string partnercode);
        Task<IEnumerable<WalletCurrencyBalance>> GetPartnerWalletCurrencyBalance(string partnercode);
        /// <summary>
        /// Gets the partner wallet currency by id.
        /// </summary>
        /// <param name="Id">The id.</param>
        /// <returns>A Task.</returns>
        Task<WalletCurrencyDetails> GetPartnerWalletCurrencyById(int Id);
        Task<WalletCurrencyDetails> GetFeeWalletCurrencyById(int Id);

        /// <summary>
        /// Adds the wallet currency async.
        /// </summary>
        /// <param name="addUpdateWalletcurrency">The add update walletcurrency.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> AddWalletCurrencyAsync(IUDUpdateWalletCurrency addUpdateWalletcurrency);
        /// <summary>
        /// Removes the wallet currency async.
        /// </summary>
        /// <param name="addUpdateWalletcurrency">The add update walletcurrency.</param>
        /// <returns>A Task.</returns>

        Task<SprocMessage> AddUpdateWalletCurrencyAsync(IUDUpdateWalletCurrency addUpdateWalletcurrency);
        Task<SprocMessage> AddUpdateFeeWalletAsync(IUDUpdateWalletCurrency addUpdateWalletcurrency);

        /// <summary>
        /// Removes the wallet currency async.
        /// </summary>
        /// <param name="addUpdateWalletcurrency">The add update walletcurrency.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> RemoveWalletCurrencyAsync(IUDUpdateWalletCurrency addUpdateWalletcurrency);

        /// <summary>
        /// Adds the update fund request async.
        /// </summary>
        /// <param name="addUpdateFundRequest">The add update fund request.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> AddUpdateFundRequestAsync(AddUpdateFundRequest addUpdateFundRequest);
        Task<SprocMessage> AddFeeBalanceAsync(AddUpdateFundRequest addUpdateFundRequest);

    }
}
