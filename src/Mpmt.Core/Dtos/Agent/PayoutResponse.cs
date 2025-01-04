using Mpmt.Core.Domain;

namespace Mpmt.Core.Dtos.Agent
{
    public class PayoutResponse : ApiResponse
    {
        public string sourceCurrency { get; set; }
        public string destinationCurrency { get; set; }
        public string sendingAmount { get; set; }
        public string senderName { get; set; }
        public string RemitTransactionId { get; set; }
        public string PayoutType { get; set; }
  
    }
}
