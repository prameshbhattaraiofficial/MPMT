using Mpmt.Core.Dtos.FeeAccount;
using Mpmt.Core.Dtos.Paging;
using Mpmt.Core.Dtos.PartnerStatement;
using Mpmt.Core.Dtos.PrefundRequest;
using Mpmt.Core.Dtos.Roles;
using Mpmt.Core.Dtos.WalletLoad.Statement;
using Mpmts.Core.Dtos;

namespace Mpmt.Data.Repositories.PreFund
{
    /// <summary>
    /// The pre fund request repo.
    /// </summary>
    public interface IPreFundRequestRepo
    {
        /// <summary>
        /// Gets the pre fund request async.
        /// </summary>
        /// <param name="requestFilter">The request filter.</param>
        /// <returns>A Task.</returns>
        Task<PagedList<PreFundRequestDetails>> GetPreFundRequestAsync(PrefundRequestFilter requestFilter);
        /// <summary>
        /// Gets the pre fund request by id async.
        /// </summary>
        /// <param name="PrefundId">The prefund id.</param>
        /// <returns>A Task.</returns>
        Task<PreFundRequest> GetPreFundRequestByIdAsync(int PrefundId);
        /// <summary>
        /// Gets the pre fund request approved by id async.
        /// </summary>
        /// <param name="PrefundId">The prefund id.</param>
        /// <returns>A Task.</returns>
        Task<FundRequestApprovedView> GetPreFundRequestApprovedByIdAsync(int PrefundId);
        /// <summary>
        /// Changes the fund request status async.
        /// </summary>
        /// <param name="fundRequestStatus">The fund request status.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> ChangeFundRequestStatusAsync(FundRequestStatusUpdate fundRequestStatus);
        /// <summary>
        /// Gets the pre fund request approved async.
        /// </summary>
        /// <param name="requestFilter">The request filter.</param>
        /// <returns>A Task.</returns>
        Task<PagedList<FundRequestApprovedList>> GetPreFundRequestApprovedAsync(PrefundRequestFilter requestFilter);
        Task<SprocMessage> isSourceCurrencyNPR(string partnercode, string sourcecurrency,bool currStatus);
        Task<PagedList<PartnerWalletStatement>> GetSatementDetails(string walletcurrencyid, Statement model);
        Task<PagedList<FeeAccountStatement>> GetFeeAccountSatementDetails(FeeAccountStatementFilter model);
    }
}
