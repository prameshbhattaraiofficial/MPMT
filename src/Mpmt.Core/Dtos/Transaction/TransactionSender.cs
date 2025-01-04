namespace Mpmt.Core.Dtos.Transaction
{
    public class TransactionSender
    {
        public string TransactionId { get; set; }
        public string SenderName { get; set; }
        public string MemberId { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
        public string Address { get; set; }
        public string Relationship { get; set; }
        public string PurposeName { get; set; }
        public string Remarks { get; set; }
        public string SendingAmount { get; set; }
    }
}
