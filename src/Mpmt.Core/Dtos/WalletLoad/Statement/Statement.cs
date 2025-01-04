using Mpmt.Core.Dtos.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Core.Dtos.WalletLoad.Statement
{
    public class Statement : PagedRequest
    {
        public string StartDateBS { get; set; }
        public string EndDateBS { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Partnercode { get; set; }
        public string DateFlag { get; set; }
        public string walletCurrencyById { get; set; }
        public string partnerId { get; set; }
        public int Export { get; set; }

    }
}
