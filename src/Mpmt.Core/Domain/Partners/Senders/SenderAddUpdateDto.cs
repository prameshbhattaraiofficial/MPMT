namespace Mpmt.Core.Domain.Partners.Senders
{
    public class SenderAddUpdateDto
    {
        public int Id { get; set; }
        public string PartnerCode { get; set; }
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public bool IsFirstNamePresent { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public int GenderId { get; set; }
        public string ProfileImagePath { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string CountryCode { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
        public string Address { get; set; }
        public int OccupationId { get; set; }
        public int DocumentTypeId { get; set; }
        public string DocumentNumber { get; set; }
        public DateTime? IssuedDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string IdFrontImgPath { get; set; }
        public string IdBackImgPath { get; set; }
        public string BankName { get; set; }
        public string BankCode { get; set; }
        public string Branch { get; set; }
        public string AccountHolderName { get; set; }
        public string AccountNumber { get; set; }
        public int IncomeSourceId { get; set; }
        public string GMTTimeZone { get; set; }
        public bool IsActive { get; set; }
        public string LoggedInUser { get; set; }
        public string UserType { get; set; }
    }
}
