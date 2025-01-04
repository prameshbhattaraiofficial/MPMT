using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Core.ViewModel.Role;

public class AddAdminPartnerRoleVm
{
    public int Id { get; set; }
    public string RoleName { get; set; }
    public string Description { get; set; }
    public bool IsSystemRole { get; set; }
    public bool IsActive { get; set; }
}
