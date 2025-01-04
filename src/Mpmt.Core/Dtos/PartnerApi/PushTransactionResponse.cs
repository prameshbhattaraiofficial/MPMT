using Mpmt.Core.Domain;

namespace Mpmt.Core.Dtos.PartnerApi
{
    public class PushTransactionResponse : ApiResponse
    {
        private string _transactionId;
        private string _mtcnNumber;
        private string _paymentType;
        private string _sourceCurrency;
        private string _destinationCurrency;
        private string _sendingAmount;
        private string _netSendingAmount;
        private string _destCreditAmount;
        private string _conversionRate;
        private string _serviceCharge;
        private string _partnerServiceCharge;
        private string _commission;
        private string _transactionDate;
        private string _senderName;
        private string _senderEmail;
        private string _senderContactNumber;
        private string _senderCountryCode;
        private string _recipientCountryCode;
        private string _recipientName;
        private string _recipientContactNumber;
        private string _payableAmount;

        public string TransactionId { get => _transactionId ?? string.Empty; set => _transactionId = value; }
        public string MtcnNumber { get => _mtcnNumber ?? string.Empty; set => _mtcnNumber = value; }
        public string PaymentType { get => _paymentType ?? string.Empty; set => _paymentType = value; }
        public string SourceCurrency { get => _sourceCurrency ?? string.Empty; set => _sourceCurrency = value; }
        public string DestinationCurrency { get => _destinationCurrency ?? string.Empty; set => _destinationCurrency = value; }
        public string SendingAmount { get => _sendingAmount ?? string.Empty; set => _sendingAmount = value; }
        public string NetSendingAmount { get => _netSendingAmount ?? string.Empty; set => _netSendingAmount = value; }
        public string DestCreditAmount { get => _destCreditAmount ?? string.Empty; set => _destCreditAmount = value; }
        public string ConversionRate { get => _conversionRate ?? string.Empty; set => _conversionRate = value; }
        public string ServiceCharge { get => _serviceCharge ?? string.Empty; set => _serviceCharge = value; }
        public string PartnerServiceCharge { get => _partnerServiceCharge ?? string.Empty; set => _partnerServiceCharge = value; }
        public string Commission { get => _commission ?? string.Empty; set => _commission = value; }
        public string TransactionDate { get => _transactionDate ?? string.Empty; set => _transactionDate = value; }
        public string SenderName { get => _senderName ?? string.Empty; set => _senderName = value; }
        public string SenderEmail { get => _senderEmail ?? string.Empty; set => _senderEmail = value; }
        public string SenderContactNumber { get => _senderContactNumber ?? string.Empty; set => _senderContactNumber = value; }
        public string SenderCountryCode { get => _senderCountryCode ?? string.Empty; set => _senderCountryCode = value; }
        public string RecipientCountryCode { get => _recipientCountryCode ?? string.Empty; set => _recipientCountryCode = value; }
        public string RecipientName { get => _recipientName ?? string.Empty; set => _recipientName = value; }
        public string RecipientContactNumber { get => _recipientContactNumber ?? string.Empty; set => _recipientContactNumber = value; }
        public string PayableAmount { get => _payableAmount ?? string.Empty; set => _payableAmount = value; }
    }
}
