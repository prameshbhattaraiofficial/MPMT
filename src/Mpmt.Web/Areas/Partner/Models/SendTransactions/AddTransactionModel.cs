using System.ComponentModel.DataAnnotations;

namespace Mpmt.Web.Areas.Partner.Models.SendTransactions
{
    public class AddTransactionModel : IValidatableObject
    {
        //public string PartnerCode { get; set; }

        [Required(ErrorMessage = "ProcessId is required.")]
        public string ProcessId { get; set; }

        [Required(ErrorMessage = "Please select source currency.")]
        public string SourceCurrency { get; set; }

        [Required(ErrorMessage = "Please select destination currency.")]
        public string DestinationCurrency { get; set; }
        [Required(ErrorMessage = "Invalid sending amount.")]
        [Range(minimum: 10, maximum: double.MaxValue, ErrorMessage = "Minimum sending amount should be 10.")]
        public decimal SendingAmount { get; set; }

        [Required(ErrorMessage = "Please select payout type.")]
        public string PaymentType { get; set; }

        //public decimal ServiceCharge { get; set; }
        //public decimal NetSendingAmount { get; set; }
        //public decimal ConversionRate { get; set; }
        //public decimal NetRecievingAmountNPR { get; set; }
        //public decimal PartnerServiceCharge { get; set; }
        public bool ExistingSender { get; set; }
        public string MemberId { get; set; }

        //[Required(ErrorMessage = "First name is required.")]
        public string SenderFirstName { get; set; }

        public bool NoSenderFirstName { get; set; }

        //[Required(ErrorMessage = "Last name is required.")]
        public string SenderLastName { get; set; }

        //[Required(ErrorMessage = "Contact number is required.")]
        public string SenderContactNumber { get; set; }

        //[Required(ErrorMessage = "Email is required.")]
        //[EmailAddress(ErrorMessage = "Invalid email address.")]
        public string SenderEmail { get; set; }

        //[Required(ErrorMessage = "Please select a country.")]
        public string SenderCountryCode { get; set; }

        public string SenderProvince { get; set; }

        //[Required(ErrorMessage = "City is required.")]
        public string SenderCity { get; set; }

        public string SenderZipcode { get; set; }

        //[Required(ErrorMessage = "Address is required.")]
        public string SenderAddress { get; set; }

        //[Required(ErrorMessage = "Please select a relationship.")]
        //public int SenderRelationshipId { get; set; }
        public string SenderRelationshipId { get; set; }

        public IFormFile SenderIdProofImg { get; set; }
        public IFormFile SenderIdProofImg2 { get; set; }

        //[Required(ErrorMessage = "Please select a purpose.")]
        //public int SenderPurposeId { get; set; }
        public string SenderPurposeId { get; set; }

        public string SenderRemarks { get; set; }

        public bool ExistingRecipient { get; set; }

        public string RecipientId { get; set; }
 
        public string RecipientType { get; set; }

       // [Required(ErrorMessage = "First name is required.")]
        public string RecipientFirstName { get; set; }
        public bool NoRecipientFirstName { get; set; }

        public string RecipientLastName { get; set; }

        public string JointAccountFirstName { get; set; }
        public bool NoJointAccountFirstName { get; set; }

        public string JointAccountLastName { get; set; }

        public string BusinessName { get; set; }

        [StringLength(10, MinimumLength = 10, ErrorMessage = "Entered contact number is not valid.")]
        public string RecipientContactNumber { get; set; }

        public string RecipientEmail { get; set; }

        public DateTime? RecipientDateOfBirth { get; set; }

        public string RecipientCountryCode { get; set; }

        //[Required]
        public string RecipientProvinceCode { get; set; }

        
        public string RecipientDistrictCode { get; set; }

        public string RecipientLocalBodyCode { get; set; }

        public string RecipientCity { get; set; }

        public string RecipientZipcode { get; set; }

       
        public string RecipientAddress { get; set; }

       
       // public int RecipientRelationshipId { get; set; }
        public string RecipientRelationshipId { get; set; }

        public string BankName { get; set; }

        public string BankCode { get; set; }

        public string Branch { get; set; }

        public string AccountHolderName { get; set; }

        public string AccountNumber { get; set; }

        public string WalletCode { get; set; }

        public string WalletName { get; set; }

        public string WalletNumber { get; set; }

        public string WalletHolderName { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNumber { get; set; }
        public string SenderOccupation { get; set; }
        public string SourceOfIncome { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();
            if (!ExistingRecipient)
            {
                if(RecipientRelationshipId == "0")
                {
                    results.Add(new ValidationResult("Please select a relationship.", new[] { "RecipientRelationshipId" }));
                }
                if(string.IsNullOrEmpty(RecipientAddress))
                {
                    results.Add(new ValidationResult("Please enter a full address.", new[] { "RecipientAddress" }));
                }
                if(string.IsNullOrEmpty(RecipientDistrictCode))
                {
                    results.Add(new ValidationResult("Please select a district.", new[] { "RecipientDistrictCode" }));
                }
                if(string.IsNullOrEmpty(RecipientDistrictCode))
                {
                    results.Add(new ValidationResult("Please select a country.", new[] { "RecipientCountryCode" }));
                }
                if(string.IsNullOrEmpty(RecipientContactNumber))
                {
                    results.Add(new ValidationResult("Contact number is required.", new[] { "RecipientContactNumber" }));
                }
                if(string.IsNullOrEmpty(RecipientLastName))
                {
                    results.Add(new ValidationResult("Last name is required.", new[] { "RecipientLastName" }));
                }
                if(string.IsNullOrEmpty(RecipientType))
                {
                    results.Add(new ValidationResult("Please select a recipient type.", new[] { "RecipientType" }));
                }
               

            }
            if(!ExistingSender)
            {

                if (string.IsNullOrEmpty(SenderLastName))
                {
                    results.Add(new ValidationResult("Last name is required.", new[] { "SenderLastName" }));
                }
                if (string.IsNullOrEmpty(SenderContactNumber))
                {
                    results.Add(new ValidationResult("Contact number is required.", new[] { "SenderContactNumber" }));
                }
                if (string.IsNullOrEmpty(SenderCountryCode))
                {
                    results.Add(new ValidationResult("Please select a country.", new[] { "SenderCountryCode" }));
                }
                if (string.IsNullOrEmpty(SenderCity))
                {
                    results.Add(new ValidationResult("City is required.", new[] { "SenderCity" }));
                }
                if (string.IsNullOrEmpty(SenderAddress))
                {
                    results.Add(new ValidationResult("Sender full address is required.", new[] { "SenderAddress" }));
                }
                if (SenderRelationshipId == "0")
                {
                    results.Add(new ValidationResult("Please select a relationship.", new[] { "SenderRelationshipId" }));
                }
                if (SenderPurposeId == "0")
                {
                    results.Add(new ValidationResult("Please select a purpose.", new[] { "SenderPurposeId" }));
                }
                if(DocumentType == "0")
                {
                    results.Add(new ValidationResult("Please select a document type.", new[] { "DocumentType" }));
                }
                if (string.IsNullOrEmpty(DocumentNumber))
                {
                    results.Add(new ValidationResult("Document number is required.", new[] { "DocumentNumber" }));
                }
                if (SenderOccupation == "0")
                {
                    results.Add(new ValidationResult("Please select a sender occupation.", new[] { "SenderOccupation" }));
                }
                if (SourceOfIncome == "0")
                {
                    results.Add(new ValidationResult("Please select a source of income.", new[] { "SourceOfIncome" }));
                }
            }

            foreach (var result in results)
            {
                yield return result; // Return each ValidationResult using yield return
            }
        }
    }
}
