using AutoMapper;
using Dapper;
using Mpmt.Core.Domain.Partners.SendTransactions;
using Mpmt.Core.Dtos.Partner;
using Mpmt.Data.Common;
using Mpmts.Core.Dtos;
using System.Data;

namespace Mpmt.Data.Repositories.Partner;

public class PartnerDashBoardRepo : IPartnerDashBoardRepo
{
    public readonly IMapper _mapper;

    public PartnerDashBoardRepo(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<SprocMessage> CheckWalletBalance(GetSendTransferAmountDetailRequest request)
    {
        try
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@PartnerCode", request.PartnerCode);
            param.Add("@SourceCurrency", request.SourceCurrency);
            param.Add("@TransferAmount", request.SourceAmount);

            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[usp_check_wallet_balance]", param, commandType: CommandType.StoredProcedure);

            //var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            //return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
            return new SprocMessage { StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<IEnumerable<DashboardWalletBalance>> GetPartnerBalanceDashboard(string partnerCode, string filterBy, string orderBy)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();
        var param = new DynamicParameters();
        param.Add("@PartnerCode", partnerCode);
        param.Add("@FilterBy", filterBy);
        param.Add("@OrderBy", orderBy);
        return await connection.QueryAsync<DashboardWalletBalance>("[dbo].[usp_get_partner_balances_dashboard]", param: param, commandType: CommandType.StoredProcedure);
    }

    public async Task<PartnerDashBoard> GetPartnerDashBoard(string Partnercode)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();

        var param = new DynamicParameters();
        param.Add("PartnerCode", Partnercode);

        var data = await connection
            .QueryMultipleAsync("[dbo].[usp_partnerdashboard]", param: param, commandType: CommandType.StoredProcedure);

        var Dashboarddata = await data.ReadFirstAsync<PartnerDashBoard>();
        var walletdata = await data.ReadAsync<DashBoardWallet>();
        var conversionratedata = await data.ReadAsync<DashBoardConversionRate>();
        var transactiondata = await data.ReadAsync<DashBoardTransactionDetails>();
        var feeAccountData = await data.ReadAsync<DashBoardFeeAccount>();
        var lineChartData = await data.ReadAsync<FrequencyWiseTransaction>();
        var walletBalance = await data.ReadAsync<DashboardWalletBalance>();
        var partnerSender = await data.ReadAsync<DashboardPartnerSender>();
        var transactionStatus = await data.ReadAsync<DashboardTransactionStatus>();
        var approxDays = await data.ReadAsync<DashboardApproxDays>();

        var mappedwallet = _mapper.Map<List<DashBoardWallet>>(walletdata);
        var mappedconversionrate = _mapper.Map<List<DashBoardConversionRate>>(conversionratedata);
        var mappedtransactiondata = _mapper.Map<List<DashBoardTransactionDetails>>(transactiondata);
        var mappedfeeaccountdata = _mapper.Map<List<DashBoardFeeAccount>>(feeAccountData);
        var mappedLineChart = _mapper.Map<List<FrequencyWiseTransaction>>(lineChartData);   
        var mappedwalletBalance = _mapper.Map<List<DashboardWalletBalance>>(walletBalance); 
        var mappedpartnerSender = _mapper.Map<List<DashboardPartnerSender>>(partnerSender); 
        var mappedtransactionStatus = _mapper.Map<List<DashboardTransactionStatus>>(transactionStatus); 
        var mappedapproxDays = _mapper.Map<List<DashboardApproxDays>>(approxDays); 

        Dashboarddata.dashboardwallet = mappedwallet;
        Dashboarddata.dashBoardConversion = mappedconversionrate;
        Dashboarddata.dashBoardtransaction = mappedtransactiondata;
        Dashboarddata.dashBoardFeeAccount = mappedfeeaccountdata;
        Dashboarddata.frequencyWiseTransactions = mappedLineChart;
        Dashboarddata.dashboardWalletBalances = mappedwalletBalance;
        Dashboarddata.dashboardPartnerSenders = mappedpartnerSender;
        Dashboarddata.dashboardTransactionStatus = mappedtransactionStatus; 
        Dashboarddata.dashboardApproxDays = mappedapproxDays;   

        return Dashboarddata;
    }

    public async Task<IEnumerable<DashboardPartnerSender>> GetPartnerSenderDashboard(string partnerCode, string frequency, string filterBy, string orderBy)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();
        var param = new DynamicParameters();
        param.Add("@PartnerCode", partnerCode);
        param.Add("@Frequency", frequency);
        param.Add("@FilterBy", filterBy);
        param.Add("@OrderBy", orderBy);
        return await connection.QueryAsync<DashboardPartnerSender>("[dbo].[usp_get_partner_senders_dashboard]", param: param, commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<FrequencyWiseTransaction>> GetTransactionDataFrequencyWise(string frequency, string partnerCode)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();
        var param = new DynamicParameters();
        param.Add("@frequency", frequency);
        param.Add("@partnerCode", partnerCode);
        return await connection.QueryAsync<FrequencyWiseTransaction>("[dbo].[usp_get_transaction_data_frequency_wise]", param: param, commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<DashboardTransactionStatus>> GetTransactionStatusDashboard(string frequency, string partnerCode)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();
        var param = new DynamicParameters();
        param.Add("@Frequency", frequency);
        param.Add("@PartnerCode", partnerCode);
        return await connection.QueryAsync<DashboardTransactionStatus>("[dbo].[usp_get_transaction_status_dashboard]", param: param, commandType: CommandType.StoredProcedure);
    }

    public async Task<SendTransferAmountDetailDto> GetTransferAmountDetailsAsync(GetSendTransferAmountDetailRequest request)
    {
        try
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@SourceCurrency", request.SourceCurrency);
            param.Add("@DestinationCurrency", request.DestinationCurrency);
            param.Add("@Amount", request.SourceAmount);

            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            var transferAmounts = await connection.QueryFirstOrDefaultAsync<SendTransferAmountDetailDto>(
                "[dbo].[usp_currency_converter]", param: param, commandType: CommandType.StoredProcedure);

            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return transferAmounts;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<SprocMessage> PartnerDashboardOTPAsync(TokenVerification tokenVerification)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();

        var param = new DynamicParameters();
        param.Add("@UserId", tokenVerification.UserId);
        param.Add("@PartnerCode", tokenVerification.PartnerCode);
        param.Add("@Username", tokenVerification.UserName);
        param.Add("@Email", tokenVerification.Email);
        param.Add("@CountryCode", tokenVerification.CountryCallingCode);
        param.Add("@Mobile", tokenVerification.Mobile);
        param.Add("@VerificationCode", tokenVerification.VerificationCode);
        param.Add("@VerificationType", tokenVerification.VerificationType);
        param.Add("@SendToMobile", tokenVerification.SendToMobile);
        param.Add("@SendToEmail", tokenVerification.SendToEmail);
        param.Add("@ExpiryDate", tokenVerification.ExpiredDate);
        param.Add("@IsConsumed", tokenVerification.IsConsumed);
        param.Add("@OtpVerificationFor", tokenVerification.OtpVerificationFor);

        param.Add("@ReturnPrimaryId", dbType: DbType.Int32, direction: ParameterDirection.Output);
        param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
        param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
        param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

        try
        {
            var _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_Token_Verification]", param: param, commandType: CommandType.StoredProcedure);
        }
        catch (Exception)
        {
            return new SprocMessage { IdentityVal = 0, StatusCode = 400, MsgType = "Error", MsgText = "Issue Inserting Data" };
        }

        var identityVal = param.Get<int>("@ReturnPrimaryId");
        var statusCode = param.Get<int>("@StatusCode");
        var msgType = param.Get<string>("@MsgType");
        var msgText = param.Get<string>("@MsgText");

        return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
    }

    public async Task<SprocMessage> SendTransferAmount(GetSendTransferAmountDetailRequest request)
    {
        try
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@PartnerCode", request.PartnerCode);
            param.Add("@SourceCurrency", request.SourceCurrency);
            param.Add("@DestinationCurrency", request.DestinationCurrency);
            param.Add("@AccountType", request.AccountType);
            param.Add("@Amount", request.SourceAmount);
            param.Add("@Remarks", request.Remarks);
            param.Add("@LoggedInUser", request.LoggedInUser);
            param.Add("@UserType", request.UserType);

            //param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[usp_transfer_fund]", param, commandType: CommandType.StoredProcedure);

            //var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            //return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
            return new SprocMessage { StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
