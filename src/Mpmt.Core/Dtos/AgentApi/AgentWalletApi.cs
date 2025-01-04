using Mpmt.Core.Domain;
using Mpmt.Core.Dtos.PartnerApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Core.Dtos.AgentApi
{
    public class AgentWalletApi:ApiResponse
    {
        private IEnumerable<CountryItem> _countryList;
        private IEnumerable<DocumentTypeItem> _senderDocumentTypeList;
        public IEnumerable<CountryItem> CountryList
        {
            get => _countryList ?? new List<CountryItem>();
            set => _countryList = value;
        }
        public IEnumerable<DocumentTypeItem> SenderDocumentTypeList
        {
            get => _senderDocumentTypeList ?? new List<DocumentTypeItem>();
            set => _senderDocumentTypeList = value;
        }
        public string TransactionStatusCode { get; set; }
       public string TransactionRemarks { get; set; }

    }
}
