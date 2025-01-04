using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Core.Dtos.Transaction
{
	public class PayoutTransactionDetail
	{
		public string TransactionId { get; set; }
		public string PartnerCode { get; set; }
		public string PartnerName { get; set; }
		public string PartnerCountryCode { get; set; }
		public string SourceCurrency { get; set; }
		public string DestinationCurrency { get; set; }
		public decimal? SendingAmount { get; set; }
		public string ServiceCharge { get; set; }
		public string ConversionRate { get; set; }
		public string NetSendingAmount { get; set; }
		public string NetReceivingAmount { get; set; }
		public string CreditSendingAmount { get; set; }
		public string SenderRegistered { get; set; }
		public string MemberId { get; set; }
		public string RecipientRegistered { get; set; }
		public string RecipientId { get; set; }
		public string Sign { get; set; }
		public string PartnerServiceCharge { get; set; }
		public string PartnerCommission { get; set; }
		public string TransactionType { get; set; }
		public string WalletType { get; set; }
		public string CurrentBalance { get; set; }
		public string PreviousBalance { get; set; }
		public string TokenNumber { get; set; }
		public string PaymentTypeId { get; set; }
		public string PaymentType { get; set; }
		public string BankName { get; set; }
		public string BankCode { get; set; }
		public string Branch { get; set; }
		public string AccountHolderName { get; set; }
		public string AccountNumber { get; set; }
		public string WalletName { get; set; }
		public string WalletCode { get; set; }
		public string WalletNumber { get; set; }
		public string WalletHolderName { get; set; }
		public string TransactionApproval { get; set; }
		public string ProcessId { get; set; }
		public string PartnerTransactionId { get; set; }
		public string PartnerTrackerId { get; set; }
		public string PartnerStatus { get; set; }
		public string AgentCode { get; set; }
		public string AgentTransactionId { get; set; }
		public string AgentTrackerId { get; set; }
		public string AccountStatus { get; set; }
		public string ContactNumber { get; set; }
		public string IsAccountValidated { get; set; }
		public string Message { get; set; }
		public string ResponseMessage { get; set; }
		public string AgentResponseCode { get; set; }
		public string AgentStatus { get; set; }
		public string AgentStatusCode { get; set; }
		public string AgentTransactionStatus { get; set; }
		public string GatewayTxnId { get; set; }
		public string GatewayStatus { get; set; }
		public string ComplianceStatusCode { get; set; }
		public string ComplianceStatus { get; set; }
		public string StatusCode { get; set; }
		public string ReasonCode { get; set; }
		public string ParentTransactionId { get; set; }
		public string IpAddress { get; set; }
		public string DeviceId { get; set; }
		public string CreatedUserType { get; set; }
		public string CreatedById { get; set; }
		public string CreatedByName { get; set; }
		public string CreatedDate { get; set; }
		public string SenderCreatedDate { get; set; }
		public string ReceiverCreatedDate { get; set; }
	}										
}
