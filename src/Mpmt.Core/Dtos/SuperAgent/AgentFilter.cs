using Mpmt.Core.Dtos.Paging;

namespace Mpmt.Core.Dtos.SuperAgent
{
    public class AgentFilter : PagedRequest
    {
        public string AgentCode { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string DistrictCode { get; set; }
        public string UserType { get; set; }
        public int Export { get; set; }
        public string UserStatus { get; set; }
        public string SuperAgentCode { get; set; }
        public string AgentName { get; set; }
    }
}
