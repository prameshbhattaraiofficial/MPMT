using Microsoft.AspNetCore.Mvc;
using Mpmt.Core.Dtos.Pagination;

namespace Mpmt.Web.Areas.Admin.Pagination
{
    /// <summary>
    /// The sort order view component.
    /// </summary>
    public class SortOrderViewComponent : ViewComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SortOrderViewComponent"/> class.
        /// </summary>
        public SortOrderViewComponent()
        {

        }

        /// <summary>
        /// Invokes the.
        /// </summary>
        /// <param name="sortOrderModel">The sort order model.</param>
        /// <returns>An IViewComponentResult.</returns>
        public IViewComponentResult Invoke(SortOrderModel sortOrderModel)
        {
            return View(sortOrderModel);
        }
    }
}
