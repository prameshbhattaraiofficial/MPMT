using System.ComponentModel.DataAnnotations;

namespace Mpmt.Agent.Models.TransactionSearch
{
    public class TransactionDetailsSearchByMTCN
    {
        [StringLength(16, MinimumLength = 16, ErrorMessage = "Control number should be 16 digits!")]
        [Required(ErrorMessage ="Control Number is required !")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Only alphanumeric characters are allowed.")]
        public string ControlNumber { get;set; }
        public string ControlNumberEncrypted { get;set; }
    }
    
}
