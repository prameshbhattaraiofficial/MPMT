using AutoMapper;
using Dapper;
using Mpmt.Core.Dtos.AdminDashboard;
using Mpmt.Core.Dtos.Partner;
using Mpmt.Core.Dtos.Roles;
using Mpmt.Data.Common;
using System.Data;

namespace Mpmt.Data.Repositories.AdminDashBoard;

public class AdminDashBoardRepo : IAdminDashBoardRepo
{
    public readonly IMapper _mapper;

    public AdminDashBoardRepo(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<AdminDashboardDetails> GetAdminDashBoard()
    {
        using var connection = DbConnectionManager.GetDefaultConnection();

        var data = await connection
            .QueryMultipleAsync("[dbo].[usp_admindashboard]", commandType: CommandType.StoredProcedure);

        var Dashboarddata = await data.ReadFirstAsync<AdminDashboardDetails>();
        var ActivityLogdata = await data.ReadAsync<DashboardActivityLog>();
        var Exchangeratedata = await data.ReadAsync<DashboardExchangeRate>();
        var transactiondata = await data.ReadAsync<DashBoardTransactionReport>();

        var lineChartData = await data.ReadAsync<FrequencyWiseTransaction>();
        var partnerBalance = await data.ReadAsync<DashboardApproxDays>();
        var transactionStatus = await data.ReadAsync<DashboardTransactionStatus>();
        var topPartner = await data.ReadAsync<DashboardAdminTopPartner>();
        var topAgent = await data.ReadAsync<DashboardAdminTopAgent>();
        var paymentMode = await data.ReadAsync<DashboardAdminPaymentMode>();
        var thresholdTrans = await data.ReadAsync<DashboardAdminPaymentMode>();
        var topAgentLocation = await data.ReadAsync<DashboardTopAgentLocation>();

        var mappedactivitylog = _mapper.Map<List<DashboardActivityLog>>(ActivityLogdata);
        var mappedExchangerate = _mapper.Map<List<DashboardExchangeRate>>(Exchangeratedata);
        var mappedtransactiondata = _mapper.Map<List<DashBoardTransactionReport>>(transactiondata);

        var mappedLineChart = _mapper.Map<List<FrequencyWiseTransaction>>(lineChartData);
        var mappedPartnerBalance = _mapper.Map<List<DashboardApproxDays>>(partnerBalance);
        var mappedTransactionStatus = _mapper.Map<List<DashboardTransactionStatus>>(transactionStatus);
        var mapperTopPartner = _mapper.Map<List<DashboardAdminTopPartner>>(topPartner);
        var mapperTopAgent = _mapper.Map<List<DashboardAdminTopAgent>>(topAgent);
        var mapperTopAgentLocation = _mapper.Map<List<DashboardTopAgentLocation>>(topAgentLocation);
        var mappedPaymentMode = _mapper.Map<List<DashboardAdminPaymentMode>>(paymentMode);
        var mappedThresholdTransaction = _mapper.Map<List<DashboardAdminPaymentMode>>(thresholdTrans);

        Dashboarddata.ActivityLog = mappedactivitylog;
        Dashboarddata.ExchangeRate = mappedExchangerate;
        Dashboarddata.TransactionReport = mappedtransactiondata;

        Dashboarddata.frequencyWiseTransactions = mappedLineChart;
        Dashboarddata.dashboardPartnerBalance = mappedPartnerBalance;
        Dashboarddata.dashboardTransactionStatus = mappedTransactionStatus;
        Dashboarddata.dashboardTopPartner = mapperTopPartner;
        Dashboarddata.dashboardTopAgent = mapperTopAgent;
        Dashboarddata.dashboardTopAgentLocation = mapperTopAgentLocation;
        Dashboarddata.dashboardPaymentMode = mappedPaymentMode;
        Dashboarddata.dashboardThresholdTrans = mappedThresholdTransaction; 

        return Dashboarddata;
    }

    public async Task<IEnumerable<DashboardApproxDays>> GetPartnerBalanceApproxDaysDashboard(string partnerCode, string filterBy, string orderBy)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();
        var param = new DynamicParameters();
        param.Add("@PartnerCode", partnerCode);
        param.Add("@FilterBy", filterBy);
        param.Add("@OrderBy", orderBy);
        return await connection.QueryAsync<DashboardApproxDays>("[dbo].[usp_get_apprx_days_dashboard]", param: param, commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<DashboardAdminPaymentMode>> GetPaymentModeDashboard(string frequency, string filterBy, string orderBy)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();
        var param = new DynamicParameters();
        param.Add("@Frequency", frequency);
        param.Add("@FilterBy", filterBy);
        param.Add("@OrderBy", orderBy);
        return await connection.QueryAsync<DashboardAdminPaymentMode>("[dbo].[usp_get_data_by_payment_type_dashboard]", param: param, commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<DashboardAdminPaymentMode>> GetThresholdDataDashboard(string frequency, string filterBy, string orderBy)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();
        var param = new DynamicParameters();
        param.Add("@Frequency", frequency);
        param.Add("@FilterBy", filterBy);
        param.Add("@OrderBy", orderBy);
        return await connection.QueryAsync<DashboardAdminPaymentMode>("[dbo].[usp_get_threshold_data_by_payment_type_dashboard]", param: param, commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<DashboardAdminTopAgent>> GetTopAgentDashboard(string frequency, string filterBy, string orderBy)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();
        var param = new DynamicParameters();
        param.Add("@Frequency", frequency);
        param.Add("@FilterBy", filterBy);
        param.Add("@OrderBy", orderBy);
        return await connection.QueryAsync<DashboardAdminTopAgent>("[dbo].[usp_get_top_agents_dashboard]", param: param, commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<DashboardTopAgentLocation>> GetTopAgentLocationDashboard(string frequency, string filterBy, string orderBy)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();
        var param = new DynamicParameters();
        param.Add("@Frequency", frequency);
        param.Add("@FilterBy", filterBy);
        param.Add("@OrderBy", orderBy);
        return await connection.QueryAsync<DashboardTopAgentLocation>("[dbo].[usp_get_top_agents_by_location_dashboard]", param: param, commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<DashboardAdminTopPartner>> GetTopPartnerDashboard(string frequency, string filterBy, string orderBy)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();
        var param = new DynamicParameters();
        param.Add("@Frequency", frequency);
        param.Add("@FilterBy", filterBy);
        param.Add("@OrderBy", orderBy);
        return await connection.QueryAsync<DashboardAdminTopPartner>("[dbo].[usp_get_top_partners_dashboard]", param: param, commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<FrequencyWiseTransaction>> GetTransactionDataFrequencyWise(string frequency)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();
        var param = new DynamicParameters();
        param.Add("@frequency", frequency);
        return await connection.QueryAsync<FrequencyWiseTransaction>("[dbo].[usp_get_transaction_data_frequency_wise]", param: param, commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<DashboardTransactionStatus>> GetTransactionStatusDashboard(string frequency)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();
        var param = new DynamicParameters();
        param.Add("@Frequency", frequency);
        return await connection.QueryAsync<DashboardTransactionStatus>("[dbo].[usp_get_transaction_status_dashboard]", param: param, commandType: CommandType.StoredProcedure);
    }
}
