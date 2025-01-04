using Mpmt.Core.ViewModel.Menu;

namespace Mpmt.Core.ViewModel.CashAgent;

public class AgentMenuChild
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Area { get; set; }
    public string Controller { get; set; }
    public string Action { get; set; }  
    public int ParentId { get; set; }
    public string MenuUrl { get; set; }
    public bool Permission { get; set; }
    public bool Status { get; set; }
    public int DisplayOrder { get; set; }
    public string ImagePath { get; set; }
    public List<AgentMenuChild> Child { get; set; } = new List<AgentMenuChild>();   
}


