using Microsoft.AspNetCore.Http;

namespace Mpmt.Core.Dtos.Partner
{
    /// <summary>
    /// The add update fund request.
    /// </summary>
    public class AddUpdateFundRequest
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the event.
        /// </summary>
        public char Event { get; set; }
        //public string PartnerCode { get; set; }
        /// <summary>
        /// Gets or sets the fund type id.
        /// </summary>
        public int FundTypeId { get; set; }
        /// <summary>
        /// Gets or sets the wallet id.
        /// </summary>
        public int WalletId { get; set; }
        //public string SourceCurrency { get; set; }
        //public string DestinationCurrency { get; set; }
        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        public decimal Amount { get; set; }
        public decimal CreditLimit { get; set; }
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
        public IFormFile VoucherImg { get; set; }
        /// <summary>
        /// Gets or sets the remarks.
        /// </summary>
        //public int RequestStatusId { get; set; }
        //public bool Maker { get; set; }
        //public bool Checker { get; set; }
        /// <summary>
        /// Gets or sets the user type.
        /// </summary>
        public string UserType { get; set; }
        /// <summary>
        /// Gets or sets the logged in user.
        /// </summary>
        public string LoggedInUser { get; set; }

    }
}
