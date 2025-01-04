namespace Mpmt.Core.Dtos.PartnerApi
{
    public class TransactionStatusRequest
    {
        public string ApiUserName { get; set; }

        public string TransactionId { get; set; }

        public string Signature { get; set; }
    }
}
