using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Core.Dtos.AgentFundTransfer
{
    public class ApproveRejectFundTransferByAdmin
    {
        public string AgentCode { get; set; }
        public string Remarks { get; set; }
        public string RequestFundId { get; set; }
        public string OperationMode { get; set; }
        public IFormFile VoucherImagePath { get; set; }
        public string VoucherImage { get; set; }
        public bool IsCommissionRequested { get; set; }
        public bool IsTxnCashRequested { get; set; }
        public string TotalAmount { get; set; }
        public string TransactionId { get; set; }
    }
}
