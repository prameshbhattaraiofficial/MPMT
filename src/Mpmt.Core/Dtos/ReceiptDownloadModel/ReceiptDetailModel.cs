namespace Mpmt.Core.Dtos.ReceiptDownloadModel;

public class ReceiptDetailModel
{
    public string PartnerName { get; set; } 
    public string PartnerContact { get; set; } 
    public string PartnerAddress { get; set; } 
    public string TransactionId { get; set; } 
    public string GatewayTransactionId { get; set; }    
    public string TransactionDate { get; set; } 
    public string SendAmount { get; set; } 
    public string SendCurrency { get; set; }    
    public string SenderName { get; set; } 
    public string SenderAddress { get; set; } 
    public string SenderCountry { get; set; } 
    public string SenderContact { get; set; } 
    public string ReceiverName { get; set; } 
    public string ReceiverAddress { get; set; } 
    public string ReceiverContact { get; set; } 
    public string PaymentType { get; set; } 
    public string AccountNumber { get; set; } 
    public string BankName { get; set; } 
    public double ReceiveAmount { get; set; }   
    public string ReceiveAmountString { get; set; } 
    public string ReceiveCurrency { get; set; } 
    public string Status { get; set; } 
}
