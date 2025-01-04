using System.ComponentModel.DataAnnotations;

namespace Mpmt.Core.ViewModel.AdminReport
{
    public class ReceiverAccountDetailsVM
    {
        public string TransactionId { get; set; }
        [Required(ErrorMessage = "Receiver name is required!")]
        public string AccountHolderName { get; set; }
        [Required(ErrorMessage = "Account number is required!")]
        public string AccountNumber { get; set; }
        [Required(ErrorMessage = "Wallet number is required!")]
        [RegularExpression(@"(\+977)?[9][6-9]\d{8}", ErrorMessage = "Mobile Number is Invalid")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Mobile Number is Invalid")]
        public string WalletNumber { get; set; }
        [Required(ErrorMessage = "Bank name is required!")]
        public string BankName { get; set; }
        [Required(ErrorMessage = "Branch name is required!")]
        public string Branch { get; set; }
        public string BankCode { get; set; }
        public string PaymentType { get; set; }
        [Required(ErrorMessage = "Receiver name is required!")]
        public string WalletHolderName { get; set; }
        public string TxnUpdateRemarks { get; set; }
        public string PaymentStatusCode { get; set; }
        public string GatewayTxnId { get; set; }
        public string logedinUser { get; set; }
        public string UserType { get; set; }
    }

    public class ReceiverAccountDetailsCashoutVM
    {
        public string TransactionId { get; set; }
        public string ReceiverName { get; set; }
        public string ContactNumber { get; set; }
        public string DistrictCode { get; set; }
        public string FullAddress { get; set; }
        public string logedinUser { get; set; }
        public string UserType { get; set; }
        public string Remarks { get; set; } 
    }
}
