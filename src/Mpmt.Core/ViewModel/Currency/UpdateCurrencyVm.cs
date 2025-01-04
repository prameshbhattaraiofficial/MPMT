using Microsoft.AspNetCore.Http;
using Mpmt.Core.Common.Attribites;

namespace Mpmt.Core.ViewModel.Currency
{
    /// <summary>
    /// The update currency vm.
    /// </summary>
    public class UpdateCurrencyVm
    {
       
        public int Id { get; set; }
     
        public string CurrencyName { get; set; }
        
        public string ShortName { get; set; }
        
        public string Symbol { get; set; }
       
        public string CountryCode { get; set; }
        public string CurrencyImagePath { get; set; }   

        [MaxFileSize(1 * 1024 * 1024)]
        [AllowedExtensions(ErrorMessage = "File not allowed")]
        public IFormFile CurrencyImage { get; set; }
       
        public bool IsActive { get; set; }
    }
}
