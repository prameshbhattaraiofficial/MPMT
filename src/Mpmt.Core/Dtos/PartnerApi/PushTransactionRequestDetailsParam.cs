using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Core.Dtos.PartnerApi
{
    public class PushTransactionRequestDetailsParam
    {
        public string PartnerCode { get; set; }
        public string PartnerTransactionId { get; set; }
        public string ProcessId { get; set; }
        public string SourceCurrency { get; set; }
        public string DestinationCurrency { get; set; }
        public decimal SendingAmount { get; set; }
        public string PaymentType { get; set; }
        public decimal NetReceivingAmount { get; set; }
        //public decimal ServiceCharge { get; set; }
        //public decimal NetSendingAmount { get; set; }
        //public decimal ConversionRate { get; set; }
        //public decimal NetRecievingAmountNPR { get; set; }
        //public decimal PartnerServiceCharge { get; set; }
        public string TransactionType { get; set; }
        public string SenderFirstName { get; set; }
        public string SenderLastName { get; set; }
        public string SenderContactNumber { get; set; }
        public string SenderEmail { get; set; }
        //public string SenderCountryCode { get; set; }
        public string SenderCountry { get; set; }
        public string SenderProvince { get; set; }
        public string SenderCity { get; set; }
        public string SenderZipcode { get; set; }
        public string SenderAddress { get; set; }
        //public string SenderDocumentTypeId { get; set; }
        public string SenderDocumentType { get; set; }
        public string SenderDocumentNumber { get; set; }
        //public int SenderRelationshipId { get; set; }
        //public int SenderPurposeId { get; set; }
        //public string SenderOccupationId { get; set; }
        public string SenderRelationshipWithRecipient { get; set; }
        public string SenderOccupation { get; set; }
        public string SenderSourceOfIncome { get; set; }
        public string SenderPurposeOfRemittance { get; set; }
        public string SenderRemarks { get; set; }
        public string RecipientType { get; set; }
        public string RecipientFirstName { get; set; }
        public string RecipientLastName { get; set; }
        public string JointAccountFirstName { get; set; }
        public string JointAccountLastName { get; set; }
        public string BusinessName { get; set; }
        public string RecipientContactNumber { get; set; }
        public string RecipientEmail { get; set; }
        public DateTime? RecipientDateOfBirth { get; set; }
        //public string RecipientCountryCode { get; set; }
        public string RecipientCountry { get; set; }
        public string RecipientCity { get; set; }
        public string RecipientZipcode { get; set; }
        public string RecipientAddress { get; set; }
        //public int RecipientRelationshipId { get; set; }
        public string RecipientRelationshipWithSender { get; set; }
        public string BankCode { get; set; }
        public string Branch { get; set; }
        public string AccountHolderName { get; set; }
        public string AccountNumber { get; set; }
        public string WalletCode { get; set; }
        public string WalletNumber { get; set; }
        public string WalletHolderName { get; set; }

        // set internally
        public string IpAddress { get; set; }
        public string DeviceId { get; set; }
        public string LoggedInUser { get; set; }
        public string UserType { get; set; }
    }
}
