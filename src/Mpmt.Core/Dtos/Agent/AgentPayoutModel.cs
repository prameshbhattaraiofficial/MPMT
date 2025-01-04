using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Core.Dtos.Agent
{
    public class AgentPayoutModel
    {
        public string AgentCode { get; set; }
        public string Amount { get; set; }
        public string SenderName { get; set; }
        public string WalletHolderName { get; set; }
        public string MTCN { get; set; }
        public string Country { get; set; }
        public string DeviceId { get; set; }
        public string AgentType { get; set; }
        public string IPAddress { get; set; }
    }
}
