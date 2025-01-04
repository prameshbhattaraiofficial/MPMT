using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Core.ViewModel.CashAgent
{
    public class RemitCashAgentsDetails
    {
            public string AgentCode { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string ContactNumber { get; set; }
            public string UserName { get; set; }
            public string OrganizationName { get; set; }
            public string DistrictName { get; set; }
            public string FullAddress { get; set; }
            public string RegistrationNumber { get; set; }
            public string CompanyLogoImgPath { get; set; }
            public string DocumentType { get; set; }
            public string DocumentNumber { get; set; }
            public string IdFrontImgPath { get; set; }
            public string IdBackImgPath { get; set; }
            public DateTime ExpiryDate { get; set; }
            public string IpAddress { get; set; }
            public bool IsActive { get; set; }
            public bool IsPrefunding { get; set; }
           public List<string> DocumentImgPath { get; set; }


    }
}
