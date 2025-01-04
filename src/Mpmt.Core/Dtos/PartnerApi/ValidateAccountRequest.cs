namespace Mpmt.Core.Dtos.PartnerApi
{
    public class ValidateAccountRequest
    {
        public string ApiUserName { get; set; }
        public string ReferenceId { get; set; }
        public string PaymentType { get; set; }
        public string WalletCode { get; set; }
        public string Amount { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string BankCode { get; set; }
        public string Signature { get; set; }
    }
}
