using Microsoft.AspNetCore.Mvc;

namespace Mpmt.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// The global settings controller.
    /// </summary>
    public class GlobalSettingsController : BaseAdminController
    {
        /// <summary>
        /// Indices the.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IActionResult> Index()
        {
            return await Task.FromResult(View());
        }
    }
}
