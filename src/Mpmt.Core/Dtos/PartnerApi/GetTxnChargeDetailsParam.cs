namespace Mpmt.Core.Dtos.PartnerApi
{
    public class GetTxnChargeDetailsParam
    {
        public string PartnerCode { get; set; }
        public string SourceCurrency { get; set; }
        public string SourceAmount { get; set; }
        public string PaymentType { get; set; }
        public string DestinationCurrency { get; set; }
    }
}
