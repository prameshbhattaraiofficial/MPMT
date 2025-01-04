namespace Mpmt.Core.Dtos.AgentApi
{
    public class CheckPayoutStatusDetail
    {
        public string PaymentStatusCode { get; set; }
        public string PaymentStatusRemarks { get; set; }
        public string TransactionId { get; set; }
        public string AmountNPR { get; set; }
        public string SourceCurrency { get; set; }
        public string DestinationCurrency { get; set; }
        public string SenderFullName { get; set; }
        public string SenderCountry { get; set; }
        public string SenderAddress { get; set; }
        public string SenderContactNumber { get; set; }
        public string ReceiverFullName { get; set; }
        public string ReceiverProvice { get; set; }
        public string ReceiverDistrict { get; set; }
        public string ReceiverLocalBody { get; set; }
        public string ReceiverAddress { get; set; }
        public string ReceiverContactNumber { get; set; }
        public string ReceiverDocumentType { get; set; }
        public string ReceiverDocumentNumber { get; set; }
        public string AgentName { get; set; }
        public string AgentContactNumber { get; set; }
        public string AgentAddress { get; set; }
        public string AgentCity { get; set; }
        public string AgentDistrict { get; set; }
        public string AgentCountry { get; set; }
        public string AgentOrganizationName { get; set; }
        public string ControlNumber { get; set; }
        public string Status { get; set; }
        public string ModeOfPayment { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string TransactionDateNepali { get; set; }
        public string AgentCommissionNPR { get; set; }
    }
}
