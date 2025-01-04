namespace Mpmt.Core.Dtos.PartnerApi
{
    public class GetTxnChargeDetailsRequest
    {
        public string ApiUserName { get; set; }

        public string SourceCurrency { get; set; }

        public string SourceAmount { get; set; }

        public string PaymentType { get; set; }

        public string DestinationCurrency { get; set; }

        public string Signature { get; set; }
    }
}
