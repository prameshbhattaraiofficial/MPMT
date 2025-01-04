using Microsoft.Build.Framework;
using Mpmt.Core.Dtos.Partner;

namespace Mpmt.Web.Areas.Admin.ViewModel.AddPartner
{
    /// <summary>
    /// The add patner dto.
    /// </summary>
    public class AddPatnerDto
    {
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        [Required]
        public string FirstName { get; set; }
        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether continue without surname.
        /// </summary>
        public bool ContinueWithoutFirstName { get; set; }
        /// <summary>
        /// Gets or sets the number.
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Gets or sets the post.
        /// </summary>
        public string Post { get; set; }
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Gets or sets the confirm password.
        /// </summary>
        public string ConfirmPassword { get; set; }
        /// <summary>
        /// Gets or sets the service.
        /// </summary>
        public string Service { get; set; }
        /// <summary>
        /// Gets or sets the balance type.
        /// </summary>
        public string BalanceType { get; set; }
        /// <summary>
        /// Gets or sets the credit send txn limit flat.
        /// </summary>
        public decimal CreditSendTxnLimitFlat { get; set; }
        /// <summary>
        /// Gets or sets the credit send txn limit percent.
        /// </summary>
        public decimal CreditSendTxnLimitPercent { get; set; }
        /// <summary>
        /// Gets or sets the credit send txn limit.
        /// </summary>
        public decimal CreditSendTxnLimit { get; set; }
        /// <summary>
        /// Gets or sets the send txn limit cash payout.
        /// </summary>
        public decimal SendTxnLimitCashPayout { get; set; }
        /// <summary>
        /// Gets or sets the send txn limit wallet.
        /// </summary>
        public decimal SendTxnLimitWallet { get; set; }
        /// <summary>
        /// Gets or sets the send txn limit bank.
        /// </summary>
        public decimal SendTxnLimitBank { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether txn approve is active.
        /// </summary>
        public bool IsTxnApproveActive { get; set; }
        /// <summary>
        /// Gets or sets the organization name.
        /// </summary>
        public string OrganizationName { get; set; }
        /// <summary>
        /// Gets or sets the company registration number.
        /// </summary>
        public string CompanyRegistrationNumber { get; set; }
        /// <summary>
        /// Gets or sets the company email.
        /// </summary>
        public string CompanyEmail { get; set; }
        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// Gets or sets the full address.
        /// </summary>
        public string FullAddress { get; set; }
        /// <summary>
        /// Gets or sets the g m t time zone.
        /// </summary>
        public string GMTTimeZone { get; set; }
        /// <summary>
        /// Gets or sets the local source currency.
        /// </summary>
        public string LocalSourceCurrency { get; set; }
        /// <summary>
        /// Gets or sets the i p address.
        /// </summary>
        public string IPAddress { get; set; }
        /// <summary>
        /// Gets or sets the company logo.
        /// </summary>
        public IFormFile CompanyLogo { get; set; }
        /// <summary>
        /// Gets or sets the license document.
        /// </summary>
        public IFormFile LicenseDocument { get; set; }
        /// <summary>
        /// Gets or sets the directors.
        /// </summary>
        public List<Director> Directors { get; set; }
        /// <summary>
        /// Gets or sets the document type.
        /// </summary>
        public string DocumentType { get; set; }
        /// <summary>
        /// Gets or sets the document number.
        /// </summary>
        public string DocumentNumber { get; set; }
        /// <summary>
        /// Gets or sets the expiry date.
        /// </summary>
        public DateTime ExpiryDate { get; set; }
        /// <summary>
        /// Gets or sets the i d front.
        /// </summary>
        public IFormFile IDFront { get; set; }
        /// <summary>
        /// Gets or sets the i d back.
        /// </summary>
        public IFormFile IDBack { get; set; }
        /// <summary>
        /// Gets or sets the address prof type.
        /// </summary>
        public string AddressProfType { get; set; }
        /// <summary>
        /// Gets or sets the address prof image.
        /// </summary>
        public IFormFile AddressProfImage { get; set; }
    }
    /// <summary>
    /// The director.
    /// </summary>
    //public class Director
    //{
    //    /// <summary>
    //    /// Gets or sets the full name.
    //    /// </summary>
    //    public string FullName { get; set; }
    //    /// <summary>
    //    /// Gets or sets the contact.
    //    /// </summary>
    //    public string Contact { get; set; }
    //    /// <summary>
    //    /// Gets or sets the email.
    //    /// </summary>
    //    public string Email { get; set; }
    //}










}
