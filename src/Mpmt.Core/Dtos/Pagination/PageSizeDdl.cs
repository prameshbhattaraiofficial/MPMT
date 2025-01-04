namespace Mpmt.Core.Dtos.Pagination
{
    /// <summary>
    /// The page size ddl.
    /// </summary>
    public class PageSizeDdl : ActionSpecs
    {
        /// <summary>
        /// Gets or sets the page size options.
        /// </summary>
        public int[] PageSizeOptions { get; set; }
        /// <summary>
        /// Gets or sets the page size.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageSizeDdl"/> class.
        /// </summary>
        /// <param name="pageSize">The page size.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="action">The action.</param>
        public PageSizeDdl(int pageSize, string controller, string action)
        {
            PageSizeOptions = new int[] { 10, 20, 30, 50, 100 };
            PageSize = pageSize;

            Controller = controller;
            Action = action;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageSizeDdl"/> class.
        /// </summary>
        /// <param name="pageSizeOptions">The page size options.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="action">The action.</param>
        public PageSizeDdl(int[] pageSizeOptions, int pageSize, string controller, string action)
        {
            PageSizeOptions = pageSizeOptions;
            PageSize = pageSize;

            Controller = controller;
            Action = action;
        }
    }
}
