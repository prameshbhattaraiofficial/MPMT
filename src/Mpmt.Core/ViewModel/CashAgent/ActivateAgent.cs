namespace Mpmt.Core.ViewModel.CashAgent
{
    public class ActivateAgent
    {
        public string AgentCode { get; set; }
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
    }

    public class ActivateAgentEmployee
    {
        public string EmployeeId { get; set; }
        public string UserName { get; set; }
        public string AgentCode { get; set; }
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
    }

    public class Withdraw
    {
        public string AgentCode { get; set; }
        public string Remarks { get; set; }
        public string LoggedInUser { get; set; }
        public string UserType { get; set; }    
    }
}
