using Mpmt.Core.Dtos.Paging;

namespace Mpmt.Core.Dtos.CashAgent;

public class CashAgentRegister
{
    public int SN { get; set; } 
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string EmailConfirmed { get; set; }
    public string FullAddress { get; set; }
    public string PhoneNumber { get; set; }
    public string Remarks { get; set; } 
    public string PhoneNumberConfirmed { get; set; }
    public string RegisteredDate { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string FailedLoginAttempt { get; set; }
    public DateTime? TemporaryLockedTillDate { get; set; }
    public bool IsActive { get; set; }
}

public class AgentRegisterFilter : PagedRequest
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string MobileNo { get; set; }
    public int Export { get; set; }
}
