namespace Mpmt.Core.ViewModel.Role
{
    public class UpdatePartnerRoleVm
    {
        public int Id { get; set; }
        public char Event { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public bool IsSystemRole { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string LoggedInUser { get; set; }
        public string UserType { get; set; }
    }
}
