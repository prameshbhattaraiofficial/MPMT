using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Core.ViewModel.Menu
{
    public class PartnerMenu
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int ParentId { get; set; }

        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }

        public bool IsActive { get; set; }


        public int DisplayOrder { get; set; }

        public string ImagePath { get; set; }

        public List<PartnerMenu> child { get; set; }
    }
    public class PartnerMenuWithPermission : PartnerMenu
    {
        public List<PartnerMenuWithPermission> child { get; set; }
        public bool Permission { get; set; }
    }
}
