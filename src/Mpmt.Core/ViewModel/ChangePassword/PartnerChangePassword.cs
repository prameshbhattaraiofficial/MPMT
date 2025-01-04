using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Core.ViewModel.ChangePassword
{
    public class PartnerChangePassword
    {
        public string Event { get; set; }
        public string PasswordHash { get; set; }
		public string PasswordSalt {get;set;}
        public string Id {get;set;}
        public string PartnerCode {get;set;}
    }
}
