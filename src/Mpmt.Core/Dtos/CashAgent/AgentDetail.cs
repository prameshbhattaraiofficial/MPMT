using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Core.Dtos.CashAgent
{
    public class AgentDetail
    {
        public string SN { get; set; }
        public string Agentcode { get; set; }
        public string DistrictCode { get; set; }
        public string District { get; set; }
        public string UserName { get; set; }
        //public string Fullname { get; set; }
        public string EmployeeId { get; set; }  
        public string LicenseDocImgPath { get; set; }   
        public string FullName { get; set; }
        public string Email { get; set; }
        public string EmailConfirmed { get; set; }
        public string FullAddress { get; set; }
        public string ContactNumber { get; set; }
        public string UserType { get; set; }
        public string ContactNumberConfirmed { get; set; }
        public string RegisteredDate { get; set; }
        public string CreatedDate { get; set; }
        public string FailedLoginAttempt { get; set; }
        public bool IsActive { get; set; }
        public bool IsPrefunding { get; set; }  
        public string CredentialId { get; set; }

        public string SuperAgentCode { get; set; }
        public string AgentName { get; set; }
    }

    public class AgentUserModel
    {
        public string AgentCode { get; set; }
        public string FullName { get; set; }    
        public string CreatedDate { get; set; }    
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string UserName { get; set; }
        public string UserType { get; set; }
        public string District { get; set; }
        public string FullAddress { get; set; }
        public string OrganizationName { get; set; }
        public bool IsActive { get; set; }
    }
}
