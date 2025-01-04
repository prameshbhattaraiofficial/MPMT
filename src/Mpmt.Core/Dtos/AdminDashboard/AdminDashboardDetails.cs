using Mpmt.Core.Dtos.Partner;

namespace Mpmt.Core.Dtos.AdminDashboard;

public class AdminDashboardDetails
{
    public int AdminUserCount { get; set; }
    public int AdminUserIncrement { get; set; }
    public decimal AdminUserIncrementPercentage { get; set; }
    public int TotalPartner { get; set; }
    public int PartnerIncrement { get; set; }
    public decimal PartnerPercentage { get; set; }
    public int TotalPartnerBank { get; set; }
    public int PartnerBankIncrement { get; set; }
    public decimal PartnerBankPercentage { get; set; }
    public int TotalPartnerUser { get; set; }
    public int PartnerUserIncrement { get; set; }
    public decimal PartnerUserPercentage { get; set; }
    public decimal TotalSendingAmount { get; set; }
    public decimal LastMonthTransactionAmount { get; set; }
    public decimal TargetSendingAmount { get; set; }
    public decimal LastWeekTransaction { get; set; }
    public decimal TodaysTransaction { get; set; }
    public decimal ThisYearTransaction { get; set; }
    public decimal ThisWeekTransaction { get; set; }
    public IEnumerable<DashboardActivityLog> ActivityLog { get; set; }
    public IEnumerable<DashboardExchangeRate> ExchangeRate { get; set; }
    public IEnumerable<DashBoardTransactionReport> TransactionReport { get; set; }

    public List<FrequencyWiseTransaction> frequencyWiseTransactions { get; set; }
    public List<DashboardApproxDays> dashboardPartnerBalance { get; set; }
    public List<DashboardTransactionStatus> dashboardTransactionStatus { get; set; }
    public List<DashboardAdminTopPartner> dashboardTopPartner { get; set; }
    public List<DashboardAdminTopAgent> dashboardTopAgent { get; set; }
    public List<DashboardTopAgentLocation> dashboardTopAgentLocation { get; set; }
    public List<DashboardAdminPaymentMode> dashboardPaymentMode { get; set; }
    public List<DashboardAdminPaymentMode> dashboardThresholdTrans { get; set; }

    public List<string> Frequency { get; set; } 
    public List<string> Labels { get; set; }
    public List<double> TransactionData { get; set; }
    public List<double> VolumeData { get; set; }

    public List<string> PieChartLabels { get; set; }
    public List<int> PieChartData { get; set; }
}

public class DashboardActivityLog
{
    public string ActivityLog { get; set; }
    public string TimeSpan { get; set; }
}

public class DashboardExchangeRate
{
    public DateTime? CreatedDate { get; set; }
    public string SourceCurrency { get; set; }
    public string CurrencyImagepath { get; set; }
    public string UnitValue { get; set; }
    public string BuyingRate { get; set; }
    public string SellingRate { get; set; }
    public string CurrentRate { get; set; }
    public string RateDifference { get; set; }
    public bool NegetiveStatus { get; set; }
}

public class DashBoardTransactionReport
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

public class DashboardAdminTopPartner
{
    public string PartnerCode { get; set; }
    public string PartnerName { get; set; }
    public int TotalTrans { get; set; }
    public string VolumeText { get; set; }
}

public class DashboardAdminTopAgent
{
    public string AgentCode { get; set; }
    public string AgentName { get; set; }
    public int TotalTrans { get; set; }
    public string VolumeText { get; set; }
}

public class DashboardAdminPaymentMode
{
    public string PaymentMode { get; set; }
    public int TotalTrans { get; set; }
    public string VolumeText { get; set; }
}

public class DashboardTopAgentLocation
{
    public string District { get; set; }    
    public int TotalTrans { get; set; }
    public string VolumeText { get; set; }
}