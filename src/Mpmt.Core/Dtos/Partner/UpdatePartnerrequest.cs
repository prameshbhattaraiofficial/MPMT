using Microsoft.AspNetCore.Http;
using Mpmt.Core.Common.Attribites;
using System.ComponentModel.DataAnnotations;

namespace Mpmt.Core.Dtos.Partner;

public class UpdatePartnerrequest
{
    public string BusinessNumber { get; set; }
    public string FinancialTransactionRegNo { get; set; }
    public string RemittancRegNumber { get; set; }
    public string LicenseNumber { get; set; }
    public string ZipCode { get; set; }
    [Required(ErrorMessage = "State is required")]
    public string OrgState { get; set; }
    public bool IsFirstNamePresent { get; set; }
    public List<string> LicensedocImgPath { get; set; }
    public List<string> DeletedLicensedocImgPath { get; set; }
    [MaxFileSize]
    [AllowedExtensions]
    public List<IFormFile> LicenseDocument { get; set; }
    public string CallingCode { get; set; }
    public int Id { get; set; }
    public string PartnerCode { get; set; }
    public string FirstName { get; set; }
    [Required(ErrorMessage = "Last Name Is Required")]
    public string SurName { get; set; }
    public bool IsSurNamePresent { get; set; }
    [Required(ErrorMessage = "Phone Number is required.")]
    [RegularExpression("^[\\+]?[(]?[0-9]{3}[)]?[-\\s\\.]?[0-9]{3}[-\\s\\.]?[0-9]{4,6}$", ErrorMessage = "Please input valid phone number")]
    public string MobileNumber { get; set; }
    public bool MobileConfirmed { get; set; }
    [Required(ErrorMessage = "Email is Required")]
    [DataType(DataType.EmailAddress, ErrorMessage = "Please Input Valid Email")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Position is required")]
    public string Post { get; set; }
    public int GenderId { get; set; }
    public int FundTypeId { get; set; }
    public string Address { get; set; }
    public string UserName { get; set; }
    public int ChargeCategoryId { get; set; }
    public string FundType { get; set; }
    [Required(ErrorMessage = "Invalid Transaction Limit")]
    public decimal CreditUptoLimitPerc { get; set; }
    [Required(ErrorMessage = "Invalid Transaction Limit")]
    public decimal CreditSendTxnLimit { get; set; }
    [Required(ErrorMessage = "Invalid Transaction Limit")]
    public decimal CashPayoutSendTxnLimit { get; set; }
    [Required(ErrorMessage = "Invalid Transaction Limit")]
    public decimal WalletSendTxnLimit { get; set; }
    [Required(ErrorMessage = "Invalid Transaction Limit")]
    public decimal BankSendTxnLimit { get; set; }
    [Required(ErrorMessage = "Invalid Transaction Limit")]
    public decimal NotificationBalanceLimit { get; set; }
    public bool TransactionApproval { get; set; }
    [Required(ErrorMessage = "Organization name is required")]
    public string OrganizationName { get; set; }
    [Required(ErrorMessage = "Organization email is required")]
    [DataType(DataType.EmailAddress, ErrorMessage = "Please input valid email")]
    public string OrgEmail { get; set; }
    public bool OrgEmailConfirmed { get; set; }
    public string CountryCode { get; set; }
    [Required(ErrorMessage = "City is required")]
    public string City { get; set; }
    [Required(ErrorMessage = "Address is required")]
    public string FullAddress { get; set; }
    [Required(ErrorMessage = "Time Zone is required")]
    public string GMTTimeZone { get; set; }
    public string GMTTimeZoneId { get; set; }
    [Required(ErrorMessage = "Registration number is required")]
    public string RegistrationNumber { get; set; }
    [Required(ErrorMessage = "Source currency is required")]
    public string SourceCurrency { get; set; }
    public string Website { get; set; }
    public string CompanyLogoImgPath { get; set; }
    [MaxFileSize]
    [AllowedExtensions]
    public IFormFile CompanyLogo { get; set; }
    [MaxFileSize]
    [AllowedExtensions]
    public IFormFile IDFront { get; set; }
    [MaxFileSize]
    [AllowedExtensions]
    public IFormFile IDBack { get; set; }
    [MaxFileSize]
    [AllowedExtensions]
    public IFormFile AddressProfImage { get; set; }
    [Required(ErrorMessage = "Invalid Document Type")]
    public int DocumentTypeId { get; set; }
    [Required(ErrorMessage = "Document Number is required")]
    public string DocumentNumber { get; set; }
    public string IdFrontImgPath { get; set; }
    public string IdBackImgPath { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public DateTime? IssueDate { get; set; }
    [Required(ErrorMessage = "Invalid Address Type")]
    public int AddressProofTypeId { get; set; }
    public string AddressProofImgPath { get; set; }
    public string LastIpAddress { get; set; }
    public int DeviceId { get; set; }
    public bool IsActive { get; set; }
    public string UpdatedDate { get; set; }
    public string CategoryName { get; set; }
    public string CountryName { get; set; }
    public string DocumentType { get; set; }
    public string AddressProofName { get; set; }
    public List<Director> Directors { get; set; }
}
