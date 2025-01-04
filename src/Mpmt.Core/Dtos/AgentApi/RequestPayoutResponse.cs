using Mpmt.Core.Domain;

namespace Mpmt.Core.Dtos.AgentApi
{
    public class RequestPayoutResponse : ApiResponse
    {
        private string _transactionId;
        private string _amountNPR;
        private string _sourceCurrency;
        private string _destinationCurrency;
        private string _senderFullName;
        private string _senderCountry;
        private string _senderAddress;
        private string _senderContactNumber;
        private string _receiverFullName;
        private string _receiverProvice;
        private string _receiverDistrict;
        private string _receiverLocalBody;
        private string _receiverAddress;
        private string _receiverContactNumber;
        private string _receiverDocumentType;
        private string _receiverDocumentNumber;
        private string _agentName;
        private string _agentContactNumber;
        private string _agentAddress;
        private string _agentCity;
        private string _agentDistrict;
        private string _agentCountry;
        private string _agentOrganizationName;
        //private string _controlNumber;
        private string _status;
        private string _agentCommissionNPR;
        private string _modeOfPayment;
        private string _transactionDate;
        private string _transactionDateNepali;

        public string TransactionId { get => _transactionId ?? string.Empty; set => _transactionId = value; }
        public string AmountNPR { get => _amountNPR ?? string.Empty; set => _amountNPR = value; }
        public string SourceCurrency { get => _sourceCurrency ?? string.Empty; set => _sourceCurrency = value; }
        public string DestinationCurrency { get => _destinationCurrency ?? string.Empty; set => _destinationCurrency = value; }
        public string SenderFullName { get => _senderFullName ?? string.Empty; set => _senderFullName = value; }
        public string SenderCountry { get => _senderCountry ?? string.Empty; set => _senderCountry = value; }
        public string SenderAddress { get => _senderAddress ?? string.Empty; set => _senderAddress = value; }
        public string SenderContactNumber { get => _senderContactNumber ?? string.Empty; set => _senderContactNumber = value; }
        public string ReceiverFullName { get => _receiverFullName ?? string.Empty; set => _receiverFullName = value; }
        public string ReceiverProvice { get => _receiverProvice ?? string.Empty; set => _receiverProvice = value; }
        public string ReceiverDistrict { get => _receiverDistrict ?? string.Empty; set => _receiverDistrict = value; }
        public string ReceiverLocalBody { get => _receiverLocalBody ?? string.Empty; set => _receiverLocalBody = value; }
        public string ReceiverAddress { get => _receiverAddress ?? string.Empty; set => _receiverAddress = value; }
        public string ReceiverContactNumber { get => _receiverContactNumber ?? string.Empty; set => _receiverContactNumber = value; }
        public string ReceiverDocumentType { get => _receiverDocumentType ?? string.Empty; set => _receiverDocumentType = value; }
        public string ReceiverDocumentNumber { get => _receiverDocumentNumber ?? string.Empty; set => _receiverDocumentNumber = value; }
        public string AgentName { get => _agentName ?? string.Empty; set => _agentName = value; }
        public string AgentContactNumber { get => _agentContactNumber ?? string.Empty; set => _agentContactNumber = value; }
        public string AgentAddress { get => _agentAddress ?? string.Empty; set => _agentAddress = value; }
        public string AgentCity { get => _agentCity ?? string.Empty; set => _agentCity = value; }
        public string AgentDistrict { get => _agentDistrict ?? string.Empty; set => _agentDistrict = value; }
        public string AgentCountry { get => _agentCountry ?? string.Empty; set => _agentCountry = value; }
        public string AgentOrganizationName { get => _agentOrganizationName ?? string.Empty; set => _agentOrganizationName = value; }
        //public string ControlNumber { get => _controlNumber ?? string.Empty; set => _controlNumber = value; }
        public string Status { get => _status ?? string.Empty; set => _status = value; }
        public string AgentCommissionNPR { get => _agentCommissionNPR ?? string.Empty; set => _agentCommissionNPR = value; }
        public string ModeOfPayment { get => _modeOfPayment ?? string.Empty; set => _modeOfPayment = value; }
        public string TransactionDate { get => _transactionDate ?? string.Empty; set => _transactionDate = value; }
        public string TransactionDateNepali { get => _transactionDateNepali ?? string.Empty; set => _transactionDateNepali = value; }
    }
}
