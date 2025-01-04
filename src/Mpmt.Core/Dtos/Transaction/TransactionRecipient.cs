namespace Mpmt.Core.Dtos.Transaction
{
    public class TransactionRecipient
    {
        public string TransactionId { get; set; }
        public string RecipientName { get; set; }
        public string RecipientId { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string LocalBody { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
        public string Address { get; set; }
        public string Relationship { get; set; }
        public string ReceivingAmount { get; set; }
        public string PaymentType { get; set; }
        public string WalletNumber { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public string WalletName { get; set; }
    }
}
