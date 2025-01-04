using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Core.Dtos.BankLoad
{
    public class MyPayBankPayoutDto
    {
        public string Amount { get; set; }
       // public string ContactNumber { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public string BankId { get; set; }
        public string AccountHolderName { get; set; }

        public string Reference { get; set; }
        public string Remarks { get; set; }
    }
}
