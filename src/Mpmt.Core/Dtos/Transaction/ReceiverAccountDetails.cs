using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Mpmt.Core.Dtos.Transaction
{
    public class ReceiverAccountDetails
    {
        public  string TransactionId { get; set; }
        public string AccountHolderName { get; set; }
        public string AccountNumber { get; set; }
        public string WalletNumber { get; set; }
        public string BankName { get; set; }
        public string Branch { get; set; }
        public string BankCode { get; set; }
        public string PaymentType { get; set; }
        public string WalletHolderName { get; set; }
        public string TxnUpdateRemarks { get; set; }
        public string PaymentStatusCode { get; set; }
        public string GatewayTxnId { get; set; }    
        public string logedinUser { get; set; }
        public string UserType { get; set; }
    }
        
    public class ReceiverCashoutDetails
    {
        public string TransactionId { get; set; }
        public string ReceiverName { get; set; }
        public string ContactNumber { get; set; }
        public string DistrictCode { get; set; }
        public string District { get; set; }
        public string FullAddress { get; set; }
        public string logedinUser { get; set; }
        public string UserType { get; set; }
        public string Remarks { get; set; }
    }
}
