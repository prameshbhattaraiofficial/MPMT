namespace Mpmt.Core.Domain.Partners.Recipient
{
    public class RecipientAddUpdate
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public bool IsSurNamePresent { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public int GenderId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string CountryCode { get; set; }
        public string ProvinceCode { get; set; }
        public string DistrictCode { get; set; }
        public string LocalBodyCode { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
        public string Address { get; set; }
        public string SourceCurrency { get; set; }
        public string DestinationCurrency { get; set; }
        public string BankName { get; set; }
        public string BankCode { get; set; }
        public string Branch { get; set; }
        public string AccountNumber { get; set; }
        public int RelationshipId { get; set; }
        public int PayoutTypeId { get; set; }
        public string AccountHolderName { get; set; }
        public string WalletName { get; set; }
        public string WalletId { get; set; }
        public string WalletRegisteredName { get; set; }
        public string GMTTimeZone { get; set; }
        public bool IsActive { get; set; }
        public string OperationMode { get; set; }
        public string LoggedInuser { get; set; }
        public string UserType { get; set; }
    }
}
