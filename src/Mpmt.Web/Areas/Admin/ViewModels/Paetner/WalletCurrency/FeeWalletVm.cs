using System.ComponentModel.DataAnnotations;

namespace Mpmt.Web.Areas.Admin.ViewModels.Paetner.WalletCurrency;

public class FeeWalletVm
{
    public int Id { get; set; }
    public string PartnerCode { get; set; }
    public string SourceCurrency { get; set; }
    public decimal NotificationBalance { get; set; }
    [Required(ErrorMessage = "Notification Balance is required!")]
    public decimal NotificationBalanceLimit { get; set; }
    [Required(ErrorMessage = "Markup min value is required!")]
    public decimal MarkupMinValue { get; set; }
    [Required(ErrorMessage = "Markup max value is required!")]
    public decimal MarkupMaxValue { get; set; }
    [Required(ErrorMessage = "Credit limit required!")]
    public decimal CreditLimit { get; set; }
    public string TypeCode { get; set; }
    public string Remarks { get; set; }
}
