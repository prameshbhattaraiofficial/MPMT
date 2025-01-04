using Mpmt.Core.Dtos.Partner;

namespace Mpmt.Core.Domain.Partners.Register
{
    public class RegisterPartner
    {
        public int Id { get; set; }
        public string Event { get; set; }
        public int FormNumber { get; set; }
        public string OTP { get; set; }
        public DateTime? OtpExipiryDate { get; set; }
        public string PartnerCode { get; set; }
        public string FirstName { get; set; }
        //public string ShortName { get; set; }
        public string SurName { get; set; }
        public bool Withoutfirstname { get; set; }
        public string MobileNumber { get; set; }
        public bool MobileConfirmed { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Post { get; set; }
        public string BusinessNumber { get; set; }
        public string FinancialTransactionRegNo { get; set; }
        public string RemittancRegNumber { get; set; }
        public List<string> LicensedocImgPath { get; set; }
        public string LicenseNumber { get; set; }
        public string ZipCode { get; set; }
        public string OrgState { get; set; }
        public string Address { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string OrganizationName { get; set; }
        public string OrgEmail { get; set; }
        public bool OrgEmailConfirmed { get; set; }
        public string CountryCode { get; set; }
        public string Callingcode { get; set; }
        public string City { get; set; }
        public string FullAddress { get; set; }
        public string GMTTimeZone { get; set; }
        public string RegistrationNumber { get; set; }
        public string SourceCurrency { get; set; }
        public string IpAddress { get; set; }
        public string CompanyLogoImgPath { get; set; }
        public int DocumentTypeId { get; set; }
        public string DocumentNumber { get; set; }
        public string IdFrontImgPath { get; set; }
        public string IdBackImgPath { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime? IssueDate { get; set; }
        public int AddressProofTypeId { get; set; }
        public string AddressProofImgPath { get; set; }
        public bool IsActive { get; set; }
        public bool Maker { get; set; }
        public bool Checker { get; set; }
        public int CreatedById { get; set; }
        public string CreatedByName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int UpdatedById { get; set; }
        public string UpdatedByName { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public List<Director> Directors { get; set; }
    }
}
