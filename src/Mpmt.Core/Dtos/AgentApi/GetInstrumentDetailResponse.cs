using Mpmt.Core.Domain;

namespace Mpmt.Core.Dtos.AgentApi
{
    public class GetInstrumentDetailResponse : ApiResponse
    {
        private string _paymentStatusCode;
        private string _paymentStatusRemarks;
        private string _transactionId;
        private string _paymentType;
        private string _paymentStatus;
        private string _sourceCurrency;
        private string _destinationCurrency;
        private string _paymentAmountNPR;
        private string _sendingDate;
        private string _sendingNepaliDate;
        private string _senderFullName;
        private string _senderContactNumber;
        private string _senderEmail;
        private string _senderCountry;
        private string _senderAddress;
        private string _senderRelationWithReceiver;
        private string _receiverFullName;
        private string _receiverContactNumber;
        private string _receiverEmail;
        private string _receiverCountry;
        private string _receiverProvince;
        private string _receiverDistrict;
        private string _receiverLocalBody;
        private string _receiverAddress;
        private string _receiverRelationWithSender;
        private IEnumerable<DocumentTypeApi> _documentTypeList;
        
        public string PaymentStatusCode { get => _paymentStatusCode ?? string.Empty; set => _paymentStatusCode = value; }
        public string PaymentStatusRemarks { get => _paymentStatusRemarks ?? string.Empty; set => _paymentStatusRemarks = value; }
        public string TransactionId { get => _transactionId ?? string.Empty; set => _transactionId = value; }
        public string PaymentType { get => _paymentType ?? string.Empty; set => _paymentType = value; }
        public string PaymentStatus { get => _paymentStatus ?? string.Empty; set => _paymentStatus = value; }
        public string SourceCurrency { get => _sourceCurrency ?? string.Empty; set => _sourceCurrency = value; }
        public string DestinationCurrency { get => _destinationCurrency ?? string.Empty; set => _destinationCurrency = value; }
        public string PaymentAmountNPR { get => _paymentAmountNPR ?? string.Empty; set => _paymentAmountNPR = value; }
        public string SendingDate { get => _sendingDate ?? string.Empty; set => _sendingDate = value; }
        public string SendingNepaliDate { get => _sendingNepaliDate ?? string.Empty; set => _sendingNepaliDate = value; }
        public string SenderFullName { get => _senderFullName ?? string.Empty; set => _senderFullName = value; }
        public string SenderContactNumber { get => _senderContactNumber ?? string.Empty; set => _senderContactNumber = value; }
        public string SenderEmail { get => _senderEmail ?? string.Empty; set => _senderEmail = value; }
        public string SenderCountry { get => _senderCountry ?? string.Empty; set => _senderCountry = value; }
        public string SenderAddress { get => _senderAddress ?? string.Empty; set => _senderAddress = value; }
        public string SenderRelationWithReceiver { get => _senderRelationWithReceiver ?? string.Empty; set => _senderRelationWithReceiver = value; }
        public string ReceiverFullName { get => _receiverFullName ?? string.Empty; set => _receiverFullName = value; }
        public string ReceiverContactNumber { get => _receiverContactNumber ?? string.Empty; set => _receiverContactNumber = value; }
        public string ReceiverEmail { get => _receiverEmail ?? string.Empty; set => _receiverEmail = value; }
        public string ReceiverCountry { get => _receiverCountry ?? string.Empty; set => _receiverCountry = value; }
        public string ReceiverProvince { get => _receiverProvince ?? string.Empty; set => _receiverProvince = value; }
        public string ReceiverDistrict { get => _receiverDistrict ?? string.Empty; set => _receiverDistrict = value; }
        public string ReceiverLocalBody { get => _receiverLocalBody ?? string.Empty; set => _receiverLocalBody = value; }
        public string ReceiverAddress { get => _receiverAddress ?? string.Empty; set => _receiverAddress = value; }
        public string ReceiverRelationWithSender { get => _receiverRelationWithSender ?? string.Empty; set => _receiverRelationWithSender = value; }

        public IEnumerable<DocumentTypeApi> DocumentTypeList { get => _documentTypeList ?? Enumerable.Empty<DocumentTypeApi>(); set => _documentTypeList = value; }
    }
}
