using System.ComponentModel.DataAnnotations;

namespace Mpmt.Web.Areas.Admin.ViewModels.Paetner.WalletCurrency
{
    /// <summary>
    /// The partner wallet currency vm.
    /// </summary>
    public class PartnerWalletCurrencyVm : IValidatableObject
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the partner code.
        /// </summary>
        public string PartnerCode { get; set; }
        /// <summary>
        /// Gets or sets the source currency.
        /// </summary>
        public string SourceCurrency { get; set; }
        /// <summary>
        /// Gets or sets the destination currency.
        /// </summary>
        public string DestinationCurrency { get; set; }

        /// <summary>
        /// Gets or sets the notification balance.
        /// </summary>
        public decimal NotificationBalance { get; set; }

        [Required(ErrorMessage = "Credit limit is required!")]
        public decimal CreditLimit { get; set; }
        /// <summary>
        /// Gets or sets the markup min value.
        /// </summary>
        [Required(ErrorMessage = "Notification Balance is required!")]
        public decimal NotificationBalanceLimit { get; set; }

        /// <summary>
        /// Gets or sets the markup min value.
        /// </summary>
        [Required(ErrorMessage = "Markup min value is required!")]
        public decimal MarkupMinValue { get; set; }
        /// <summary>
        /// Gets or sets the markup max value.
        /// </summary>

        [Required(ErrorMessage = "Markup max value is required!")]
        public decimal MarkupMaxValue { get; set; }
        /// <summary>
        /// Gets or sets the type code.
        /// </summary>
        public string TypeCode { get; set; }
        /// <summary>
        /// Gets or sets the remarks.
        /// </summary>
        public string Remarks { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();
            if (string.IsNullOrWhiteSpace(SourceCurrency))
            {
                results.Add(new ValidationResult("Please Select Source Currency ", new[] { "SourceCurrency" }));

            }
            if (string.IsNullOrWhiteSpace(DestinationCurrency))
            {
                results.Add(new ValidationResult("Please Select Destination Currency ", new[] { "DestinationCurrency" }));

            }
            if (MarkupMinValue > MarkupMaxValue)
            {
                results.Add(new ValidationResult("Min value can not be greater than max ", new[] { "MarkupMinValue" }));

            }
            foreach (var result in results)
            {
                yield return result; // Return each ValidationResult using yield return
            }
        }
    }
}
