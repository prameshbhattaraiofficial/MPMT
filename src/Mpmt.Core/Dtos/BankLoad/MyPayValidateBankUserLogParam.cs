using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Core.Dtos.BankLoad
{
    public class MyPayValidateBankUserLogParam
    {
        public string RemitTransactionId { get; set; }
        public string AgentCode { get; set; }
        public string AccountStatus { get; set; }
        public string AccountNumber { get; set; }
        public string Amount { get; set; }
        public bool IsAccountValidated { get; set; }
        public string Message { get; set; }
        public string ResponseMessage { get; set; }
        public string ResponseCode { get; set; }
        public bool Status { get; set; }
        public string LoggedInUser { get; set; }
        public string UserType { get; set; }
    }
}
