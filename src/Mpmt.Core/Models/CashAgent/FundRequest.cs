using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Core.Models.CashAgent
{
    public class FundRequest
    {
        public string FundRequestId { get; set; }
        public string SN { get; set; }
        public string AgentCode { get; set; }
        public string AgentName { get; set; }
        public string OrganizationName { get; set; }    
        public string AgentType { get; set; }
        public string District { get; set; }
        public bool IsTxnCashRequested { get; set; }
        public decimal TxnCashAmount { get; set; }
        public bool IsCommissionRequested { get; set; }
        public decimal CommissionAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string Remarks { get; set; }
        public string StatusCode { get; set; }
        public string StatusName { get; set; }
        public string RequesteDate { get; set; }
        public string RequestedNepaliDate { get; set; }
        public string EndDateAD { get; set; }
        public string EndDateBS { get; set; }
        public bool IsPrefunding { get; set; }    

    }
}
