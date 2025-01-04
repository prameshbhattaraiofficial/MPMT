namespace Mpmt.Core.Dtos.Roles;

public class IUDPartnerRole
{
    public char Event { get; set; }

    public int Id { get; set; } 

    public string RoleName { get; set; }

    public string Description { get; set; }

    public bool IsSystemRole { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public string LoggedInUser { get; set; }

    public string UserType { get; set; }
}
