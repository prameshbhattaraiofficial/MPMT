using Mpmt.Core.Dtos;
using Mpmt.Core.Dtos.Partner;
using Mpmts.Core.Dtos;
using System.Security.Claims;

namespace Mpmt.Services.Partner.IService
{
    /// <summary>
    /// The partner wallet currency services.
    /// </summary>
    public interface IPartnerWalletCurrencyServices
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
        /// <param name="user">The user.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> AddWalletCurrencyAsync(IUDUpdateWalletCurrency addUpdateWalletcurrency, ClaimsPrincipal user);
        Task<SprocMessage> AddFeeWalletAsync(IUDUpdateWalletCurrency addUpdateWalletcurrency, ClaimsPrincipal user);

        /// <summary>
        /// Removes the wallet currency async.
        /// </summary>
        /// <param name="addUpdateWalletcurrency">The add update walletcurrency.</param>
        /// <param name="user">The user.</param>
        /// <returns>A Task.</returns>

        Task<SprocMessage> UpdateWalletCurrencyAsync(IUDUpdateWalletCurrency addUpdateWalletcurrency, ClaimsPrincipal user);
        Task<SprocMessage> UpdateFeeWalletAsync(IUDUpdateWalletCurrency addUpdateWalletcurrency, ClaimsPrincipal user);

        /// <summary>
        /// Removes the wallet currency async.
        /// </summary>
        /// <param name="addUpdateWalletcurrency">The add update walletcurrency.</param>
        /// <param name="user">The user.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> RemoveWalletCurrencyAsync(IUDUpdateWalletCurrency addUpdateWalletcurrency, ClaimsPrincipal user);
        /// <summary>
        /// Adds the fund request async.
        /// </summary>
        /// <param name="addUpdateFundRequest">The add update fund request.</param>
        /// <param name="user">The user.</param>
        /// <returns>A Task.</returns>
        Task<MpmtResult> AddFundRequestAsync(AddUpdateFundRequest addUpdateFundRequest, ClaimsPrincipal user);

        Task<SprocMessage> AddFeeBalanceAsync(AddUpdateFundRequest addUpdateFundRequest, ClaimsPrincipal user);
        /// <summary>
        /// Updates the fund request async.
        /// </summary>
        /// <param name="addUpdateFundRequest">The add update fund request.</param>
        /// <param name="user">The user.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> UpdateFundRequestAsync(AddUpdateFundRequest addUpdateFundRequest, ClaimsPrincipal user);
    }
}
