using Mpmt.Core.Domain;

namespace Mpmt.Core.Dtos.PartnerApi
{
    public class InstrumentLists : ApiResponse
    {
        private IEnumerable<PaymentTypeItem> _paymentTypeList;
        private IEnumerable<SourceCurrencyItem> _sourceCurrencyList;
        private IEnumerable<DestinationCurrencyItem> _destinationCurrencyList;
        private IEnumerable<CountryItem> _countryList;
        //private IEnumerable<ProvinceItem> _recipientProvinceList;
        //private IEnumerable<DistrictItem> _recipientDistrictList;
        //private IEnumerable<LocalLevelItem> _recipientLocalLevelList;
        private IEnumerable<BankItem> _recipientBankList;
        private IEnumerable<WalletTypeItem> _recipientWalletTypeList;
        //private IEnumerable<RelationshipItem> _relationshipList;
        //private IEnumerable<PurposeItem> _purposeList;
        private IEnumerable<RecipientTypeItem> _recipientTypeList;
        private IEnumerable<DocumentTypeItem> _senderDocumentTypeList;
        //private IEnumerable<OccupationItem> _occupationList;
        //private IEnumerable<SourceOfIncomeItem> _sourceOfIncomeList;

        public IEnumerable<PaymentTypeItem> PaymentTypeList
        {
            get => _paymentTypeList ?? new List<PaymentTypeItem>();
            set => _paymentTypeList = value;
        }

        public IEnumerable<SourceCurrencyItem> SourceCurrencyList
        {
            get => _sourceCurrencyList ?? new List<SourceCurrencyItem>();
            set => _sourceCurrencyList = value;
        }

        public IEnumerable<DestinationCurrencyItem> DestinationCurrencyList
        {
            get => _destinationCurrencyList ?? new List<DestinationCurrencyItem>();
            set => _destinationCurrencyList = value;
        }

        public IEnumerable<CountryItem> CountryList
        {
            get => _countryList ?? new List<CountryItem>();
            set => _countryList = value;
        }

        //public IEnumerable<ProvinceItem> RecipientProvinceList
        //{
        //    get => _recipientProvinceList ?? new List<ProvinceItem>();
        //    set => _recipientProvinceList = value;
        //}

        //public IEnumerable<DistrictItem> RecipientDistrictList
        //{
        //    get => _recipientDistrictList ?? new List<DistrictItem>();
        //    set => _recipientDistrictList = value;
        //}

        //public IEnumerable<LocalLevelItem> RecipientLocalLevelList
        //{
        //    get => _recipientLocalLevelList ?? new List<LocalLevelItem>();
        //    set => _recipientLocalLevelList = value;
        //}

        public IEnumerable<BankItem> RecipientBankList
        {
            get => _recipientBankList ?? new List<BankItem>();
            set => _recipientBankList = value;
        }

        public IEnumerable<WalletTypeItem> RecipientWalletTypeList
        {
            get => _recipientWalletTypeList ?? new List<WalletTypeItem>();
            set => _recipientWalletTypeList = value;
        }

        //public IEnumerable<RelationshipItem> RelationshipList
        //{
        //    get => _relationshipList ?? new List<RelationshipItem>();
        //    set => _relationshipList = value;
        //}

        //public IEnumerable<PurposeItem> PurposeList
        //{
        //    get => _purposeList ?? new List<PurposeItem>();
        //    set => _purposeList = value;
        //}

        public IEnumerable<RecipientTypeItem> RecipientTypeList
        {
            get => _recipientTypeList ?? new List<RecipientTypeItem>();
            set => _recipientTypeList = value;
        }

        public IEnumerable<DocumentTypeItem> SenderDocumentTypeList
        {
            get => _senderDocumentTypeList ?? new List<DocumentTypeItem>();
            set => _senderDocumentTypeList = value;
        }

        //public IEnumerable<OccupationItem> OccupationList
        //{
        //    get => _occupationList ?? new List<OccupationItem>();
        //    set => _occupationList = value;
        //}

        //public IEnumerable<SourceOfIncomeItem> SourceOfIncomeList
        //{
        //    get => _sourceOfIncomeList ?? new List<SourceOfIncomeItem>();
        //    set => _sourceOfIncomeList = value;
        //}
    }
}
