using Microsoft.AspNetCore.Mvc;
using Mpmt.Core.Dtos.Pagination;

namespace Mpmt.Agent.Components.Pagination
{
    /// <summary>
    /// The sort by view component.
    /// </summary>
    public class SortByViewComponent : ViewComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SortByViewComponent"/> class.
        /// </summary>
        public SortByViewComponent()
        {

        }

        /// <summary>
        /// Invokes the.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>An IViewComponentResult.</returns>
        public IViewComponentResult Invoke(SortByModel model)
        {
            return View(model);
        }
    }
}
