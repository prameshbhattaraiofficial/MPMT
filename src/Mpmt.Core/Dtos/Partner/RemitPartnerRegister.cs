using Mpmt.Core.Dtos.Paging;
using System.ComponentModel.DataAnnotations;

namespace Mpmt.Core.Dtos.Partner
{
    public class RemitPartnerRegister
    {

        public string SN { get; set; }
        public Guid Id { get; set; }
        public string FullName { get; set; }
        [Required]
        public string Shortname { get; set; }   
        public string Email { get; set; }
        public string EmailConfirmed { get; set; }
        public string PartnerCode { get; set; }
        public string Address { get; set; }
        public string Post { get; set; }
        public string MobileNumber { get; set; }
        public string Remarks { get; set; }
        public string MobileConfirmed { get; set; }
        public string RegisteredDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string FailedLoginAttempt { get; set; }
        public DateTime? TemporaryLockedTillDate { get; set; }
        public bool IsActive { get; set; }
    }
    public class RemitPartnerRegisterFilter : PagedRequest
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string PartnerCode { get; set; }
        public int Export { get; set; }
    }
}

