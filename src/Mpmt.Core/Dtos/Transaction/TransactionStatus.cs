using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Core.Dtos.Transaction
{
    public class TransactionStatus
    {
        public string TransactionId { get; set; }
        public string AgentTransactionId { get; set; }
        public string OwnStatus { get; set; }
        public string GatewayStatus { get; set; }
        public string TransactionState { get; set; }
        public string ComplianceStatus { get; set; }
        public string GatewayTxnId { get; set; }
    }
}   
