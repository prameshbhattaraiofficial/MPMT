namespace Mpmt.Core.Dtos.PartnerApplications
{
    public class PartnerApplicationsModel
    {
        public int Sn { get; set; }
        public string ApplicationId { get; set; }
        public string FullName { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationEmail { get; set; }
        public string OrganizationContactNo { get; set; }
        public string Designation { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public string Message { get; set; }
        public string IpAddress { get; set; }
        public bool IsReviewed { get; set; }
        public string UpdatedBy { get; set; }
        public string ReviewedLocalDate { get; set; }
        public string CreatedLocalDate { get; set; }
        public string UpdatedLocalDate { get; set; }
    }

    public class PublicFeedbacksModel
    {
        public int Sn { get; set; } 
        public string FeedbackId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string ContactNo { get; set; }
        public string Subject { get; set; } 
        public string Message { get; set; }
        public string IpAddress { get; set; }
        public bool IsReviewed { get; set; }
        public string UpdatedBy { get; set; }
        public string ReviewedLocalDate { get; set; }
        public string CreatedLocalDate { get; set; }
        public string UpdatedLocalDate { get; set; }
    }
}
