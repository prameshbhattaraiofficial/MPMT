namespace Mpmt.Core.ViewModel.Role;

public class UpdateAdminPartnerRoleVm
{
    public int Id { get; set; }
    public string RoleName { get; set; }
    public string Description { get; set; }
    public bool IsSystemRole { get; set; }
    public bool IsActive { get; set; }
}
