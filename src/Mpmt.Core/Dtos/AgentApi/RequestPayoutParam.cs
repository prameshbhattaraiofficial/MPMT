namespace Mpmt.Core.Dtos.AgentApi
{
    public class RequestPayoutParam
    {
        public string RemitTransactionId { get; set; }
        public string ProcessId { get; set; }
        public string AgentTransactionId { get; set; }
        public string PayoutTransactionType { get; set; }
        public string PaymentType { get; set; }
        public string AgentCode { get; set; }
        //public string FullName { get; set; }
        public string ContactNumber { get; set; }
        //public string Country { get; set; }
        //public string Province { get; set; }
        //public string District { get; set; }
        //public string LocalBody { get; set; }
        //public string Address { get; set; }
        public string DocumentTypeCode { get; set; }
        //public string DocumentType { get; set; }
        public string DocumentNumber { get; set; }
        public string DocumentIssueDate { get; set; }
        public string DocumentExpiryDate { get; set; }
        public string DocumentIssueDateNepali { get; set; }
        public string DocumentExpiryDateNepali { get; set; }
        public string DocumentFrontImagePath { get; set; }
        public string DocumentBackImagePath { get; set; }
        public string Username { get; set; }
        public string UserType { get; set; }
        public string LoggedInUser { get; set; }
        public string IpAddress { get; set; }
        public string DeviceId { get; set; }
    }
}
