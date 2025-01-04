using System.ComponentModel.DataAnnotations;

namespace Mpmt.Agent.Models.TransactionSearch
{
    public class checkAgentTransactionPayoutModel
    {
        public string ControlNumber { get; set; }
        public string PaymentType { get; set; }
        public string PaymentStatus { get; set; }
        public string SourceCurrency { get; set; }
        public string DestinationCurrency { get; set; }
        public string PaymentAmountNPR { get; set; }
        public string SenderFullName { get; set; }
        public string SenderContactNumber { get; set; }
        public string SenderEmail { get; set; }
        public string SenderCountry { get; set; }
        public string SenderAddress { get; set; }
        public string SenderRelationWithReceiver { get; set; }
        public string ReceiverFullName { get; set; }
        public string ReceiverContactNumber { get; set; }
        public string ReceiverEmail { get; set; }
        public string ReceiverCountry { get; set; }
        public string ReceiverProvince { get; set; }
        public string ReceiverDistrict { get; set; }
        public string ReceiverLocalBody { get; set; }
        public string ReceiverAddress { get; set; }
        public string ReceiverRelationWithSender { get; set; }


        [Required(ErrorMessage = "Mobile number is required!")]
        public string MobileNumber { get; set; }
        [Required(ErrorMessage = "Indentification type is required!")]
        public string IdentificationType { get; set; }
        [Required(ErrorMessage = "Identification number is required!")]
        public string IndentificationNumber { get; set; }
        [Required(ErrorMessage = "Issue date is required!")]
        public string IssueDate { get; set; }
        [Required(ErrorMessage = "Expiry date is required!")]
        public string ExpiryDate { get; set; }
        public string UploadImage { get; set; }
        [Required(ErrorMessage = "Relationship is required!")]
        public string RelationShip { get; set; }
        [Required(ErrorMessage = "Transaction purpose is required!")]
        public string TransactionPurpose { get; set; }
    }
}
