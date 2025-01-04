using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Core.Dtos.CashAgent
{
    public class AgentLoginActivity
    {
        
        public string AgentCode { get; set; }
        public int FailedLoginAttempt { get; set; }
        public DateTime? TemporaryLockedTillUtcDate { get; set; }
        public string IpAddress { get; set; }
        public string DeviceId { get; set; }
        public bool IsActive { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime? LastLoginDateUtc { get; set; }
        public DateTime? LastActivityDateUtc { get; set; }
    }
}
