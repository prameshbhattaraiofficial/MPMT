namespace Mpmt.Core.Domain.Partners.Applications
{
    public class PartnerApplication
    {
        public int ApplicationId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationEmail { get; set; }
        public string OrganizationContactNo { get; set; }
        public string Designation { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string Address { get; set; }
        public string Message { get; set; }
        public string IpAddress { get; set; }
        public bool IsReviewed { get; set; }
        public DateTime? ReviewedLocalDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? CreatedLocalDate { get; set; }
        public DateTime? UpdatedLocalDate { get; set; }
    }
}
