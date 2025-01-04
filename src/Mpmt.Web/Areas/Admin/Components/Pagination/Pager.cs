using Microsoft.AspNetCore.Mvc;
using Mpmt.Core.Dtos.Pagination;

namespace Mpmt.Web.Areas.Admin.Pagination
{
    /// <summary>
    /// The pager.
    /// </summary>
    public class Pager : ViewComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Pager"/> class.
        /// </summary>
        public Pager()
        {

        }

        /// <summary>
        /// Invokes the.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>An IViewComponentResult.</returns>
        public IViewComponentResult Invoke(PagerModel model)
        {
            return View(model);
        }
    }
}
