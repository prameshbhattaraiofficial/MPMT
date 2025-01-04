using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Core.Dtos.Transaction
{
    public class PartnerSendTransactionReceipt
    {
        public string PartnerCode {  get; set; }
        public string PartnerName { get; set;}
        public string PartnerCountryCode { get; set;}
        public string SourceCurrency { get; set;}
        public string DestinationCurrency { get; set;}
        public string SendingAmount { get; set;}
        public string ServiceCharge { get; set;}
        public string ConversionRate { get; set;}
        public string NetSendingAmount { get; set;}
        public string NetReceivingAmount { get; set;}
        public string CreditSendingAmount { get; set;}
        public string SenderRegistered { get; set;}
        public string MemberId { get; set;}

    }
}
