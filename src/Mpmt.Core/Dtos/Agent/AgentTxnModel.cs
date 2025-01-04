using Microsoft.AspNetCore.Http;
using Mpmts.Core.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Core.Dtos.Agent
{
    public class AgentTxnModel
    {
        public string TransactionId { get; set; }

        public string ControlNumber { get; set; }
        public string PaymentType { get; set; }
        public string PaymentStatus { get; set; }
        public string SourceCurrency { get; set; }
        public string DestinationCurrency { get; set; }
        public string PaymentAmountNPR { get; set; }
        public string SenderFullName { get; set; }
        public string SenderContactNumber { get; set; }
        public string SenderEmail { get; set; }
        public string SenderCountry { get; set; }
        public string SenderAddress { get; set; }
        public string SenderRelationWithReceiver { get; set; }
        public string ReceiverFullName { get; set; }
        public string ReceiverContactNumber { get; set; }
        public string ReceiverEmail { get; set; }
        public string ReceiverCountry { get; set; }
        public string ReceiverProvince { get; set; }
        public string ReceiverDistrict { get; set; }
        public string ReceiverLocalBody { get; set; }
        public string ReceiverAddress { get; set; }
        public string ReceiverRelationWithSender { get; set; }
        public string IssueDateAD { get; set; }


        public string AgentCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Id_ExpiryDateBS { get; set; }
        public string IpAddress { get; set; }
        public string DeviceId { get; set; }
        public string processId { get; set; }
        public string IDTypeName { get; set; }



        [Required(ErrorMessage ="Mobile number is required!")]
        public string MobileNumber { get; set; }
        [Required(ErrorMessage ="Indentification type is required!")]
        public string IdentificationType { get; set; }
        [Required(ErrorMessage ="Identification number is required!")]
        public string IndentificationNumber { get; set; }
        [Required(ErrorMessage ="Issue date is required!")]
        public string IssueDate { get; set; }
        public string Id_IssuedDateBS { get; set; }
        [Required(ErrorMessage ="Expiry date is required!")]
        public string ExpiryDate { get; set; }
        public IFormFile UploadImage { get; set; }
        public IFormFile UploadBackImage { get; set; }
        public string UploadBackImagePath { get; set; }
        public string UploadImagePath { get; set; }

        [Required(ErrorMessage ="Relationship is required!")]
        public string RelationShip { get; set; }
        [Required(ErrorMessage ="Transaction purpose is required!")]
        public string TransactionPurpose { get; set; }

        public string RemarksPurpose { get; set; }

       
    }
}
