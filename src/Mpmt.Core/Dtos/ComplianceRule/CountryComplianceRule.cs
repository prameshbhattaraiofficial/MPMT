using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Core.Dtos.ComplianceRule
{
    public class CountryComplianceRule
    {
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
    }
    public class CountryComplianceRuleTest
    {
        public string[] CountryCode { get; set; }
    
    }
}
