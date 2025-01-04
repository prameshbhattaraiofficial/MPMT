using Microsoft.AspNetCore.Http;
using Mpmt.Core.Common.Attribites;
using System.ComponentModel.DataAnnotations;

namespace Mpmt.Core.ViewModel.Currency
{
    /// <summary>
    /// The add currency vm.
    /// </summary>
    public class AddCurrencyVm
    {
        /// <summary>
        /// Gets or sets the currency name.
        /// </summary>
        [Required]
        public string CurrencyName { get; set; }
        /// <summary>
        /// Gets or sets the short name.
        /// </summary>
        [Required]
        public string ShortName { get; set; }
        /// <summary>
        /// Gets or sets the symbol.
        /// </summary>
        [Required]
        public string Symbol { get; set; }
        /// <summary>
        /// Gets or sets the country code.
        /// </summary>
        [Required]
        public string CountryCode { get; set; }
        [Required]
        [MaxFileSize(5 * 1024 * 1024)]
        [AllowedExtensions(ErrorMessage = "File not allowed")]
        public IFormFile CurrencyImage { get; set; }
        public bool IsActive { get; set; }
    }
}
