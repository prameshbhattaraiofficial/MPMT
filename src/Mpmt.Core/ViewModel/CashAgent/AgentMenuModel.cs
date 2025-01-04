namespace Mpmt.Core.ViewModel.CashAgent;

public class AgentMenuModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Area { get; set; }
    public string Controller { get; set; }
    public string Action { get; set; }
    public string MenuUrl { get; set; }
    public int ParentId { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
    public string ImagePath { get; set; }
}
