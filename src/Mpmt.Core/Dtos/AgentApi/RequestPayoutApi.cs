using Microsoft.AspNetCore.Http;

namespace Mpmt.Core.Dtos.AgentApi
{
    public class RequestPayoutApi
    {
        public string ApiUserName { get; set; }
        public string RemitTransactionId { get; set; }
        public string ProcessId { get; set; }
        public string AgentTransactionId { get; set; }
        public string PaymentType { get; set; }
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
        public string DocumentIssueDateAD { get; set; }
        public string DocumentExpiryDateAD { get; set; }
        public string DocumentIssueDateBS { get; set; }
        public string DocumentExpiryDateBS { get; set; }
        public IFormFile DocumentFrontImage { get; set; }
        public IFormFile DocumentBackImage { get; set; }
        public string Signature { get; set; }
    }


    public class RequestPayoutForAgentWalletApi
    {
        public string ApiUserName { get; set; }
        public string RemitTransactionId { get; set; }
        public string ProcessId { get; set; }
        public string AgentTransactionId { get; set; }
        public string PaymentType { get; set; }
        public string DocumentTypeCode { get; set; }
        public string DocumentNumber { get; set; }
        public string DocumentIssueDateAD { get; set; }
        public string DocumentExpiryDateAD { get; set; }
        public string DocumentIssueDateBS { get; set; }
        public string DocumentExpiryDateBS { get; set; }
        public IFormFile DocumentFrontImage { get; set; }
        public IFormFile DocumentBackImage { get; set; }
        public string Signature { get; set; }
    }


}
