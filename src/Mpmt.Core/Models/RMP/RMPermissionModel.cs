using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Core.Models.RMP
{
    public class RMPermissionModel
    {
        public int MenuId { get; set; }

        public string Title { get; set; }

        public int ParentId { get; set; }


        public string MenuUrl { get; set; }
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }


        public int DisplayOrder { get; set; }

        public string ImagePath { get; set; }

        public bool ViewPer { get; set; }

        public bool CreatePer { get; set; }

        public bool UpdatePer { get; set; }

        public bool DeletePer { get; set; }
    }
    public class MenuSubMenu : RMPermissionModel 
    {
        public List<RMPermissionModel> submenus { get; set; }
    }
    public class ActionPermission
    {
        public string Action { get; set; }
        public bool Permission { get;set; }
    }


}
