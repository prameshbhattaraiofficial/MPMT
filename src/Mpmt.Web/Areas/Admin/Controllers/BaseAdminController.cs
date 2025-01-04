using Microsoft.AspNetCore.Mvc;
using Mpmt.Core;
using Mpmt.Web.Controllers;
using System.Web.Mvc;

namespace Mpmt.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// The base admin controller.
    /// </summary>
    [Area(AreaNames.Admin)]

    [Authorize(Roles = "AdminAccess")]
    public abstract partial class BaseAdminController : BaseController
    {
    }
}
