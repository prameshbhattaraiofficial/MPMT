using AutoMapper;
using Dapper;
using DocumentFormat.OpenXml.EMMA;
using Mpmt.Core.Dtos.ComplianceRule;
using Mpmt.Core.Dtos.DropDown;
using Mpmt.Core.Dtos.Paging;
using Mpmt.Core.Dtos.PartnerReport;
using Mpmt.Data.Common;
using Mpmts.Core.Dtos;
using System.Collections.Generic;
using System.Data;

namespace Mpmt.Data.Repositories.ComplianceRule;

public class ComplianceRuleRepo : IComplianceRuleRepo
{
    private readonly IMapper _mapper;

    public ComplianceRuleRepo(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<SprocMessage> AddComplianceCountryList(string countryListString)
    {
        try
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@CountryCodes", countryListString);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[usp_update_country_compliance]", param, commandType: CommandType.StoredProcedure);

            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IEnumerable<CountryComplianceRule>> GetAllCountryList()
    {
        try
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            return (await connection.QueryAsync<CountryComplianceRule>("[dbo].[usp_get_country_non_compliance]", commandType: CommandType.StoredProcedure));
        }
        catch (Exception ex) 
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<IEnumerable<CountryComplianceRule>> GetComplianceCountryList()
    {
        try
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            return (await connection.QueryAsync<CountryComplianceRule>("[dbo].[usp_get_country_compliance]", commandType: CommandType.StoredProcedure));
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);

        }
    }

    public async Task<PagedList<ComplianceRuleList>> GetComplianceRuleAsync(ComplianceRuleFilter filter)
    {
        try
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@ComplianceCode", filter.ComplianceCode);
            param.Add("@ComplianceRule", filter.ComplianceRule);
            param.Add("@ComplianceAction", filter.ComplianceAction);


            param.Add("@PageNumber", filter.PageNumber);
            param.Add("@PageSize", filter.PageSize);
            param.Add("@SortingCol", filter.SortBy);
            param.Add("@SortType", filter.SortOrder);
            param.Add("@SearchVal", filter.SearchVal);


            var data = await connection
                .QueryMultipleAsync("[dbo].[usp_get_compliance_settings]", param: param, commandType: CommandType.StoredProcedure);

            var ComplianceRule = await data.ReadAsync<ComplianceRuleList>();
            var pagedInfo = await data.ReadFirstAsync<PageInfo>();
            var mappeddata = _mapper.Map<PagedList<ComplianceRuleList>>(pagedInfo);
            mappeddata.Items = ComplianceRule;
            return mappeddata;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<PagedList<RemitTxnReport>> GetComplianceTransactionAsync(RemitTxnReportFilter txnFilter)
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

        param.Add("@UserType", txnFilter.UserType);
        param.Add("@LoggedInUser", txnFilter.LoggedInUser);
        param.Add("@PageNumber", txnFilter.PageNumber);
        param.Add("@PageSize", txnFilter.PageSize);
        param.Add("@SortingCol", txnFilter.SortOrder);
        param.Add("@SortType", txnFilter.SortBy);
        param.Add("@SearchVal", txnFilter.SearchVal);
        var data = await connection
            .QueryMultipleAsync("[dbo].[usp_get_complicance_transaction_report_list]",param:param, commandType: CommandType.StoredProcedure);

        var txnlist = await data.ReadAsync<RemitTxnReport>();
        var pagedInfo = await data.ReadFirstAsync<PageInfo>();
        var mappeddata = _mapper.Map<PagedList<RemitTxnReport>>(pagedInfo);
        mappeddata.Items = txnlist;
        return mappeddata;
    }

    public async Task<IEnumerable<Commonddl>> GetFrequency()
    {
        try
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            return (await connection.QueryAsync<Commonddl>("[dbo].[usp_get_date_frequency]", commandType: CommandType.StoredProcedure));
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);

        }
    }

    public async Task<SprocMessage> ReleaseTransaction(string transactionId, string LoggedInuser, string UserType)
    {
        try
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@transactionId", transactionId);
            param.Add("@LoggedInUser", LoggedInuser);
            param.Add("@Usertype", UserType);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
            _ = await connection.ExecuteAsync("[dbo].[usp_update_compliance_transaction]", param, commandType: CommandType.StoredProcedure);
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<SprocMessage> UpdateComplianceRule(ComplianceRuleDetail list)
    {
        try
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@OperationMode", 'U');
            param.Add("@Id", list.Id);
            param.Add("@Count", list.CountValue);
            param.Add("@NoOfDays", list.Frequency);
            param.Add("@ComplianceAction", list.ComplianceAction);
            param.Add("@IsActive", list.IsActive);

            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[usp_compliance_rule_addupdate]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@IdentityVal");
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
}
