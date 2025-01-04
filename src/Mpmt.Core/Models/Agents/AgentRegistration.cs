using System.ComponentModel.DataAnnotations;

namespace Mpmt.Core.Models.Agents;

public class AgentRegistration
{
    [Required]
    [MaxLength(128)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(128)]
    public string LastName { get; set; }

    [Required]
    [MaxLength(20)]
    public string ContactNumber { get; set; }

    [Required]
    [MaxLength(128)]
    public string Address { get; set; }

    [Required]
    [MaxLength(10)]
    public string DristrictCode { get; set; }

    [Required]
    [MaxLength(50)]
    public string Dristrict { get; set; }

    [Required]
    [MaxLength(128)]
    public string TypeOfBusiness { get; set; }

    [MaxLength(2048)]
    public string Message { get; set; }
}
