namespace Mpmt.Core.Dtos.Pagination
{
    /// <summary>
    /// The pagination footer.
    /// </summary>
    public class PaginationFooter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PaginationFooter"/> class.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="pageItemsCount">The page items count.</param>
        /// <param name="filteredCount">The filtered count.</param>
        /// <param name="totalCount">The total count.</param>
        /// <param name="totalPages">The total pages.</param>
        public PaginationFooter(int pageNumber, int pageSize, int pageItemsCount, int filteredCount, int totalCount, int totalPages)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            PageItemsCount = pageItemsCount;
            FilteredCount = filteredCount;
            TotalCount = totalCount;
            TotalPages = totalPages;
        }

        /// <summary>
        /// Gets or sets the page number.
        /// </summary>
        public int PageNumber { get; set; }
        /// <summary>
        /// Gets or sets the page size.
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// Gets or sets the page items count.
        /// </summary>
        public int PageItemsCount { get; set; }
        /// <summary>
        /// Gets or sets the filtered count.
        /// </summary>
        public int FilteredCount { get; set; }
        /// <summary>
        /// Gets or sets the total count.
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// Gets or sets the total pages.
        /// </summary>
        public int TotalPages { get; set; }
    }
}
