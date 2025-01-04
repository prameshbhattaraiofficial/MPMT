using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mpmt.Agent.Controllers
{
    [Authorize(Roles = "AgentAccess")]
    public class AgentBaseController : Controller
    {
        
    }
}
