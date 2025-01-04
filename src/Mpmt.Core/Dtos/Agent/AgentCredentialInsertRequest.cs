using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Core.Dtos.Agent
{
    public class AgentCredentialInsertRequest
    {
        public string AgentCode { get; set; }
        
        public string ApiUserName { get; set; }
       
        public string[] IPAddress { get; set; }

        public string IsActive { get; set; }
    }

    public class AgentCredentialUpdateRequest : AgentCredentialInsertRequest
    {
        public string CredentialId { get; set; }
    }
}
