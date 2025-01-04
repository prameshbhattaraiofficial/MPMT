namespace Mpmt.Core.Dtos.CashAgent;

public class IUDAgentMenu   
{
    public char Event { get; set; }
    public int Id { get; set; }
    public string Title { get; set; }
    public int ParentId { get; set; }
    public string MenuUrl { get; set; }
    public string Area { get; set; }
    public string Controller { get; set; }
    public string Action { get; set; }
    public bool IsActive { get; set; }
    public int DisplayOrder { get; set; }
    public string ImagePath { get; set; }
    public string CreatedBy { get; set; }
}
