using Mpmt.Core.Dtos.Paging;

namespace Mpmt.Core.Dtos.ComplianceRule
{
    public class ComplianceRuleFilter : PagedRequest
    {
        public string ComplianceCode { get; set; }
        public string ComplianceRule { get; set; }
        public string ComplianceAction { get; set; }
        public string LoggedInUser { get; set; }
    }
}