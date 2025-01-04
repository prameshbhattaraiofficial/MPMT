using Mpmt.Core.Dtos.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Core.Models.Route
{
    public class Routelocation
    {
        public string Action { get; set; }
        public string Controller { get; set; }
        public string Area { get; set; }
    }
    public class Controlleraction
    {
        public string Areacontrolleraction { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string Area { get; set; }
        public string CreatedBy { get; set; }
  
    }
    public class GetcontrollerAction
    {
        public int Id { get; set; }
        public string Areacontrolleraction { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string Area { get; set; }
        public bool Permission { get; set;}
        public int RoleId { get; set;}
        public string DisplayName { get; set; }
        public bool IsActive { get; set; }

    }
    public class AddcontrollerAction
    {
       
        public int RoleId { get; set; }
        public List<MenucontrollerAction> MenusIds { get; set;}

    }

    public class AddControllerActionUserType
    {
        public string UserType { get; set; }
        public string CreatedBy { get; set; }
        public List<MenucontrollerAction> MenusIds { get; set; }

    }

    public class MenucontrollerAction 
    {

            public int Id { get; set; }
            public bool Permission { get; set; }

    }

    public class ArrangedRoutelocation
    {
        public string Area { get; set; }
        public List<Loccontrollers> Controllers { get;}

    }
    public class Loccontrollers
    {
        public string ControllerName { get; set; }
        public List<locActions> Actions { get; set; }
    }
    public class locActions
    {
        public string Action { get; set; }
    }
    public class menuupdateRequest
    {
        public int RoleId { get; set; }
        public list<menupremission> menupremissions { get; set; }
    }

    public class menupremission : Routelocation
    {
        public bool Permission { get; set; }
    }
}
