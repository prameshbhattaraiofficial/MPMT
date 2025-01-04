using Mpmt.Core.Dtos.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Core.Dtos.PartnerApplications
{
    public class PartnerApplicationsFilter : PagedRequest
    {
        public string FullName { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationEmail { get; set; }
        public string OrganizationContactNo { get; set; }
        public int Export { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class PublicFeedbacksFilter : PagedRequest   
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string ContactNo { get; set; }
        public int Export { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
