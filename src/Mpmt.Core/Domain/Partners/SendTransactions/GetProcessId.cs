using System.ComponentModel.DataAnnotations;

namespace Mpmt.Core.Domain.Partners.SendTransactions
{
    public class GetProcessId
    {
        [Required]
        public string ReferenceId { get; set; }
    }
}
