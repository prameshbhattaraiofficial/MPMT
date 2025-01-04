namespace Mpmt.Web.Areas.Partner.Models.TransferAmount
{
    public class GetConvertedTransferAmount
    {
        public string SourceCurrency { get; set; }
        public string DestinationCurrency { get; set; }
        public string AccountType { get; set; }
        public string Remarks { get; set; } 
        public decimal SourceAmount { get; set; }
        public decimal DestinationAmount { get; set; }   
        public string OTP { get; set; }   
    }
}
