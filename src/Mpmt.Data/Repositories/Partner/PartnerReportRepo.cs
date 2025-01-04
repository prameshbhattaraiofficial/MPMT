using AutoMapper;
using Dapper;
using Mpmt.Core.Dtos.InwardRemitanceReport;
using Mpmt.Core.Dtos.Paging;
using Mpmt.Core.Dtos.PartnerReport;
using Mpmt.Core.Dtos.Transaction;
using Mpmt.Data.Common;
using System.Data;
using System.Runtime.CompilerServices;

namespace Mpmt.Data.Repositories.Partner
{
    public class PartnerReportRepo : IPartnerReportRepo
    {
        private readonly IMapper _mapper;

        public PartnerReportRepo(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<PagedList<RemitTxnReport>> GetRemitTxnReportAsync(RemitTxnReportFilter txnFilter)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@PartnerCode", txnFilter.PartnerCode);
            param.Add("@DateFilterBy", txnFilter.DateFilterBy);
            param.Add("@StartDate", txnFilter.StartDate);
            param.Add("@EndDate", txnFilter.EndDate);
            param.Add("@SourceCurrency", txnFilter.SourceCurrency);
            param.Add("@DestinationCurrency", txnFilter.DestinationCurrency);
            param.Add("@TransactionId", txnFilter.TransactionId);
            param.Add("@TransactionStatus", txnFilter.TransactionStatus);
            param.Add("@SignType", txnFilter.SignType);
            param.Add("@TransactionType", txnFilter.TransactionType);
            param.Add("@ComplianceStatus", txnFilter.ComplianceStatus);
            param.Add("@PartnerName", txnFilter.PartnerName);
            param.Add("@GatewayTxnId", txnFilter.GatewayTxnId);
            param.Add("@AgentTrackerId", txnFilter.AgentTrackerId);
            param.Add("@TrackerId", txnFilter.TrackerId);
            param.Add("@mtcnNumber", txnFilter.ControlNumber);
            param.Add("@PaymentType", txnFilter.PaymentType);
            param.Add("@UserType", txnFilter.UserType);
            param.Add("@LoggedInUser", txnFilter.LoggedInUser);
            param.Add("@PageNumber", txnFilter.PageNumber);
            param.Add("@PageSize", txnFilter.PageSize);
            param.Add("@SortingCol", txnFilter.SortOrder);
            param.Add("@SortType", txnFilter.SortBy);
            param.Add("@SearchVal", txnFilter.SearchVal);
            param.Add("@Export", txnFilter.Export);
            var data = await connection
                .QueryMultipleAsync("[dbo].[usp_get_remit_transaction_report]", param: param, commandType: CommandType.StoredProcedure);

            var txnlist = await data.ReadAsync<RemitTxnReport>();
            var pagedInfo = await data.ReadFirstAsync<PageInfo>();
            var mappeddata = _mapper.Map<PagedList<RemitTxnReport>>(pagedInfo);
            mappeddata.Items = txnlist;
            return mappeddata;
        }

        public async Task<PagedList<RemitSettlementReport>> GetRemitSettlementReportAsync(RemitSettlementReportFilter txnFilter)
        {
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();

                var param = new DynamicParameters();
                param.Add("@PartnerCode", txnFilter.PartnerCode);
                param.Add("@StartDate", txnFilter.StartDate);
                param.Add("@EndDate", txnFilter.EndDate);
                param.Add("@SourceCurrency", txnFilter.SourceCurrency);
                param.Add("@DestinationCurrency", txnFilter.DestinationCurrency);
                param.Add("@TransactionId", txnFilter.TransactionId);
                param.Add("@SignType", txnFilter.SignType);
                param.Add("@TransactionType", txnFilter.TransactionType);

                param.Add("@MerchantMobileNo", txnFilter.MerchantMobileNo);
                param.Add("@GatewayTxnId", txnFilter.GatewayTxnId);
                param.Add("@TrackerId", txnFilter.TrackerId);
                param.Add("@Type", txnFilter.Type);

                param.Add("@Export", txnFilter.Export);

                param.Add("@UserType", txnFilter.UserType);
                param.Add("@LoggedInUser", txnFilter.LoggedInUser);
                param.Add("@PageNumber", txnFilter.PageNumber);
                param.Add("@PageSize", txnFilter.PageSize);
                param.Add("@SortingCol", txnFilter.SortOrder);
                param.Add("@SortType", txnFilter.SortBy);
                param.Add("@SearchVal", txnFilter.SearchVal);
                var data = await connection
                    .QueryMultipleAsync("[dbo].[usp_get_remit_settlement_report]", param: param, commandType: CommandType.StoredProcedure);

                var txnlist = await data.ReadAsync<RemitSettlementReport>();
                var pagedInfo = await data.ReadFirstAsync<PageInfo>();
                var mappeddata = _mapper.Map<PagedList<RemitSettlementReport>>(pagedInfo);
                mappeddata.Items = txnlist;
                return mappeddata;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<PagedList<RemitSettlementReport>> GetSettlementReportAsync(RemitSettlementReportFilter txnFilter)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@PartnerCode", txnFilter.PartnerCode);
            param.Add("@DateFilterBy", txnFilter.DateFilterBy);
            param.Add("@StartDate", txnFilter.StartDate);
            param.Add("@EndDate", txnFilter.EndDate);
            param.Add("@SourceCurrency", txnFilter.SourceCurrency);
            param.Add("@DestinationCurrency", txnFilter.DestinationCurrency);
            param.Add("@TransactionId", txnFilter.TransactionId);
            param.Add("@PartnerName", txnFilter.PartnerName);
            param.Add("@SignType", txnFilter.SignType);
            param.Add("@TransactionType", txnFilter.TransactionType);
            param.Add("PaymentType", txnFilter.PaymentType);
            param.Add("@MerchantMobileNo", txnFilter.MerchantMobileNo);
            param.Add("@GatewayTxnId", txnFilter.GatewayTxnId);
            param.Add("@AgentTrackerId", txnFilter.AgentTrackerId);
            param.Add("@TrackerId", txnFilter.TrackerId);
            param.Add("@Type", txnFilter.Type);
            param.Add("@UserType", txnFilter.UserType);
            param.Add("@LoggedInUser", txnFilter.LoggedInUser);
            param.Add("@PageNumber", txnFilter.PageNumber);
            param.Add("@PageSize", txnFilter.PageSize);
            param.Add("@SortingCol", txnFilter.SortOrder);
            param.Add("@SortType", txnFilter.SortBy);
            param.Add("@SearchVal", txnFilter.SearchVal);
            param.Add("@Export", txnFilter.Export);
            var data = await connection
                .QueryMultipleAsync("[dbo].[usp_get_remit_settlement_detail_report]", param: param, commandType: CommandType.StoredProcedure);

            var txnlist = await data.ReadAsync<RemitSettlementReport>();
            var pagedInfo = await data.ReadFirstAsync<PageInfo>();
            var mappeddata = _mapper.Map<PagedList<RemitSettlementReport>>(pagedInfo);
            mappeddata.Items = txnlist;
            return mappeddata;
        }

        public async Task<PagedList<RemitFeeTxnReport>> GetRemitFeeTxnReportAsync(RemitTxnReportFilter txnFilter)
        {
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();

                var param = new DynamicParameters();
                param.Add("@PartnerCode", txnFilter.PartnerCode);
                param.Add("@StartDate", txnFilter.StartDate);
                param.Add("@EndDate", txnFilter.EndDate);
                param.Add("@SourceCurrency", txnFilter.SourceCurrency);
                param.Add("@DestinationCurrency", txnFilter.DestinationCurrency);
                param.Add("@TransactionId", txnFilter.TransactionId);
                param.Add("@SignType", txnFilter.SignType);
                param.Add("@TransactionType", txnFilter.TransactionType);

                param.Add("@PartnerName", txnFilter.PartnerName);
                param.Add("@GatewayTxnId", txnFilter.GatewayTxnId);
                param.Add("@TrackerId", txnFilter.TrackerId);

                param.Add("@Export", txnFilter.Export);

                param.Add("@UserType", txnFilter.UserType);
                param.Add("@LoggedInUser", txnFilter.LoggedInUser);
                param.Add("@PageNumber", txnFilter.PageNumber);
                param.Add("@PageSize", txnFilter.PageSize);
                param.Add("@SortingCol", txnFilter.SortOrder);
                param.Add("@SortType", txnFilter.SortBy);
                param.Add("@SearchVal", txnFilter.SearchVal);
                var data = await connection
                    .QueryMultipleAsync("[dbo].[usp_get_remit_feetransaction_report]", param: param, commandType: CommandType.StoredProcedure);

                var txnlist = await data.ReadAsync<RemitFeeTxnReport>();
                var pagedInfo = await data.ReadFirstAsync<PageInfo>();
                var mappeddata = _mapper.Map<PagedList<RemitFeeTxnReport>>(pagedInfo);
                mappeddata.Items = txnlist;
                return mappeddata;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PagedList<CommisionTransction>> GetRemitCommissionTxnReportAsync(CommissionTransactionFilter txnFilter)
        {
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();

                var param = new DynamicParameters();
                param.Add("@PartnerCode", txnFilter.PartnerCode);
                param.Add("@StartDate", txnFilter.StartDate);
                param.Add("@EndDate", txnFilter.EndDate);
                param.Add("@SourceCurrency", txnFilter.SourceCurrency);
                param.Add("@DestinationCurrency", txnFilter.DestinationCurrency);
                param.Add("@TransactionId", txnFilter.TransactionId);
                param.Add("@SignType", txnFilter.SignType);
                param.Add("@TransactionType", txnFilter.TransactionType);

                param.Add("@Export", txnFilter.Export);

                param.Add("@UserType", txnFilter.UserType);
                param.Add("@LoggedInUser", txnFilter.LoggedInUser);
                param.Add("@PageNumber", txnFilter.PageNumber);
                param.Add("@PageSize", txnFilter.PageSize);
                param.Add("@SortingCol", txnFilter.SortOrder);
                param.Add("@SortType", txnFilter.SortBy);
                param.Add("@SearchVal", txnFilter.SearchVal);
                var data = await connection
                    .QueryMultipleAsync("[dbo].[usp_get_remit_Commission_transaction_report]", param: param, commandType: CommandType.StoredProcedure);

                var txnlist = await data.ReadAsync<CommisionTransction>();
                var pagedInfo = await data.ReadFirstAsync<PageInfo>();
                var mappeddata = _mapper.Map<PagedList<CommisionTransction>>(pagedInfo);
                mappeddata.Items = txnlist;
                return mappeddata;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<(SumofTransaction, PagedList<InwardRemitanceDto>)> GetInwardRemitanceReport(InwardRemitanceFilterDto txnFilter)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@CountryCode", txnFilter.CountryCode);
            param.Add("@DateFlag", txnFilter.DateFlag);
            param.Add("@StartDate", txnFilter.StartDate);
            param.Add("@EndDate", txnFilter.EndDate);
            param.Add("@NprToUsdRate", txnFilter.NprToUsdRate);
            param.Add("@frequency", txnFilter.Frequency);

            //param.Add("@Export", txnFilter.Export);

            param.Add("@PageNumber", txnFilter.PageNumber);
            param.Add("@PageSize", txnFilter.PageSize);
            param.Add("@SortingCol", txnFilter.SortOrder);
            param.Add("@SortType", txnFilter.SortBy);
            param.Add("@SearchVal", txnFilter.SearchVal);


            var data = await connection
                .QueryMultipleAsync("[dbo].[usp_get_inward_remittance_countrywise_report]", param: param, commandType: CommandType.StoredProcedure);

            var txnlist = await data.ReadAsync<InwardRemitanceDto>();
            var pagedInfo = await data.ReadFirstAsync<PageInfo>();
            var total = await data.ReadFirstAsync<SumofTransaction>();
            var mappeddata = _mapper.Map<PagedList<InwardRemitanceDto>>(pagedInfo);
            mappeddata.Items = txnlist;
            return (total, mappeddata);
        }

        public async Task<(SumofTransaction, PagedList<InwardRemitanceCompanyWiseDto>)> GetInwardRemitanceCompanyWiseReport(InwardRemitanceFilterDto txnFilter)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@CountryCode", txnFilter.CountryCode);
            param.Add("@StartDate", txnFilter.StartDate);
            param.Add("@EndDate", txnFilter.EndDate);
            param.Add("@NprToUsdRate", txnFilter.NprToUsdRate);
            param.Add("@frequency", txnFilter.Frequency);
            param.Add("@DateFlag", txnFilter.DateFlag);
            param.Add("@Export", txnFilter.Export);

            param.Add("@PageNumber", txnFilter.PageNumber);
            param.Add("@PageSize", txnFilter.PageSize);
            param.Add("@SortingCol", txnFilter.SortOrder);
            param.Add("@SortType", txnFilter.SortBy);
            param.Add("@SearchVal", txnFilter.SearchVal);


            var data = await connection
                .QueryMultipleAsync("[dbo].[usp_get_inward_remittance_partnerwise_report]", param: param, commandType: CommandType.StoredProcedure);

            var txnlist = await data.ReadAsync<InwardRemitanceCompanyWiseDto>();
            var pagedInfo = await data.ReadFirstAsync<PageInfo>();
            var total = await data.ReadFirstAsync<SumofTransaction>();
            var mappeddata = _mapper.Map<PagedList<InwardRemitanceCompanyWiseDto>>(pagedInfo);
            mappeddata.Items = txnlist;
            return (total, mappeddata);
        }

        public async Task<(SumofTransaction, PagedList<InwardRemitanceAgentWiseReport>)> GetRemitanceCompanySelfOrAgentOrSubAgentInwardDetails(InwardRemitanceFilterDto txnFilter)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@DateFlag", txnFilter.DateFlag);
            param.Add("@StartDate", txnFilter.StartDate);
            param.Add("@EndDate", txnFilter.EndDate);
            param.Add("@frequency", txnFilter.Frequency);

            //param.Add("@Export", txnFilter.Export);

            param.Add("@PageNumber", txnFilter.PageNumber);
            param.Add("@PageSize", txnFilter.PageSize);
            param.Add("@SortingCol", txnFilter.SortOrder);
            param.Add("@SortType", txnFilter.SortBy);
            param.Add("@SearchVal", txnFilter.SearchVal);


            var data = await connection
                .QueryMultipleAsync("[dbo].[usp_get_inward_remittance_agentwise_report]", param: param, commandType: CommandType.StoredProcedure);

            var txnlist = await data.ReadAsync<InwardRemitanceAgentWiseReport>();
            var pagedInfo = await data.ReadFirstAsync<PageInfo>();
            var total = await data.ReadFirstAsync<SumofTransaction>();
            var mappeddata = _mapper.Map<PagedList<InwardRemitanceAgentWiseReport>>(pagedInfo);
            mappeddata.Items = txnlist;
            return (total, mappeddata);
        }

        public async Task<PagedList<ActionTakenByRemitanceDto>> GetActionTakenbyTheRemitanceReport(InwardRemitanceFilterDto txnFilter)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@StartDate", txnFilter.StartDate);
            param.Add("@DateFlag", txnFilter.DateFlag);
            param.Add("@EndDate", txnFilter.EndDate);
            param.Add("@frequency", txnFilter.Frequency);

            //param.Add("@Export", txnFilter.Export);

            param.Add("@PageNumber", txnFilter.PageNumber);
            param.Add("@PageSize", txnFilter.PageSize);
            param.Add("@SortingCol", txnFilter.SortOrder);
            param.Add("@SortType", txnFilter.SortBy);
            param.Add("@SearchVal", txnFilter.SearchVal);


            var data = await connection
                .QueryMultipleAsync("[dbo].[usp_get_legal_obligation_taken_agent_list_report]", param: param, commandType: CommandType.StoredProcedure);

            var txnlist = await data.ReadAsync<ActionTakenByRemitanceDto>();
            var pagedInfo = await data.ReadFirstAsync<PageInfo>();
            //var total = await data.ReadFirstAsync<SumofTransaction>();
            var mappeddata = _mapper.Map<PagedList<ActionTakenByRemitanceDto>>(pagedInfo);
            mappeddata.Items = txnlist;
            return mappeddata;
        }

        public async Task<PagedList<ExportRemitSettlementReport>> ExportSettlementReportAsync(RemitSettlementReportFilter txnFilter)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@PartnerCode", txnFilter.PartnerCode);
            param.Add("@StartDate", txnFilter.StartDate);
            param.Add("@EndDate", txnFilter.EndDate);
            param.Add("@SourceCurrency", txnFilter.SourceCurrency);
            param.Add("@DestinationCurrency", txnFilter.DestinationCurrency);
            param.Add("@TransactionId", txnFilter.TransactionId);
            param.Add("@PartnerName", txnFilter.PartnerName);
            param.Add("@SignType", txnFilter.SignType);
            param.Add("@TransactionType", txnFilter.TransactionType);
            param.Add("@PaymentType", txnFilter.PaymentType);
            param.Add("@MerchantMobileNo", txnFilter.MerchantMobileNo);
            param.Add("@GatewayTxnId", txnFilter.GatewayTxnId);
            param.Add("@TrackerId", txnFilter.TrackerId);
            param.Add("@Type", txnFilter.Type);

            param.Add("@Export", txnFilter.Export);

            param.Add("@UserType", txnFilter.UserType);
            param.Add("@LoggedInUser", txnFilter.LoggedInUser);
            param.Add("@PageNumber", txnFilter.PageNumber);
            param.Add("@PageSize", txnFilter.PageSize);
            param.Add("@SortingCol", txnFilter.SortOrder);
            param.Add("@SortType", txnFilter.SortBy);
            param.Add("@SearchVal", txnFilter.SearchVal);
            var data = await connection
                .QueryMultipleAsync("[dbo].[usp_get_remit_settlement_detail_report]", param: param, commandType: CommandType.StoredProcedure);

            var txnlist = await data.ReadAsync<ExportRemitSettlementReport>();
            var pagedInfo = await data.ReadFirstAsync<PageInfo>();
            var mappeddata = _mapper.Map<PagedList<ExportRemitSettlementReport>>(pagedInfo);
            mappeddata.Items = txnlist;
            return mappeddata;
        }

        public async Task<PagedList<ExportRemitTxnReport>> ExportRemitTxnReportAsync(RemitTxnReportFilter txnFilter)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@PartnerCode", txnFilter.PartnerCode);
            param.Add("@StartDate", txnFilter.StartDate);
            param.Add("@EndDate", txnFilter.EndDate);
            param.Add("@SourceCurrency", txnFilter.SourceCurrency);
            param.Add("@DestinationCurrency", txnFilter.DestinationCurrency);
            param.Add("@TransactionId", txnFilter.TransactionId);
            param.Add("@SignType", txnFilter.SignType);
            param.Add("@TransactionType", txnFilter.TransactionType);
            param.Add("@ComplianceStatus", txnFilter.ComplianceStatus);
            param.Add("@PartnerName", txnFilter.PartnerName);
            param.Add("@GatewayTxnId", txnFilter.GatewayTxnId);
            param.Add("@TransactionStatus", txnFilter.TransactionStatus);
            param.Add("@TrackerId", txnFilter.TrackerId);
            param.Add("@mtcnNumber", txnFilter.ControlNumber);
            param.Add("@PaymentType", txnFilter.PaymentType);
            param.Add("@Export", txnFilter.Export);
            param.Add("@UserType", txnFilter.UserType);
            param.Add("@LoggedInUser", txnFilter.LoggedInUser);
            param.Add("@PageNumber", txnFilter.PageNumber);
            param.Add("@PageSize", txnFilter.PageSize);
            param.Add("@SortingCol", txnFilter.SortOrder);
            param.Add("@SortType", txnFilter.SortBy);
            param.Add("@SearchVal", txnFilter.SearchVal);
            var data = await connection
                .QueryMultipleAsync("[dbo].[usp_get_remit_transaction_report]", param: param, commandType: CommandType.StoredProcedure);

            var txnlist = await data.ReadAsync<ExportRemitTxnReport>();
            var pagedInfo = await data.ReadFirstAsync<PageInfo>();
            var mappeddata = _mapper.Map<PagedList<ExportRemitTxnReport>>(pagedInfo);
            mappeddata.Items = txnlist;
            return mappeddata;
        }

        public async Task<PagedList<AdminExportRemitTxnReport>> AdminExportRemitTxnReportAsync(RemitTxnReportFilter txnFilter)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@PartnerCode", txnFilter.PartnerCode);
            param.Add("@StartDate", txnFilter.StartDate);
            param.Add("@EndDate", txnFilter.EndDate);
            param.Add("@SourceCurrency", txnFilter.SourceCurrency);
            param.Add("@DestinationCurrency", txnFilter.DestinationCurrency);
            param.Add("@TransactionId", txnFilter.TransactionId);
            param.Add("@SignType", txnFilter.SignType);
            param.Add("@TransactionStatus", txnFilter.TransactionStatus);
            param.Add("@TransactionType", txnFilter.TransactionType);
            param.Add("@ComplianceStatus", txnFilter.ComplianceStatus);
            param.Add("@PaymentType", txnFilter.PaymentType);
            param.Add("@PartnerName", txnFilter.PartnerName);
            param.Add("@GatewayTxnId", txnFilter.GatewayTxnId);
            param.Add("@TrackerId", txnFilter.TrackerId);
            param.Add("@mtcnNumber", txnFilter.ControlNumber);
            param.Add("@Export", txnFilter.Export);
            param.Add("@UserType", txnFilter.UserType);
            param.Add("@LoggedInUser", txnFilter.LoggedInUser);
            param.Add("@PageNumber", txnFilter.PageNumber);
            param.Add("@PageSize", txnFilter.PageSize);
            param.Add("@SortingCol", txnFilter.SortOrder);
            param.Add("@SortType", txnFilter.SortBy);
            param.Add("@SearchVal", txnFilter.SearchVal);
            var data = await connection
                .QueryMultipleAsync("[dbo].[usp_get_remit_transaction_report]", param: param, commandType: CommandType.StoredProcedure);

            var txnlist = await data.ReadAsync<AdminExportRemitTxnReport>();
            var pagedInfo = await data.ReadFirstAsync<PageInfo>();
            var mappeddata = _mapper.Map<PagedList<AdminExportRemitTxnReport>>(pagedInfo);
            mappeddata.Items = txnlist;
            return mappeddata;
        }

        public async Task<PagedList<AdminExportRemitSettlementReport>> ExportAdminRemitSettlementReportAsync(RemitSettlementReportFilter txnFilter)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@PartnerCode", txnFilter.PartnerCode);
            param.Add("@StartDate", txnFilter.StartDate);
            param.Add("@EndDate", txnFilter.EndDate);
            param.Add("@SourceCurrency", txnFilter.SourceCurrency);
            param.Add("@DestinationCurrency", txnFilter.DestinationCurrency);
            param.Add("@TransactionId", txnFilter.TransactionId);
            param.Add("@PartnerName", txnFilter.PartnerName);
            param.Add("@SignType", txnFilter.SignType);
            param.Add("@TransactionType", txnFilter.TransactionType);
            param.Add("@PaymentType", txnFilter.PaymentType);
            param.Add("@MerchantMobileNo", txnFilter.MerchantMobileNo);
            param.Add("@GatewayTxnId", txnFilter.GatewayTxnId);
            param.Add("@TrackerId", txnFilter.TrackerId);
            param.Add("@Type", txnFilter.Type);
            param.Add("@Export", txnFilter.Export);
            param.Add("@UserType", txnFilter.UserType);
            param.Add("@LoggedInUser", txnFilter.LoggedInUser);
            param.Add("@PageNumber", txnFilter.PageNumber);
            param.Add("@PageSize", txnFilter.PageSize);
            param.Add("@SortingCol", txnFilter.SortOrder);
            param.Add("@SortType", txnFilter.SortBy);
            param.Add("@SearchVal", txnFilter.SearchVal);
            var data = await connection
                .QueryMultipleAsync("[dbo].[usp_get_remit_settlement_detail_report]", param: param, commandType: CommandType.StoredProcedure);

            var txnlist = await data.ReadAsync<AdminExportRemitSettlementReport>();
            var pagedInfo = await data.ReadFirstAsync<PageInfo>();
            var mappeddata = _mapper.Map<PagedList<AdminExportRemitSettlementReport>>(pagedInfo);
            mappeddata.Items = txnlist;
            return mappeddata;
        }



        public async Task<PagedList<ExportRemitSettlementReport>> ExportRemitSettlementReportAsync(RemitSettlementReportFilter txnFilter)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@PartnerCode", txnFilter.PartnerCode);
            param.Add("@StartDate", txnFilter.StartDate);
            param.Add("@EndDate", txnFilter.EndDate);
            param.Add("@SourceCurrency", txnFilter.SourceCurrency);
            param.Add("@DestinationCurrency", txnFilter.DestinationCurrency);
            param.Add("@TransactionId", txnFilter.TransactionId);
            param.Add("@PartnerName", txnFilter.PartnerName);
            param.Add("@SignType", txnFilter.SignType);
            param.Add("@TransactionType", txnFilter.TransactionType);

            param.Add("@MerchantMobileNo", txnFilter.MerchantMobileNo);
            param.Add("@GatewayTxnId", txnFilter.GatewayTxnId);
            param.Add("@TrackerId", txnFilter.TrackerId);
            param.Add("@Type", txnFilter.Type);

            param.Add("@Export", txnFilter.Export);

            param.Add("@UserType", txnFilter.UserType);
            param.Add("@LoggedInUser", txnFilter.LoggedInUser);
            param.Add("@PageNumber", txnFilter.PageNumber);
            param.Add("@PageSize", txnFilter.PageSize);
            param.Add("@SortingCol", txnFilter.SortOrder);
            param.Add("@SortType", txnFilter.SortBy);
            param.Add("@SearchVal", txnFilter.SearchVal);
            var data = await connection
                .QueryMultipleAsync("[dbo].[usp_get_remit_settlement_report]", param: param, commandType: CommandType.StoredProcedure);

            var txnlist = await data.ReadAsync<ExportRemitSettlementReport>();
            var pagedInfo = await data.ReadFirstAsync<PageInfo>();
            var mappeddata = _mapper.Map<PagedList<ExportRemitSettlementReport>>(pagedInfo);
            mappeddata.Items = txnlist;
            return mappeddata;
        }

        public async Task<PagedList<AdminExportRemitSettlementReport>> ExportAdminSettlementReportAsync(RemitSettlementReportFilter txnFilter)
        {
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();

                var param = new DynamicParameters();
                param.Add("@PartnerCode", txnFilter.PartnerCode);
                param.Add("@StartDate", txnFilter.StartDate);
                param.Add("@EndDate", txnFilter.EndDate);
                param.Add("@SourceCurrency", txnFilter.SourceCurrency);
                param.Add("@DestinationCurrency", txnFilter.DestinationCurrency);
                param.Add("@TransactionId", txnFilter.TransactionId);
                param.Add("@SignType", txnFilter.SignType);
                param.Add("@TransactionType", txnFilter.TransactionType);

                param.Add("@MerchantMobileNo", txnFilter.MerchantMobileNo);
                param.Add("@GatewayTxnId", txnFilter.GatewayTxnId);
                param.Add("@TrackerId", txnFilter.TrackerId);

                param.Add("@Export", txnFilter.Export);
                param.Add("@Type", txnFilter.Type);


                param.Add("@UserType", txnFilter.UserType);
                param.Add("@LoggedInUser", txnFilter.LoggedInUser);
                param.Add("@PageNumber", txnFilter.PageNumber);
                param.Add("@PageSize", txnFilter.PageSize);
                param.Add("@SortingCol", txnFilter.SortOrder);
                param.Add("@SortType", txnFilter.SortBy);
                param.Add("@SearchVal", txnFilter.SearchVal);
                var data = await connection
                    .QueryMultipleAsync("[dbo].[usp_get_remit_settlement_report]", param: param, commandType: CommandType.StoredProcedure);

                var txnlist = await data.ReadAsync<AdminExportRemitSettlementReport>();
                var pagedInfo = await data.ReadFirstAsync<PageInfo>();
                var mappeddata = _mapper.Map<PagedList<AdminExportRemitSettlementReport>>(pagedInfo);
                mappeddata.Items = txnlist;
                return mappeddata;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }
}
