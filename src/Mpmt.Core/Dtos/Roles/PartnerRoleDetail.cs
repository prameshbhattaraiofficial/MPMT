namespace Mpmt.Core.Dtos.Roles
{
    public class PartnerRoleDetail
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public bool IsSystemRole { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedLocalDate { get; set; }
        public DateTime CreatedUtcDate { get; set; }
        public string CreatedNepaliDate { get; set; }
    }
}
