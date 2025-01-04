using Mpmt.Core.Dtos.Pagination;

namespace Mpmt.Core.Dtos.PageModel
{
    /// <summary>
    /// The page model.
    /// </summary>
    public class PageModel : ActionSpecs
    {
        /// <summary>
        /// The sort order.
        /// </summary>
        public enum sortOrder { Ascending = 0, Descending = 1 }
        /// <summary>
        /// Initializes a new instance of the <see cref="PageModel"/> class.
        /// </summary>
        public PageModel()
        {

        }
        /// <summary>
        /// Gets or sets the search name.
        /// </summary>
        public string SearchName { get; set; }
        /// <summary>
        /// Gets or sets the current page value.
        /// </summary>
        public string CurrentPageValue { get; set; }
        /// <summary>
        /// Gets or sets the page sizer.
        /// </summary>
        public string PageSizer { get; set; }
        /// <summary>
        /// Gets or sets the sort expression.
        /// </summary>
        public string sortExpression { get; set; }
    }
}
