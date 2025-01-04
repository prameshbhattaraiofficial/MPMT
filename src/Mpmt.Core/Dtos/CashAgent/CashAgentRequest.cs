namespace Mpmt.Core.Dtos.CashAgent;

public class CashAgentRequest
{
    public Guid Id { get; set; }
    public string OperationMode { get; set; }
    public string LoggedInUser { get; set; }
    public string UserType { get; set; }
    public string Email { get; set; }
}
