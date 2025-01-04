namespace Mpmt.Core.Dtos.Paging
{
    /// <summary>
    /// The page info.
    /// </summary>
    public class PageInfo
    {
        /// <summary>
        /// Gets or sets the page number.
        /// </summary>
        public int PageNumber { get; set; }
        /// <summary>
        /// Gets or sets the page size.
        /// </summary>
        public int PageSize { get; set; }
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
