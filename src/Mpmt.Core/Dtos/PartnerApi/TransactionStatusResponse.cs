using Mpmt.Core.Domain;

namespace Mpmt.Core.Dtos.PartnerApi
{
    public class TransactionStatusResponse : ApiResponse
    {
        private string _transactionId;
        private string _paymentType;
        private string _transactionDate;
        private string _partnerTransactionId;
        private string _payoutStatusCode;
        private string _complianceStatusCode;
        private string _debitStatusCode;
        private string _payoutDate;
        private string _transactionStateRemarks;

        public string TransactionId { get => _transactionId ?? string.Empty; set => _transactionId = value; }
        public string PartnerTransactionId { get => _partnerTransactionId ?? string.Empty; set => _partnerTransactionId = value; }
        public string PaymentType { get => _paymentType ?? string.Empty; set => _paymentType = value; }
        public string PayoutStatusCode { get => _payoutStatusCode ?? string.Empty; set => _payoutStatusCode = value; }
        public string ComplianceStatusCode { get => _complianceStatusCode ?? string.Empty; set => _complianceStatusCode = value; }
        public string DebitStatusCode { get => _debitStatusCode ?? string.Empty; set => _debitStatusCode = value; }
        public string TransactionDate { get => _transactionDate ?? string.Empty; set => _transactionDate = value; }
        public string PayoutDate { get => _payoutDate ?? string.Empty; set => _payoutDate = value; }
        public string TransactionStateRemarks { get => _transactionStateRemarks ?? string.Empty; set => _transactionStateRemarks = value; }
    }
}
