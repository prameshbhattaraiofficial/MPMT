using Microsoft.AspNetCore.Mvc;
using Mpmt.Core.Dtos.Pagination;

namespace Mpmt.Agent.Components.Pagination
{
    /// <summary>
    /// The pagination summary.
    /// </summary>
    public class PaginationSummary : ViewComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PaginationSummary"/> class.
        /// </summary>
        public PaginationSummary()
        {

        }
        /// <summary>
        /// Invokes the.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>An IViewComponentResult.</returns>
        public IViewComponentResult Invoke(PaginationSummaryModel model)
        {
            return View(model);
        }

    }
}
