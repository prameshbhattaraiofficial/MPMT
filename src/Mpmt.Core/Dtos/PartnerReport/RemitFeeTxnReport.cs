using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Core.Dtos.PartnerReport
{
    public class RemitFeeTxnReport
    {
        public int SN { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime SenderCreatedDate { get; set; }
        public DateTime ReceiverCreatedDate { get; set; }
        public string SenderDtl { get; set; }
        public string ReceiverDtl { get; set; }
        public string TransactionId { get; set; }
        public string ParentTransactionId { get; set; } 
        public decimal ServiceCharge { get; set; }
        public decimal ServiceChargeUSD { get; set; }
        public string Sign { get; set; }
        public string PartnerId { get; set; }
        public string PartnerFullName { get; set; }
        public string SourceCurrency { get; set; }
        public string DestinationCurrency { get; set; }
        public string PaymentType { get; set; }
        public decimal Amount { get; set; }
        public decimal SendAmount { get; set; }
        public decimal ReceiveAmount { get; set; }
        public string Status { get; set; }
        public string SenderUserStatus { get; set; }
        public string ReceiverUserStatus { get; set; }
    }
}
