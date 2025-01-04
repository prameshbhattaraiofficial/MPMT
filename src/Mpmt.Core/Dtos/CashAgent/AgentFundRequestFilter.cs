using Microsoft.AspNetCore.Http;
using Mpmt.Core.Dtos.Paging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Core.Dtos.CashAgent
{
    public class AgentFundRequestFilter : PagedRequest
    {
        public string AgentName { get; set; }   
        public string AgentCode { get; set; }
        public string StartDate { get; set; }
        public string StartDateBS { get; set; }
        public string EndDate { get; set; }
        public string EndDateBS { get; set; }
        public string Status { get; set; }  
        public int Export { get; set; }
        public string RequestedNepaliDate { get; set; }
        public string RequestedDate { get; set; }
        public string Email { get; set; }
    }

    public class AgentFundApproveRejectModel
    {
        public string AgentCode { get; set; }
        [Required(ErrorMessage ="Remarks required!")]
        public string Remarks { get; set; }
        public string RequestFundId { get; set; }
        public string OperationMode { get; set; }
        public IFormFile VoucherImagePath { get; set; }
        public string VoucherImage { get; set; }
        public bool IsCommissionRequested { get; set; }
        public bool IsTxnCashRequested { get; set; }
        [Required(ErrorMessage = "Total amount is requred!")]
        public decimal TotalAmount { get; set; }
        public string TransactionId { get; set; }
        public string AgentName { get; set; }
        public string Email { get; set; }

    }

    public class ApproveRejectReviewModel
    {
        public string AgentCode { get; set; }
        public string AgentEmail { get; set; }
        public string AgentName { get; set; }
        public string SuperAgentCode { get; set; }
        public string SuperAgentEmail { get; set; }
        public string SuperAgentName { get; set; }
        public string MpmtAdminEmail { get; set; }
        public bool IsPrefunding { get; set; }
    }
}
