using Microsoft.AspNetCore.Http;
using Mpmt.Core.Common.Attribites;

namespace Mpmt.Core.Dtos.SuperAgent;

public class AddAgentFundRequest
{
    public char Event { get; set; }
    public string AgentCode { get; set; }
    public string FundType { get; set; }
    public string SourceCurrency { get; set; }
    public decimal Amount { get; set; }
    public decimal NotificationBalance { get; set; }
    public string TransactionId { get; set; }
    public string VoucherImgPath { get; set; }
    public string Remarks { get; set; }
    public IFormFile VoucherImg { get; set; }
    public string UserType { get; set; }
    public string LoggedInUser { get; set; }
}
