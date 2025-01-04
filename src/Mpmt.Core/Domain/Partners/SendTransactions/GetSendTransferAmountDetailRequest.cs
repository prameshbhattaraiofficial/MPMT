using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Core.Domain.Partners.SendTransactions
{
    public class GetSendTransferAmountDetailRequest
    {
        public string PartnerCode { get; set; }
        public string SourceCurrency { get; set; }
        public string DestinationCurrency { get; set; }
        public string Remarks { get; set; }
        public decimal SourceAmount { get; set; }
        public string AccountType { get; set; }
        public decimal DestinationAmount { get; set; }
        public string LoggedInUser { get; set; }
        public string UserType { get; set; }
    }   
}
