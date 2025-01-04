using Mpmt.Core.Domain;

namespace Mpmt.Core.Dtos.PartnerApi
{
    public class TxnChargeDetails : ApiResponse
    {
        private string _sourceCurrency;
        private string _destinationCurrency;
        private string _sendingAmount;
        private string _conversionRate;
        private string _netSendingAmount;
        private string _receivingAmountNPR;
        private string _serviceCharge;
        private string _commission;
        private string _partnerServiceCharge;

        public string SourceCurrency { get => _sourceCurrency ?? string.Empty; set => _sourceCurrency = value; }
        public string DestinationCurrency { get => _destinationCurrency ?? string.Empty; set => _destinationCurrency = value; }
        public string SendingAmount { get => _sendingAmount ?? string.Empty; set => _sendingAmount = value; }
        public string ConversionRate { get => _conversionRate ?? string.Empty; set => _conversionRate = value; }
        public string NetSendingAmount { get => _netSendingAmount ?? string.Empty; set => _netSendingAmount = value; }
        public string ReceivingAmountNPR { get => _receivingAmountNPR ?? string.Empty; set => _receivingAmountNPR = value; }
        public string ServiceCharge { get => _serviceCharge ?? string.Empty; set => _serviceCharge = value; }
        public string Commission { get => _commission ?? string.Empty; set => _commission = value; }
        public string PartnerServiceCharge { get => _partnerServiceCharge ?? string.Empty; set => _partnerServiceCharge = value; }
    }
}
