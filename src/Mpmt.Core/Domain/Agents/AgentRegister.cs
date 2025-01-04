namespace Mpmt.Core.Domain.Agents;

public class AgentRegister
{
    public int AgentId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string ContactNumber { get; set; }
    public string Address { get; set; }
    public string Message { get; set; }
    public string TypeOfBusiness { get; set; }
    public string Dristrict { get; set; }
    public string DristrictCode { get; set; }
    public string IpAddress { get; set; }
    public bool IsReviewed { get; set; }
    public DateTime? ReviewedLocalDate { get; set; }
    public string UpdatedBy { get; set; }
    public DateTime? CreatedLocalDate { get; set; }
    public DateTime? UpdatedLocalDate { get; set; }
}