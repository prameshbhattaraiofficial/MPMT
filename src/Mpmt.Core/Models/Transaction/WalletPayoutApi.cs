using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Core.Models.Transaction
{
    public class WalletPayoutApi
    {
        public string MTCN { get; set; }
        public string SenderName { get; set; }
        public string Amount { get; set; }
        public string WalletHolderName { get; set; }
        public string Country { get; set; }
        public string DeviceId { get; set; }
        //public string AgentType { get; set; }
        public string ApiUserName { get; set; }
        public string Signature { get; set; }
    }
}
