using Mpmt.Core.Dtos.Roles;

namespace Mpmt.Core.Dtos.Partner;

public class AppPartner
{
    public string Event { get; set; }

    public string ApiUserName { get; set; }
    public string BusinessNumber { get; set; }
    public Guid UserGuid { get; set; }
    public string FinancialTransactionRegNo { get; set; }
    public string RemittancRegNumber { get; set; }
    public string LicenseNumber { get; set; }
    public string ZipCode { get; set; }
    public string OrgState { get; set; }
    public string ShortName { get; set; }
    public bool IsFirstNamePresent { get; set; }
    public bool EmailConfirmed { get; set; }
    public string PartnerTimeZone { get; set; }
    public List<string> LicensedocImgPath { get; set; }
    public int Id { get; set; }
    public string PartnerCode { get; set; }
    public string FirstName { get; set; }
    public string SurName { get; set; }
    public string MobileNumber { get; set; }
    public bool MobileConfirmed { get; set; }
    public string Email { get; set; }
    public string Post { get; set; }
    public int GenderId { get; set; }
    public int FundTypeId { get; set; }
    public string Address { get; set; }
    public string UserName { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
    public string AccessCodeHash { get; set; }
    public string AccessCodeSalt { get; set; }
    public string MPINHash { get; set; }
    public string MPINSalt { get; set; }
    public int ChargeCategoryId { get; set; }
    public string FundType { get; set; }
    public decimal CreditUptoLimitPerc { get; set; }
    public decimal CreditSendTxnLimit { get; set; }
    public decimal CashPayoutSendTxnLimit { get; set; }
    public decimal WalletSendTxnLimit { get; set; }
    public decimal BankSendTxnLimit { get; set; }
    public decimal NotificationBalanceLimit { get; set; }
    public bool TransactionApproval { get; set; }
    public string OrganizationName { get; set; }
    public string OrgEmail { get; set; }
    public bool OrgEmailConfirmed { get; set; }
    public string CountryCode { get; set; }
    public string City { get; set; }
    public string FullAddress { get; set; }
    public string GMTTimeZone { get; set; }
    public string GMTTimeZoneId { get; set; }
    public string RegistrationNumber { get; set; }
    public string SourceCurrency { get; set; }
    public string Website { get; set; }
    public string IpAddress { get; set; }
    public string IPAddress { get; set; }
    public string CompanyLogoImgPath { get; set; }
    public int DocumentTypeId { get; set; }
    public string DocumentNumber { get; set; }
    public string IdFrontImgPath { get; set; }
    public string IdBackImgPath { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public DateTime? IssueDate { get; set; }
    public DateTime? LastPasswordChangedUtcDate { get; set; }
    public int AddressProofTypeId { get; set; }
    public string AddressProofImgPath { get; set; }
    public string LastIpAddress { get; set; }
    public int DeviceId { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsBlocked { get; set; }
    public int FailedLoginAttempt { get; set; }
    public string TemporaryLockedTillUtcDate { get; set; }
    public string LastLoginDateUtc { get; set; }
    public string LastActivityDateUtc { get; set; }
    public string KycStatusCode { get; set; }
    public bool Is2FAAuthenticated { get; set; }
    public bool Is2FARequired { get; set; }
    public string AccountSecretKey { get; set; }
    public bool Maker { get; set; }
    public bool Checker { get; set; }
    public int CreatedById { get; set; }
    public string CreatedByName { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public int UpdatedById { get; set; }
    public string UpdatedByName { get; set; }
    public string UpdatedDate { get; set; }
    public string CategoryName { get; set; }
    public string CountryName { get; set; }
    public string DocumentType { get; set; }
    public string AddressProofName { get; set; }
    public List<Director> Directors { get; set; } = new List<Director>();
    public string Remarks { get; set; }
    public List<PartnerRoleDetail> PartnerRoles { get; set; } = new List<PartnerRoleDetail>();
}