namespace Mpmt.Core.Dtos.Pagination
{
    /// <summary>
    /// The pagination summary model.
    /// </summary>
    public class PaginationSummaryModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PaginationSummaryModel"/> class.
        /// </summary>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="pageItemsCount">The page items count.</param>
        /// <param name="matchedRecords">The matched records.</param>
        /// <param name="totalRecords">The total records.</param>
        public PaginationSummaryModel(int pageIndex, int pageSize, int pageItemsCount, int matchedRecords, int totalRecords)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            PageItemsCount = pageItemsCount;
            MatchedRecords = matchedRecords;
            TotalRecords = totalRecords;
        }

        /// <summary>
        /// Gets or sets the page index.
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// Gets or sets the page size.
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// Gets or sets the page items count.
        /// </summary>
        public int PageItemsCount { get; set; }
        /// <summary>
        /// Gets or sets the matched records.
        /// </summary>
        public int MatchedRecords { get; set; }
        /// <summary>
        /// Gets or sets the total records.
        /// </summary>
        public int TotalRecords { get; set; }
    }
}
