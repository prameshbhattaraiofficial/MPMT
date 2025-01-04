using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Core.Domain
{
    public class AgentAuthDefaults
    {
        public const string CookieAuthenticationName = "mpmtAgentauth";

        public const string SuperAgentLoginPath = "/Login";

        public const string AgentLoginPath = "/Login";

        public const string AgentLogoutPath = "/Login";

        public const string AccessDeniedPath = "/errors/forbidden";
    }
}
