using Mpmt.Core.Dtos.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Core.Dtos.PartnerStatement
{
    public class PartnerWalletStatement 
    {
        public string SN { get; set; }
        public string PartnerCode { get; set; }
        public string PartnerName { get; set; }
        public string PartnerCountry { get; set; }
        public string WalletCurrency { get; set; }
        public string ReferenceId { get; set; }
        public string Type { get; set; }
        public string Particular { get; set; }
        public string Amount { get; set; }
        public string ExchangeRate { get; set; }
        public string Sign { get; set; }
        public string PreviousBalance { get; set; }
        public string CurrentBalance { get; set; }
        public string Remarks { get; set; }
        public string UserType { get; set; }
        public string TxnBy { get; set; }
        public string NepalStandardDate { get; set; }
        public string NepaliDate { get; set; }
        
    }


   
}
