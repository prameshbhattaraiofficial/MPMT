using Mpmt.Core.Dtos.FeeAccount;
using Mpmt.Core.Dtos.Paging;
using Mpmt.Core.Dtos.Partner;
using Mpmt.Core.Dtos.PartnerStatement;
using Mpmt.Core.Dtos.PrefundRequest;
using Mpmt.Core.Dtos.Roles;
using Mpmt.Core.Dtos.WalletLoad.Statement;
using Mpmt.Data.Repositories.PreFund;
using Mpmts.Core.Dtos;
using System.Security.Claims;

namespace Mpmt.Services.Services.PreFund
{
    /// <summary>
    /// The pre fund request services.
    /// </summary>
    public class PreFundRequestServices : IPreFundRequestServices
    {
        private readonly IPreFundRequestRepo _requestRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="PreFundRequestServices"/> class.
        /// </summary>
        /// <param name="requestRepo">The request repo.</param>
        public PreFundRequestServices(IPreFundRequestRepo requestRepo)
        {
            _requestRepo = requestRepo;
        }


        /// <summary>
        /// Approveds the fund request status async.
        /// </summary>
        /// <param name="fundRequestStatus">The fund request status.</param>
        /// <param name="User">The user.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> ApprovedFundRequestStatusAsync(FundRequestStatusUpdate fundRequestStatus, ClaimsPrincipal User)
        {
            fundRequestStatus.OperationMode = "APPROVE";
            fundRequestStatus.LoggedInUser = User?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            fundRequestStatus.UserType = User?.Claims.FirstOrDefault(x => x.Type == "UserType")?.Value;
            var response = await _requestRepo.ChangeFundRequestStatusAsync(fundRequestStatus);
            return response;
        }

        public async Task<PagedList<FeeAccountStatement>> GetFeeAccountSatementDetails(FeeAccountStatementFilter model)
        {
            var data = await _requestRepo.GetFeeAccountSatementDetails(model);
            return data;
        }

        /// <summary>
        /// Gets the pre fund request approved async.
        /// </summary>
        /// <param name="requestFilter">The request filter.</param>
        /// <returns>A Task.</returns>
        public async Task<PagedList<FundRequestApprovedList>> GetPreFundRequestApprovedAsync(PrefundRequestFilter requestFilter)
        {
            var data = await _requestRepo.GetPreFundRequestApprovedAsync(requestFilter);
            return data;
        }

        /// <summary>
        /// Gets the pre fund request approved by id async.
        /// </summary>
        /// <param name="Prefund">The prefund.</param>
        /// <returns>A Task.</returns>
        public async Task<FundRequestApprovedView> GetPreFundRequestApprovedByIdAsync(int Prefund)
        {
            var data = await _requestRepo.GetPreFundRequestApprovedByIdAsync(Prefund);
            return data;
        }

        /// <summary>
        /// Gets the pre fund request async.
        /// </summary>
        /// <param name="requestFilter">The request filter.</param>
        /// <returns>A Task.</returns>
        public async Task<PagedList<PreFundRequestDetails>> GetPreFundRequestAsync(PrefundRequestFilter requestFilter)
        {
            var data = await _requestRepo.GetPreFundRequestAsync(requestFilter);
            return data;
        }

        /// <summary>
        /// Gets the pre fund request by id async.
        /// </summary>
        /// <param name="PrefundId">The prefund id.</param>
        /// <returns>A Task.</returns>
        public async Task<PreFundRequest> GetPreFundRequestByIdAsync(int PrefundId)
        {
            var data = await _requestRepo.GetPreFundRequestByIdAsync(PrefundId);
            return data;
        }

        public async Task<PagedList<PartnerWalletStatement>> GetSatementDetails(string walletcurrencyid, Statement model)
        {
            var data = await _requestRepo.GetSatementDetails(walletcurrencyid,model);
            return data;
        }

        public async Task<SprocMessage> isSourceCurrencyNPR(string partnercode, string sourcecurrency, bool currStatus)
        {
            var data = await _requestRepo.isSourceCurrencyNPR(partnercode, sourcecurrency, currStatus);
            return data;
        }

        /// <summary>
        /// Rejects the fund request status async.
        /// </summary>
        /// <param name="fundRequestStatus">The fund request status.</param>
        /// <param name="User">The user.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> RejectFundRequestStatusAsync(FundRequestStatusUpdate fundRequestStatus, ClaimsPrincipal User)
        {
            fundRequestStatus.OperationMode = "REJECT";
            fundRequestStatus.LoggedInUser = User?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            fundRequestStatus.UserType = User?.Claims.FirstOrDefault(x => x.Type == "UserType")?.Value;
            var response = await _requestRepo.ChangeFundRequestStatusAsync(fundRequestStatus);
            return response;
        }
    }
}
