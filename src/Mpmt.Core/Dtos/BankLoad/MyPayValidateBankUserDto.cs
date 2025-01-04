using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Core.Dtos.BankLoad
{
    public class MyPayValidateBankUserDto
    {
        public string BankCode { get; set; }  
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public decimal? Amount { get; set; }
        public string Reference { get;set; }
    }

    public class MyPayCheckComplianceForBankLoadDto
    {
        public string transaction_ID { get; set; }
       
    }
}
