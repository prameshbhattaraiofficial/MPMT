using Microsoft.AspNetCore.Mvc;
using Mpmt.Core.Dtos.Pagination;

namespace Mpmt.Web.Areas.Admin.Pagination
{
    /// <summary>
    /// The page sizer.
    /// </summary>
    public class PageSizer : ViewComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PageSizer"/> class.
        /// </summary>
        public PageSizer()
        {
        }

        /// <summary>
        /// Invokes the.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>An IViewComponentResult.</returns>
        public IViewComponentResult Invoke(PageSizeDdl model)
        {
            return View(model);
        }

    }
}
