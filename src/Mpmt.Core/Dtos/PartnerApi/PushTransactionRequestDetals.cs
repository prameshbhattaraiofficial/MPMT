using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Core.Dtos.PartnerApi
{
    public class PushTransactionRequestDetals
    {
        public string ApiUserName { get; set; }

        public string ProcessId { get; set; }

        public string PartnerTransactionId { get; set; }

        public string PaymentType { get; set; }

        public string SourceCurrency { get; set; }

        public string DestinationCurrency { get; set; }

        public string SendingAmount { get; set; }
        public string NetReceivingAmount { get; set; }

        //public string ServiceCharge { get; set; }

        //public string NetSendingAmount { get; set; }

        //public string ConversionRate { get; set; }

        //public string NetRecievingAmountNPR { get; set; }

        //public string PartnerServiceCharge { get; set; }



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

        //public string SenderRelationshipId { get; set; }

        //public string SenderPurposeId { get; set; }

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

        public string RecipientDateOfBirth { get; set; }

        //public string RecipientCountryCode { get; set; }
        public string RecipientCountry { get; set; }

        public string RecipientCity { get; set; }

        public string RecipientZipcode { get; set; }

        public string RecipientAddress { get; set; }

        //public string RecipientRelationshipId { get; set; }
        public string RecipientRelationshipWithSender { get; set; }

        public string BankCode { get; set; }

        public string Branch { get; set; }

        public string AccountHolderName { get; set; }

        public string AccountNumber { get; set; }

        public string WalletCode { get; set; }

        public string WalletNumber { get; set; }

        public string WalletHolderName { get; set; }

        public string Signature { get; set; }
    }
}
