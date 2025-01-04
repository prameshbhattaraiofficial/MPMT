using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Core.Dtos.AgentReport
{
    public class AgentSettlementReport
    {
        public DateTime? TransactionDate { get; set; }
        public string SenderTransactionDateString { get; set; }
        public DateTime? PayoutDate{ get; set; }
        public string PayoutNepaliDate { get; set; }
        public string SenderName { get; set; }
        public string SenderCountry { get; set; }
        public string SenderContact { get; set; }
        public string SenderAddress { get; set; }
        public string SenderEmail { get; set; }
        public string ReceiverName { get; set; }

        public string ReceiverContact { get; set; }
        public string ReceiverCountry { get; set; }
        public string ReceiverAddress { get; set; }
        public string ReceiverEmail { get; set; }
        public string TransactionId { get; set; }
        public string AgentTrackerId { get; set; }
        public string SourceCurrency { get; set; }
        public string DestinationCurrency { get; set; }

        public decimal SendingAmount { get; set; }
        public decimal ConversionRate { get; set; }
        public decimal NetSendingAmount { get; set; }
        public decimal NetReceivingAmountNPR { get; set; }
        public string AgentDistrict { get; set; }
        public string AgentName { get; set; }
        public string AgentCode { get; set; }
        public decimal AgentCommissionNPR { get; set; }
        public string SuperAgentName { get; set; }
        public string SuperAgentCode { get; set; }
        public decimal SuperAgentCommissionNPR { get; set; }
        public string TransactionType { get; set; }
        public string PayoutType { get; set; }
        public string Status { get; set; }
    }
}
