namespace Mpmt.Core.Dtos.Partner;
public class PartnerDashBoard
{
    public string TotalUser { get; set; }
    public string TodayUser { get; set; }
    public string WeekUser { get; set; }
    public string MonthUser { get; set; }
    public string TotalTransaction { get; set; }
    public string TodayTransaction { get; set; }
    public string WeekTransaction { get; set; }
    public string MonthTransaction { get; set; }
    public string TotalCommissionTransaction { get; set; }
    public string TodayCommissionTransaction { get; set; }
    public string WeekCommissionTransaction { get; set; }
    public string MonthCommissionTransaction { get; set; }
    public string apiCredentialsid { get; set; }
    public string ApiKey { get; set; }
    public string ApiPassword { get; set; }
    public DateTime LocalDateTime { get; set; }
    public List<DashBoardWallet> dashboardwallet { get; set; }
    public List<DashBoardConversionRate> dashBoardConversion { get; set; }
    public List<DashBoardTransactionDetails> dashBoardtransaction { get; set; }
    public List<DashBoardFeeAccount> dashBoardFeeAccount { get; set; }
    public List<FrequencyWiseTransaction> frequencyWiseTransactions { get; set; }
    public List<DashboardWalletBalance> dashboardWalletBalances { get; set; }
    public List<DashboardPartnerSender> dashboardPartnerSenders { get; set; }
    public List<DashboardTransactionStatus> dashboardTransactionStatus { get; set; }    
    public List<DashboardApproxDays> dashboardApproxDays { get; set; }      
    public string PartnerCode { get; set; }
    public string SourceCurrency { get; set; }
    public string DestinationCurrency { get; set; }
    public string AccountType { get; set; }
    public string SourceAmount { get; set; }
    public string Remarks { get; set; }
    public string DestinationAmount { get; set; }
    public List<string> Frequency { get; set; }
    public List<string> Labels { get; set; }
    public List<double> TransactionData { get; set; }
    public List<double> VolumeData { get; set; }

    public List<string> PieChartLabels { get; set; }
    public List<int> PieChartData { get; set; } 
}

public class DashBoardWallet
{
    public string CurrencyName { get; set; }
    public string CurrencyImagepath { get; set; }
    public string ShortName { get; set; }
    public decimal Balance { get; set; }
    public decimal CreditBalance { get; set; }
    public string PartnerCode { get; set; }
}

public class DashBoardFeeAccount
{
    public string SourceCurrency { get; set; }
    public string CurrencyName { get; set; }
    public string SourceCurrencyImg { get; set; }
    public decimal Balance { get; set; }
    public string PartnerCode { get; set; }
}

public class DashBoardConversionRate
{
    public string SourceCurrency { get; set; }
    public string DestinationCurrency { get; set; }
    public string sourcecurrencyimg { get; set; }
    public string Destinationcurrencyimg { get; set; }
    public string UnitValue { get; set; }
    public string CurrentRate { get; set; }
    public string PartnerCode { get; set; }
}

public class DashBoardTransactionDetails
{
    public string PartnerCode { get; set; }
    public string SourceCurrency { get; set; }
    public string DestinationCurrency { get; set; }
    public string TransactionId { get; set; }
    public string SenderDetails { get; set; }
    public string RecipientDetails { get; set; }
    public string SendingAmount { get; set; }
    public string ServiceCharge { get; set; }
    public string PartnerServiceCharge { get; set; }
    public string ConversionRate { get; set; }
    public string NetSendingAmount { get; set; }
    public string NetReceivingAmount { get; set; }
    public string PaymentType { get; set; }
    public string GatewayStatus { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string SenderCreatedDate { get; set; }
    public string ReceiverCreatedDate { get; set; }
}

public class DashboardWalletBalance
{
    public string PartnerCode { get; set; }
    public string PartnerName { get; set; }
    public string WalletCurrency { get; set; }
    public string WalletBalanceText { get; set; }
    public string FeeBalanceText { get; set; }
}

public class DashboardPartnerSender
{
    public string PartnerCode { get; set; }
    public string SenderName { get; set; }
    public int TotalTrans { get; set; }
    public string VolumeText { get; set; }
}

public class DashboardTransactionStatus
{
    public string StatusName { get; set; }
    public int TotalTrans { get; set; }
    public string VolumeText { get; set; }
}

public class DashboardApproxDays
{
    public string PartnerCode { get; set; }
    public string PartnerName { get; set; }
    public string Wallet { get; set; }
    public string Balance { get; set; }
    public string Fee { get; set; }
    public string ApproxNoOfTrans { get; set; }
    public string ApproxNoOfDays { get; set; }  
}