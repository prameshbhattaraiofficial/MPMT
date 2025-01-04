using Mpmt.Core.Dtos.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Core.Dtos.AgentReport
{
    public class AgentSettlementFilter : PagedRequest
    {
        public string AgentCode { get; set; }
        public string AgentName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string SourceCurrency { get; set; }
        public string DestinationCurrency { get; set; }
        public string TransactionId { get; set; }
        public string TransactionType { get; set; }
        public string AgentDistrict { get; set; }
        public string UserType { get; set; }
        public string LoggedInUser { get; set; }
        public int Export { get; set; }
    }
}
