using AutoMapper;
using Dapper;
using DocumentFormat.OpenXml.EMMA;
using Mpmt.Core.Domain.Partners.SendTransactions;
using Mpmt.Core.Dtos.FeeAccount;
using Mpmt.Core.Dtos.Paging;
using Mpmt.Core.Dtos.PartnerStatement;
using Mpmt.Core.Dtos.PrefundRequest;
using Mpmt.Core.Dtos.Roles;
using Mpmt.Core.Dtos.WalletLoad.Statement;
using Mpmt.Data.Common;
using Mpmts.Core.Dtos;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Data;

namespace Mpmt.Data.Repositories.PreFund
{
    /// <summary>
    /// The pre fund request repo.
    /// </summary>
    public class PreFundRequestRepo : IPreFundRequestRepo
    {
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PreFundRequestRepo"/> class.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        public PreFundRequestRepo(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Changes the fund request status async.
        /// </summary>
        /// <param name="fundRequestStatus">The fund request status.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> ChangeFundRequestStatusAsync(FundRequestStatusUpdate fundRequestStatus)
        {
            try
            {

                using var connection = DbConnectionManager.GetDefaultConnection();

                var param = new DynamicParameters();
                param.Add("@OperationMode", fundRequestStatus.OperationMode);
                param.Add("@FundRequestId", fundRequestStatus.FundRequestId);
                param.Add("@Remarks", fundRequestStatus.Remarks);   
                param.Add("@LoggedInUser", fundRequestStatus.LoggedInUser);
                param.Add("@UserType", fundRequestStatus.UserType);


                param.Add("@ReturnPrimaryId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
                param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                _ = await connection.ExecuteAsync("[dbo].[usp_prefund_request_status_update]", param, commandType: CommandType.StoredProcedure);

                var identityVal = param.Get<int>("@ReturnPrimaryId");
                var statusCode = param.Get<int>("@StatusCode");
                var msgType = param.Get<string>("@MsgType");
                var msgText = param.Get<string>("@MsgText");

                return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<PagedList<FeeAccountStatement>> GetFeeAccountSatementDetails(FeeAccountStatementFilter model)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@PartnerCode", model.PartnerCode);
            param.Add("@WalletCurrency", model.WalletCurrency);
            param.Add("@DateFlag", model.DateFlag);
            param.Add("@StartDate", model.StartDate);
            param.Add("@EndDate", model.EndDate);

            param.Add("@PageNumber", model.PageNumber);
            param.Add("@PageSize", model.PageSize);
            param.Add("@SortingCol", model.SortBy);
            param.Add("@SortType", model.SortOrder);
            param.Add("@SearchVal", model.SearchVal);


            var data = await connection
                .QueryMultipleAsync("[dbo].[usp_get_partner_fee_account_statements]", param: param, commandType: CommandType.StoredProcedure);

            var prefundList = await data.ReadAsync<FeeAccountStatement>();
            var pagedInfo = await data.ReadFirstAsync<PageInfo>();
            var mappeddata = _mapper.Map<PagedList<FeeAccountStatement>>(pagedInfo);
            mappeddata.Items = prefundList;
            return mappeddata;
        }

        public async Task<PagedList<FundRequestApprovedList>> GetPreFundRequestApprovedAsync(PrefundRequestFilter requestFilter)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@PartnerCode", requestFilter.PartnerCode);
            param.Add("@TxnId", requestFilter.TxnId);
            param.Add("@StartDate", requestFilter.StartDate);
            param.Add("@EndDate", requestFilter.EndDate);
            param.Add("@PageNumber", requestFilter.PageNumber);
            param.Add("@PageSize", requestFilter.PageSize);
            param.Add("@SortingCol", requestFilter.SortBy);
            param.Add("@SortType", requestFilter.SortOrder);
            param.Add("@SearchVal", requestFilter.SearchVal);
            param.Add("@Export", requestFilter.Export);
            var data = await connection
                .QueryMultipleAsync("[dbo].[sp_get_fundrequestapproved_list]", param: param, commandType: CommandType.StoredProcedure);

            var prefundList = await data.ReadAsync<FundRequestApprovedList>();
            var pagedInfo = await data.ReadFirstAsync<PageInfo>();
            var mappeddata = _mapper.Map<PagedList<FundRequestApprovedList>>(pagedInfo);
            mappeddata.Items = prefundList;
            return mappeddata;
        }

        public async Task<FundRequestApprovedView> GetPreFundRequestApprovedByIdAsync(int PrefundId)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@Id", PrefundId);

            var data = await connection
                .QueryFirstOrDefaultAsync<FundRequestApprovedView>("[dbo].[usp_get_prefundrequestapproved_byid]", param: param, commandType: CommandType.StoredProcedure);
            return data;
        }


        public async Task<PagedList<PreFundRequestDetails>> GetPreFundRequestAsync(PrefundRequestFilter requestFilter)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@PartnerCode", requestFilter.PartnerCode);
            //param.Add("@TxnId", requestFilter.TxnId);
            param.Add("@StartDate", requestFilter.StartDate);
            param.Add("@EndDate", requestFilter.EndDate);
            param.Add("@Status", requestFilter.Status);
            param.Add("@PageNumber", requestFilter.PageNumber);
            param.Add("@PageSize", requestFilter.PageSize);
            param.Add("@SortingCol", requestFilter.SortBy);
            param.Add("@SortType", requestFilter.SortOrder);
            param.Add("@SearchVal", requestFilter.SearchVal);
            param.Add("@Export", requestFilter.Export);
            var data = await connection
                .QueryMultipleAsync("[dbo].[sp_get_fundrequest_list]", param: param, commandType: CommandType.StoredProcedure);

            var prefundList = await data.ReadAsync<PreFundRequestDetails>();
            var pagedInfo = await data.ReadFirstAsync<PageInfo>();
            var mappeddata = _mapper.Map<PagedList<PreFundRequestDetails>>(pagedInfo);
            mappeddata.Items = prefundList;
            return mappeddata;
        }

        /// <summary>
        /// Gets the pre fund request by id async.
        /// </summary>
        /// <param name="PrefundId">The prefund id.</param>
        /// <returns>A Task.</returns>
        public async Task<PreFundRequest> GetPreFundRequestByIdAsync(int PrefundId)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@Id", PrefundId);

            var data = await connection
                .QueryFirstOrDefaultAsync<PreFundRequest>("[dbo].[usp_get_prefundrequest_byid]", param: param, commandType: CommandType.StoredProcedure);
            return data;
        }

        public async Task<PagedList<PartnerWalletStatement>> GetSatementDetails(string walletcurrencyid, Statement model)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@PartnerCode", model.Partnercode);
            param.Add("@WalletCurrency",model.walletCurrencyById);
            param.Add("@DateFlag", model.DateFlag);
            param.Add("@StartDate", model.StartDate);
            param.Add("@EndDate", model.EndDate);
            param.Add("@Export", model.Export);

            param.Add("@PageNumber", model.PageNumber);
            param.Add("@PageSize", model.PageSize);
            param.Add("@SortingCol", model.SortBy);
            param.Add("@SortType", model.SortOrder);
            param.Add("@SearchVal", model.SearchVal);


            var data = await connection
                .QueryMultipleAsync("[dbo].[usp_get_partner_wallet_statements]", param: param, commandType: CommandType.StoredProcedure);

            var prefundList = await data.ReadAsync<PartnerWalletStatement>();
            var pagedInfo = await data.ReadFirstAsync<PageInfo>();
            var mappeddata = _mapper.Map<PagedList<PartnerWalletStatement>>(pagedInfo);
            mappeddata.Items = prefundList;
            return mappeddata;
        }

        public async Task<SprocMessage> isSourceCurrencyNPR(string partnercode, string sourcecurrency, bool currStatus)
        {
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();

                var param = new DynamicParameters();
                param.Add("@PartnerCode", partnercode);
                param.Add("@SourceCurrency", sourcecurrency);
                param.Add ("@IsNPRSourceCurrency", currStatus);


                param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
                param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                _ = await connection.ExecuteAsync("[dbo].[usp_update_transaction_currency]", param, commandType: CommandType.StoredProcedure);
               
                var statusCode = param.Get<int>("@StatusCode");
                var msgType = param.Get<string>("@MsgType");
                var msgText = param.Get<string>("@MsgText");

                return new SprocMessage {StatusCode = statusCode, MsgType = msgType, MsgText = msgText };

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            throw new NotImplementedException();
        }
    }
}
