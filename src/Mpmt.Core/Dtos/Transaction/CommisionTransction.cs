using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Core.Dtos.Transaction
{
    public class CommisionTransction
    {
        public string SN { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionDateString { get; set; }
        public string SenderTransactionDateString { get; set; }
        public string ReceiverTransactionDateString { get; set; }
        public string TransactionId { get; set; }
        public string ParentTransactionId { get; set; }
        public string PartnerId { get; set; }
        public string PartnerFullName { get; set; }
        public string SourceCurrency { get; set; }
        public string DestinationCurrency { get; set; }
        public string PaymentType { get; set; }
        public decimal CommisionAmount { get; set; }
        public decimal PreviousWalletBalance { get; set; }
        public decimal CurrentWalletBalance { get; set; }
        public decimal SendAmount { get; set; }
        public decimal ReceiveAmount { get; set; }
        public string Status { get; set; }
    }
}
	