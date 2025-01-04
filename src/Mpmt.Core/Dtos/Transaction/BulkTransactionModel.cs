using DocumentFormat.OpenXml.Office2019.Drawing.Model3D;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Core.Dtos.Transaction
{
    public class BulkTransactionModel
    {
        public IFormFile uploadFile { get; set; }
    }
    public class BulkTransactionDetailsModel
    {
        public string PaymentType { get; set; }
        public string SourceCurrency { get; set; }
        public string DestinationCurrency { get; set; }
        public string SendingAmount { get; set; }
        public string ReceivingAmount { get; set; }

        public string SenderName { get; set; }
        public string SendeContactNumber { get; set; }
        public string SenderEmail { get; set; }
        public string SenderCountry { get; set; }
        public string SenderCity { get; set; }
        public string SenderAddress { get; set; }
        public string SenderDocType { get; set; }
        public string SenderDocNumber{ get; set; }
        //public string SenderIDexpiryDate { get; set; }
        public string RelationshipWithBeneficiary { get; set; }
        public string SenderOccupation { get; set; }
        public string SenderSourceOfIncome { get; set; }
        public string PurposeOfRemittance { get; set; }
        public string BeneficiaryName { get; set; }
        public string BeneficiaryContactNumber { get; set; }
        public string BeneficiaryDOB { get; set; }
        public string BeneficiaryCountry { get; set; }
        public string BeneficiaryCity { get; set; }
        public string BeneficiaryAddress { get; set; }
        public string BeneficiaryRelationwithSender { get; set; }
        public string BeneficiaryBankCode { get; set; }
        public string BeneficiaryBankName { get; set; }
        public string BeneficiaryBankBranch { get; set; }
        public string BankAccountNo { get; set; }
        public string WalletCode { get; set; }
        public string WalletID { get; set; }   
    }

    public class BulkTxnBaseModel
    {
        public string TransactionType { get; set; }
        public string PartnerCode { get; set; }
        public string IpAddress { get; set; }
        public string LoggedInUser { get; set; }
        public string UserType { get; set; }
        public string ProcessId { get; set; }
        public string DeviceId { get; set; }
        public string FileName { get; set; }

        public byte[] SendExcelFile {  get; set; }
        public string PartnerEmail {  get; set; }
    }


    public class BulkTransactionResponse
    {
        public string PaymentType { get; set; }
        public string SourceCurrency { get; set; }
        public string DestinationCurrency { get; set; }
        public string SendingAmount { get; set; }
        public string ReceivingAmount { get; set; }

        public string SenderName {  get; set; } 
        public string SendeContactNumber { get; set; }
        public string SenderEmail { get; set; }
        public string SenderCountry { get; set; }
        public string SenderCity { get; set; }
        public string SenderAddress { get; set; }
        public string SenderDocType { get; set; }
        public string SenderDocNumber { get; set; }
        public string RelationshipWithBeneficiary { get; set; }
        public string SenderOccupation { get; set; }
        public string SenderSourceOfIncome { get; set; }
        public string PurposeOfRemittance { get; set; }
        public string BeneficiaryName { get; set; }
        public string BeneficiaryContactNumber { get; set; }
        public string BeneficiaryDOB { get; set; }
        public string BeneficiaryCountry { get; set; }
        public string BeneficiaryCity { get; set; }
        public string BeneficiaryAddress { get; set; }
        //public string BeneficiaryDocument { get; set; }
        public string BeneficiaryRelationwithSender { get; set; }
        public string BeneficiaryBankCode { get; set; }
        public string BeneficiaryBankName { get; set; }
        public string BeneficiaryBankBranch { get; set; }
        public string BankAccountNo { get; set; }
        public string WalletCode { get; set; }
        public string WalletID { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
    }
}
