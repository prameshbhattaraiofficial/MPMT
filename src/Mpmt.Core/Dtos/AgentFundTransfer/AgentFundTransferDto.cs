using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Core.Dtos.AgentFundTransfer
{
    public class AgentFundTransferDto
    {
        public string AgentCode { get; set; }
        public decimal CommissionAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal ReceivableAmount { get; set; }
        public string FundType { get; set; }
        public string Remarks { get; set; }
        public string UserType { get; set; }
        public string LoggedInUser { get; set; }
        public string OperationMode { get; set; }
        public decimal? AmountTotal { get; set; }
        public string amtTotal { get; set; }
        public string totalAmount { get; set; }

        public string isCommission { get; set; }
        public string isReceivable { get; set; }
        public int RequestStatus { get; set; }
        public int CommRequestStatus { get; set; }  

        public string PrefundBalance { get; set; }
        public bool IsAgentPrefunding { get; set; }

    }
    public class FundRequestModel
    {
        public string TotalAmount { get; set; }
    }

}
