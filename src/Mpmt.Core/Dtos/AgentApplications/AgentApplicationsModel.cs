namespace Mpmt.Core.Dtos.AgentApplications;

public class AgentApplicationsModel
{
    public int Sn { get; set; }
    public int AgentId { get; set; }
    public string FullName { get; set; }    
    public string ContactNumber { get; set; }   
    public string Address { get; set; }
    public string Message { get; set; }
    public string TypeOfBusiness { get; set; }
    public string District { get; set; }
    public string DistrictCode { get; set; }
    public string IpAddress { get; set; }
    public bool IsReviewed { get; set; }
    public DateTime? ReviewedLocalDate { get; set; }
    public string UpdatedBy { get; set; }
    public DateTime? CreatedLocalDate { get; set; }
    public DateTime? UpdatedLocalDate { get; set; }
}
