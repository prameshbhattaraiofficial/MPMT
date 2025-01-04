using System.ComponentModel.DataAnnotations;

namespace Mpmt.Web.Areas.Partner.Models.SendTransactions
{
    public class GetSendTxnChargeAmountDetailsModel
    {
        [Required]
        public string SourceCurrency { get; set; }

        [Required]
        public string SourceAmount { get; set; }

        [Required]
        public string PaymentType { get; set; }

        [Required]
        public string DestinationCurrency { get; set; }
    }
}
