using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Core.Dtos.CashAgent
{
    public class AgentCredential
    {
       
        public string CredentialId { get; set; }
        
        public string AgentCode { get; set; }
      
        public string ApiUserName { get; set; }
       
        public string ApiPassword { get; set; }
        
        public string ApiKey { get; set; }
       
        public string SystemPrivateKey { get; set; }
       
        public string SystemPublicKey { get; set; }
        
        public string UserPrivateKey { get; set; }
        
        public string UserPublicKey { get; set; }
      
        public string IPAddress { get; set; }

        public string IsActive { get; set; }

        public string CreatedById { get; set; }
       
        public string CreatedByName { get; set; }
     
        public string CreatedDate { get; set; }
        
        public string UpdatedById { get; set; }
       
        public string UpdatedByName { get; set; }
       
        public string UpdatedDate { get; set; }
        public string OperationMode { get; set; }
    }
}
