using Mpmt.Core.Dtos.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Core.Dtos.Transaction
{
    public class CommissionTransactionFilter: PagedRequest
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
        public int Export { get; set; }
    }
}
