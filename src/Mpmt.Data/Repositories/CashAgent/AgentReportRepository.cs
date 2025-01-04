using AutoMapper;
using Dapper;
using Mpmt.Core.Dtos.AgentFundTransfer;
using Mpmt.Core.Dtos.AgentReport;
using Mpmt.Core.Dtos.CashAgent;
using Mpmt.Core.Dtos.Paging;
using Mpmt.Core.Models.CashAgent;
using Mpmt.Data.Common;
using Mpmts.Core.Dtos;
using System.Data;

namespace Mpmt.Data.Repositories.CashAgent;

public class AgentReportRepository : IAgentReportRepository
{
    private readonly IMapper _mapper;

    public AgentReportRepository(IMapper mapper)
    {
        _mapper = mapper;
    }
    public async Task<PagedList<FundRequest>> GetAgentFundRequestDetailsAsync(AgentFundRequestFilter txnFilter)
    {
        try
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@AgentCode", txnFilter.AgentCode);
            param.Add("@StartDate", txnFilter.StartDateBS);
            param.Add("@EndDate", txnFilter.EndDateBS);
            param.Add("@Export", txnFilter.Export);

            param.Add("@PageNumber", txnFilter.PageNumber);
            param.Add("@PageSize", txnFilter.PageSize);
            param.Add("@SortingCol", txnFilter.SortOrder);
            param.Add("@SortType", txnFilter.SortBy);
            param.Add("@SearchVal", txnFilter.SearchVal);
            var data = await connection
                .QueryMultipleAsync("[dbo].[sp_get_agent_fundrequest_list]", param: param, commandType: CommandType.StoredProcedure);

            var commissionList = await data.ReadAsync<FundRequest>();
            var pagedInfo = await data.ReadFirstAsync<PageInfo>();
            var mappedData = _mapper.Map<PagedList<FundRequest>>(pagedInfo);
            mappedData.Items = commissionList;
            return mappedData;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<PagedList<AgentCommissionTransactionReport>> GetAgentCommissionTxnReportAsync(AgentCommissionFilter txnFilter)
    {
        try
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@AgentCode", txnFilter.AgentCode);
            param.Add("@StartDate", txnFilter.StartDate);
            param.Add("@EndDate", txnFilter.EndDate);
            param.Add("@DistrictCode", txnFilter.DistrictCode);
            param.Add("@TransactionId", txnFilter.TransactionId);
            param.Add("@AgentOrganizationName", txnFilter.AgentOrganizationName);
            param.Add("@RecipientContactNumber", txnFilter.RecipientContactNumber);
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
                .QueryMultipleAsync("[dbo].[usp_get_remit_agent_Commission_transaction_report]", param: param, commandType: CommandType.StoredProcedure);

            var commissionList = await data.ReadAsync<AgentCommissionTransactionReport>();
            var pagedInfo = await data.ReadFirstAsync<PageInfo>();
            var mappedData = _mapper.Map<PagedList<AgentCommissionTransactionReport>>(pagedInfo);
            mappedData.Items = commissionList;
            return mappedData;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<PagedList<FundRequest>> GetFundReqListForAdmin(AgentFundRequestFilter txnFilter)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();
        try
        {
            var param = new DynamicParameters();
            param.Add("@AgentName", txnFilter.AgentName);
            param.Add("@AgentCode", txnFilter.AgentCode);
            param.Add("@Status", txnFilter.Status);
            param.Add("@StartDate", txnFilter.StartDateBS); 
            param.Add("@EndDate", txnFilter.EndDateBS);
            param.Add("@Export", txnFilter.Export);

            param.Add("@PageNumber", txnFilter.PageNumber);
            param.Add("@PageSize", txnFilter.PageSize);
            param.Add("@SortingCol", txnFilter.SortOrder);
            param.Add("@SortType", txnFilter.SortBy);
            param.Add("@SearchVal", txnFilter.SearchVal);
            var data = await connection
                .QueryMultipleAsync("[dbo].[sp_get_agent_fundrequest_list]", param: param, commandType: CommandType.StoredProcedure);

            var commissionList = await data.ReadAsync<FundRequest>();
            var pagedInfo = await data.ReadFirstAsync<PageInfo>();
            var mappedData = _mapper.Map<PagedList<FundRequest>>(pagedInfo);
            mappedData.Items = commissionList;
            return mappedData;
        }
        catch (Exception ex)
        {
            throw;
        }
    }
 
    public async Task<PagedList<AgentSettlementReport>> GetAgentSettlementReportAsync(AgentSettlementFilter txnFilter)

    {
        try
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@AgentCode", txnFilter.AgentCode);
            param.Add("@StartDate", txnFilter.StartDate);
            param.Add("@EndDate", txnFilter.EndDate);
            param.Add("@AgentName", txnFilter.AgentName);
            param.Add("@StartDate", txnFilter.StartDate);
            param.Add("@EndDate", txnFilter.EndDate);
            param.Add("@SourceCurrency", txnFilter.SourceCurrency);
            param.Add("@DestinationCurrency", txnFilter.DestinationCurrency);
            param.Add("@TransactionId", txnFilter.TransactionId);
            param.Add("@TransactionType", txnFilter.TransactionType);
            param.Add("@AgentDistrict", txnFilter.AgentDistrict);
            param.Add("@UserType", txnFilter.UserType);
            param.Add("@LoggedInUser", txnFilter.LoggedInUser);
            param.Add("@Export", txnFilter.Export);

            param.Add("@PageNumber", txnFilter.PageNumber);
            param.Add("@PageSize", txnFilter.PageSize);
            param.Add("@SortingCol", txnFilter.SortOrder);
            param.Add("@SortType", txnFilter.SortBy);
            param.Add("@SearchVal", txnFilter.SearchVal);
            var data = await connection
                .QueryMultipleAsync("[dbo].[usp_get_remit_settlement_detail_report_agent]", param: param, commandType: CommandType.StoredProcedure);

            var commissionList = await data.ReadAsync<AgentSettlementReport>();
            var pagedInfo = await data.ReadFirstAsync<PageInfo>();
            var mappedData = _mapper.Map<PagedList<AgentSettlementReport>>(pagedInfo);

            mappedData.Items = commissionList;
            return mappedData;
        }
        catch (Exception)
        {
            throw;
        }
    }


    public async Task<(ApproveRejectReviewModel, SprocMessage)> ApproveRejectAgentFundRequestAysnc(ApproveRejectFundTransferByAdmin model)    
     {
        try
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@FundRequestId", model.RequestFundId);
            param.Add("@AgentCode", model.AgentCode);
            param.Add("@OperationMode", model.OperationMode);
            param.Add("@VoucherImagePath", model.VoucherImage);
            param.Add("@IsCommissionRequested", model.IsCommissionRequested);
            param.Add("@IsTxnCashRequested", model.IsTxnCashRequested);
            param.Add("@Remarks", model.Remarks);
            param.Add("@Amount", model.TotalAmount);
            param.Add("@TxnId", model.TransactionId);
           
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            //_ = await connection.ExecuteAsync("[dbo].[usp_agent_fund_request_status_update]", param, commandType: CommandType.StoredProcedure);
            //var statusCode = param.Get<int>("@StatusCode");
            //var msgType = param.Get<string>("@MsgType");
            //var msgText = param.Get<string>("@MsgText");

            //return new SprocMessage {  StatusCode = statusCode, MsgType = msgType, MsgText = msgText };

            var data = await connection
                .QueryMultipleAsync("[dbo].[usp_agent_fund_request_status_update]", param: param, commandType: CommandType.StoredProcedure);
            var agentInfo = await data.ReadFirstOrDefaultAsync<ApproveRejectReviewModel>();
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");
            return (agentInfo, new SprocMessage { StatusCode = statusCode, MsgType = msgType, MsgText = msgText });
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<AgentFundApproveRejectModel> GetDetailsRequestFundAsync(AgentFundApproveRejectModel model)
    {
        try
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@FundRequestId", model.RequestFundId);
            param.Add("@AgentCode", model.AgentCode);
            var check = await connection
                .QueryFirstOrDefaultAsync<AgentFundApproveRejectModel>("[dbo].[usp_get_agent_fundrequest_byid]", param, commandType: CommandType.StoredProcedure);
            return check;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<IEnumerable<AgentUnsettledAmount>> GetAgentUnsettledAmountSummaryList()
    {
        using var connection = DbConnectionManager.GetDefaultConnection();
        return await connection.QueryAsync<AgentUnsettledAmount>("[dbo].[usp_get_agent_unsettled_amount_summary_list]", commandType: CommandType.StoredProcedure);
    }

    public async Task<PagedList<AgentCommissionTransactionReportDetail>> GetAgentCommissionTxnReportDetailAsync(AgentCommissionFilter txnFilter)
    {
        try
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@AgentCode", txnFilter.AgentCode);
            param.Add("@StartDate", txnFilter.StartDate);
            param.Add("@EndDate", txnFilter.EndDate);
            param.Add("@DistrictCode", txnFilter.DistrictCode);
            param.Add("@TransactionId", txnFilter.TransactionId);
            param.Add("@AgentOrganizationName", txnFilter.AgentOrganizationName);
            param.Add("@RecipientContactNumber", txnFilter.RecipientContactNumber);
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
                .QueryMultipleAsync("[dbo].[usp_get_remit_agent_Commission_transaction_report_detail]", param: param, commandType: CommandType.StoredProcedure);

            var commissionList = await data.ReadAsync<AgentCommissionTransactionReportDetail>();
            var pagedInfo = await data.ReadFirstAsync<PageInfo>();
            var mappedData = _mapper.Map<PagedList<AgentCommissionTransactionReportDetail>>(pagedInfo);
            mappedData.Items = commissionList;
            return mappedData;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
