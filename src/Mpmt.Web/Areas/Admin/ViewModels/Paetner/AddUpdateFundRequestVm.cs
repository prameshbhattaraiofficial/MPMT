using Mpmt.Core.Common.Attribites;

namespace Mpmt.Web.Areas.Admin.ViewModels.Paetner
{
    /// <summary>
    /// The add update fund request vm.
    /// </summary>
    public class AddUpdateFundRequestVm
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the wallet id.
        /// </summary>
        public int WalletId { get; set; }
        /// <summary>
        /// Gets or sets the partner code.
        /// </summary>  
        public string PartnerCode { get; set; }
        public string Currency { get; set; }
        public string PartnerFullName { get; set; }
        public string Organization { get; set; }    
        /// <summary>
        /// Gets or sets the fund type id.
        /// </summary>
        public int FundTypeId { get; set; }
        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        public decimal Amount { get; set; }
        public decimal CreditLimitAmount { get; set; }
        /// <summary>
        /// Gets or sets the sign.
        /// </summary>
        public string Sign { get; set; }
        /// <summary>
        /// Gets or sets the transaction id.
        /// </summary>
        public string TransactionId { get; set; }
        /// <summary>
        /// Gets or sets the voucher img path.
        /// </summary>
        public string VoucherImgPath { get; set; }

        public string Remarks { get; set; }
        /// <summary>
        /// Gets or sets the voucher img.
        /// </summary>

        [MaxFileSize]
        [AllowedExtensions]
        public IFormFile? VoucherImg { get; set; }
        /// <summary>
        /// Gets or sets the remarks.
        /// </summary>
    }
}
