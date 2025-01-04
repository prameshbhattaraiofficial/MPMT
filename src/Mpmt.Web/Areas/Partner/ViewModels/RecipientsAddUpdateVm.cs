using System.ComponentModel.DataAnnotations;

namespace Mpmt.Web.Areas.Partner.ViewModels
{
    public class RecipientsAddUpdateVm
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
    }
    public class RecipientsAddUpdateViewmodel
    {
        [Required]
        public int SenderId { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public bool IsSurNamePresent { get; set; }
        [Required]
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        [Required]
        public int GenderId { get; set; }
        [Required]
        public DateTime? DateOfBirth { get; set; }
        [Required]
        public string CountryCode { get; set; }
        [Required]
        public string ProvinceCode { get; set; }
        [Required]
        public string DistrictCode { get; set; }
        [Required]
        public string LocalBodyCode { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Zipcode { get; set; }
        public string Address { get; set; }
        [Required]
        public string SourceCurrency { get; set; }
        [Required]
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
        [Required]
        public string GMTTimeZone { get; set; }
        public bool IsActive { get; set; }
    }
}
