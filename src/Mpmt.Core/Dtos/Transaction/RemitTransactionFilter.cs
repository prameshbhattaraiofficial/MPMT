using Mpmt.Core.Dtos.Paging;

namespace Mpmt.Core.Dtos.Transaction
{
    public class RemitTransactionFilter : PagedRequest
    {
        public string PartnerCode { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string SourceCurrency { get; set; }
        public string DestinationCurrency { get; set; }
        public string TransactionId { get; set; }
        public string SignType { get; set; }
        public string TransactionType { get; set; }
        public string UserType { get; set; }
        public string LoggedInUser { get; set; }
    }
}
