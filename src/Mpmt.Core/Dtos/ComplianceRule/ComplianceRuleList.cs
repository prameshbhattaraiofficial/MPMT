using System.Security.AccessControl;

namespace Mpmt.Core.Dtos.ComplianceRule
{
    public class ComplianceRuleList
    {
        public int Id { get; set; }
        public int SN { get; set; }
        public string ComplianceCode { get; set; }
        public string ComplianceRule { get; set; }
        public string Description { get; set; }
        //public int Count { get; set; }
        //public string NoOfDays { get; set; }
        public decimal CountValue { get; set; }
        public string Frequency { get; set; }
        public string ComplianceAction { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedByName { get; set; }
    }

    public class ComplianceRuleDetail
    {
        public int Id { get; set; }
        public decimal? CountValue { get; set; }
        public string Frequency { get; set; }
        public string ComplianceAction { get; set; }
        public bool IsActive { get; set; }

    }
 
}
