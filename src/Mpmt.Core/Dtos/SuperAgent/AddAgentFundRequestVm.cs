using Microsoft.AspNetCore.Http;
using Mpmt.Core.Common.Attribites;
using System.ComponentModel.DataAnnotations;

namespace Mpmt.Core.Dtos.SuperAgent;

public class AddAgentFundRequestVm
{
    public string AgentCode { get; set; }
    public string SuperAgentName { get; set; }
    public string FundType { get; set; }
    public string SourceCurrency { get; set; }
    [Required]
    public decimal Amount { get; set; }
    [Required]
    public decimal NotificationBalance { get; set; }
    [Required]
    public string TransactionId { get; set; }
    public string VoucherImgPath { get; set; }
    [Required]
    public string Remarks { get; set; }
    [MaxFileSize]
    [Required]
    [AllowedExtensions]
    public IFormFile? VoucherImg { get; set; }
}
