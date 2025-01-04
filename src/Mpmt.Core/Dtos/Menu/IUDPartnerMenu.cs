using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Core.Dtos.Menu
{
    public class IUDPartnerMenu
    {

        public int Id { get; set; }

        public string Title { get; set; }

        public int ParentId { get; set; } = 0;

        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }

        public bool IsActive { get; set; }

        public int DisplayOrder { get; set; }

        public string ImagePath { get; set; }

        public string CreatedBy { get; set; }

        public string CreatedNepaliDate { get; set; }

        public string UpdatedBy { get; set; }

        public string UpdatedNepaliDate { get; set; }
    }
}
