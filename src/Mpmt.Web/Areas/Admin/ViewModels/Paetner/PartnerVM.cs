namespace Mpmt.Web.Areas.Admin.ViewModels.Paetner
{
    /// <summary>
    /// The partner v m.
    /// </summary>
    public class PartnerVM
    {
        /// <summary>
        /// Gets or sets the event.
        /// </summary>
        public string Event { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether email confirmed.
        /// </summary>
        public bool EmailConfirmed { get; set; }
        /// <summary>
        /// Gets or sets the partner time zone.
        /// </summary>
        public string PartnerTimeZone { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the partner code.
        /// </summary>
        public string PartnerCode { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the sur name.
        /// </summary>
        public string SurName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether sur name is present.
        /// </summary>
        public bool IsFirstNamePresent { get; set; }

        /// <summary>
        /// Gets or sets the mobile number.
        /// </summary>
        public string MobileNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether mobile confirmed.
        /// </summary>
        public bool MobileConfirmed { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the post.
        /// </summary>
        public string Post { get; set; }

        /// <summary>
        /// Gets or sets the gender id.
        /// </summary>
        public int GenderId { get; set; }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password hash.
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// Gets or sets the password salt.
        /// </summary>
        public string PasswordSalt { get; set; }

        /// <summary>
        /// Gets or sets the access code hash.
        /// </summary>
        public string AccessCodeHash { get; set; }

        /// <summary>
        /// Gets or sets the access code salt.
        /// </summary>
        public string AccessCodeSalt { get; set; }

        /// <summary>
        /// Gets or sets the m p i n hash.
        /// </summary>
        public string MPINHash { get; set; }

        /// <summary>
        /// Gets or sets the m p i n salt.
        /// </summary>
        public string MPINSalt { get; set; }

        /// <summary>
        /// Gets or sets the charge category id.
        /// </summary>
        public int ChargeCategoryId { get; set; }

        /// <summary>
        /// Gets or sets the fund type id.
        /// </summary>
        public int FundTypeId { get; set; }

        /// <summary>
        /// Gets or sets the credit upto limit perc.
        /// </summary>
        public decimal CreditUptoLimitPerc { get; set; }

        /// <summary>
        /// Gets or sets the credit send txn limit.
        /// </summary>
        public decimal CreditSendTxnLimit { get; set; }

        /// <summary>
        /// Gets or sets the cash payout send txn limit.
        /// </summary>
        public decimal CashPayoutSendTxnLimit { get; set; }

        /// <summary>
        /// Gets or sets the wallet send txn limit.
        /// </summary>
        public decimal WalletSendTxnLimit { get; set; }

        /// <summary>
        /// Gets or sets the bank send txn limit.
        /// </summary>
        public decimal BankSendTxnLimit { get; set; }

        /// <summary>
        /// Gets or sets the notification balance limit.
        /// </summary>
        public decimal NotificationBalanceLimit { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether transaction approval.
        /// </summary>
        public bool TransactionApproval { get; set; }

        /// <summary>
        /// Gets or sets the organization name.
        /// </summary>
        public string OrganizationName { get; set; }

        /// <summary>
        /// Gets or sets the org email.
        /// </summary>
        public string OrgEmail { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether org email confirmed.
        /// </summary>
        public bool OrgEmailConfirmed { get; set; }

        /// <summary>
        /// Gets or sets the country code.
        /// </summary>
        public string CountryCode { get; set; }
        public string CountryName { get; set; }

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
        /// Gets or sets the registration number.
        /// </summary>
        public string RegistrationNumber { get; set; }

        /// <summary>
        /// Gets or sets the source currency.
        /// </summary>
        public string SourceCurrency { get; set; }

        /// <summary>
        /// Gets or sets the website.
        /// </summary>
        public string Website { get; set; }

        /// <summary>
        /// Gets or sets the ip address.
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// Gets or sets the company logo img path.
        /// </summary>
        public string CompanyLogoImgPath { get; set; }

        /// <summary>
        /// Gets or sets the document type id.
        /// </summary>
        public int DocumentTypeId { get; set; }

        /// <summary>
        /// Gets or sets the document number.
        /// </summary>
        public string DocumentNumber { get; set; }

        /// <summary>
        /// Gets or sets the id front img path.
        /// </summary>
        public string IdFrontImgPath { get; set; }
        public string LicensedocImgPath { get; set; }

        /// <summary>
        /// Gets or sets the id back img path.
        /// </summary>
        public string IdBackImgPath { get; set; }

        /// <summary>
        /// Gets or sets the expiry date.
        /// </summary>
        public DateTime? ExpiryDate { get; set; }
        public DateTime? IssueDate { get; set; }

        /// <summary>
        /// Gets or sets the address proof type id.
        /// </summary>
        public int AddressProofTypeId { get; set; }

        /// <summary>
        /// Gets or sets the address proof img path.
        /// </summary>
        public string AddressProofImgPath { get; set; }

        /// <summary>
        /// Gets or sets the last ip address.
        /// </summary>
        public string LastIpAddress { get; set; }

        /// <summary>
        /// Gets or sets the device id.
        /// </summary>
        public int DeviceId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is deleted.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is blocked.
        /// </summary>
        public bool IsBlocked { get; set; }

        /// <summary>
        /// Gets or sets the failed login attempt.
        /// </summary>
        public int FailedLoginAttempt { get; set; }

        /// <summary>
        /// Gets or sets the temporary locked till utc date.
        /// </summary>
        public string TemporaryLockedTillUtcDate { get; set; }

        /// <summary>
        /// Gets or sets the last login date utc.
        /// </summary>
        public string LastLoginDateUtc { get; set; }

        /// <summary>
        /// Gets or sets the last activity date utc.
        /// </summary>
        public string LastActivityDateUtc { get; set; }

        /// <summary>
        /// Gets or sets the kyc status code.
        /// </summary>
        public string KycStatusCode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is2 f a authenticated.
        /// </summary>
        public bool Is2FAAuthenticated { get; set; }

        /// <summary>
        /// Gets or sets the account secret key.
        /// </summary>
        public string AccountSecretKey { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether maker.
        /// </summary>
        public bool Maker { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether checker.
        /// </summary>
        public bool Checker { get; set; }

        /// <summary>
        /// Gets or sets the created by id.
        /// </summary>
        public int CreatedById { get; set; }

        /// <summary>
        /// Gets or sets the created by name.
        /// </summary>
        public string CreatedByName { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets the updated by id.
        /// </summary>
        public int UpdatedById { get; set; }

        /// <summary>
        /// Gets or sets the updated by name.
        /// </summary>
        public string UpdatedByName { get; set; }

        /// <summary>
        /// Gets or sets the updated date.
        /// </summary>
        public string UpdatedDate { get; set; }
        /// <summary>
        /// Gets or sets the directors.
        /// </summary>
        //public List<Director> Directors { get; set; }


    }
    /// <summary>
    /// The director.
    /// </summary>
    public class Director
    {

        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// Gets or sets the contact.
        /// </summary>
        public string Contact { get; set; }
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public string Email { get; set; }
    }
}
