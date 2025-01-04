using System.ComponentModel.DataAnnotations;

namespace Mpmt.Core.Dtos.Adjustment;

public class AdjustmentWallet
{
    public string WalletCurrency { get; set; }
    public string PartnerCode { get; set; }

    [Required(ErrorMessage = "Amount is required")]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "Type is required")]
    public string Type { get; set; }

    [Required(ErrorMessage = "Remarks is required")]
    [MaxLength(250, ErrorMessage = "Maxlength for Remarks is 250")]
    public string Remarks { get; set; }
}

public class AdjustmentWalletDTO
{   
    public char OperationMode { get; set; }
    public string WalletCurrency { get; set; }
    public string PartnerCode { get; set; }
    public decimal Amount { get; set; }
    public string Type { get; set; }
    public string Remarks { get; set; }
    public string LoggedInUser { get; set; }
    public string UserType { get; set; }    
}
