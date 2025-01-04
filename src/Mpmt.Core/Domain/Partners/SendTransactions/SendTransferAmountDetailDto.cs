using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Core.Domain.Partners.SendTransactions
{
    public class SendTransferAmountDetailDto
    {
        public string SourceCurrency { get; set; }
        public string DestinationCurrency { get; set; }
        public decimal SourceAmount { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal? DestinationAmount { get; set; }
    }
}
