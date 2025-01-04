using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Core.Dtos.AgentFundTransfer
{
    public class GetAgentFundRequestList
    {
        //public string AgentEmail { get; set; }
        public string AgentCode { get; set; }
        public string AgentEmail { get; set; }
        public string SuperAgentCode { get; set; }
        public string SuperAgentEmail { get; set;}
        public string MpmtAdminEmail { get; set; }
        public string AgentName { get; set; }
        public string SuperAgentName { get; set; }
        public decimal? TotalAmount { get; set; }

    }
}
