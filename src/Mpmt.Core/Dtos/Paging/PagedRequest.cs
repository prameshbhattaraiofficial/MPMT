namespace Mpmt.Core.Dtos.Paging
{
    /// <summary>
    /// The paged request.
    /// </summary>
    public class PagedRequest
    {
        /// <summary>
        /// Gets the max page size.
        /// </summary>
        public virtual int MaxPageSize => 500;
        /// <summary>
        /// Gets the default page size.
        /// </summary>
        public virtual int DefaultPageSize => 20;

        private int _pageSize;
        private int _pageNumber;
        private string _searchVal = string.Empty;
        private string _sortOrder = "DESC";
        private string _sortBy = string.Empty;

        /// <summary>
        /// Gets or sets the page size.
        /// </summary>
        public virtual int PageSize
        {
            get => _pageSize;
            set => _pageSize = value < 1 ? DefaultPageSize : Math.Min(Math.Max(value, 1), MaxPageSize);
        }

        /// <summary>
        /// Gets or sets the page number.
        /// </summary>
        public virtual int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = Math.Max(value, 1);
        }

        /// <summary>
        /// Gets or sets the search val.
        /// </summary>
        public virtual string SearchVal
        {
            get => _searchVal;
            set => _searchVal = string.IsNullOrWhiteSpace(value) ? string.Empty : value;
        }

        /// <summary>
        /// Gets or sets the sort by.
        /// </summary>
        public virtual string SortBy
        {
            get => _sortBy;
            set => _sortBy = string.IsNullOrWhiteSpace(value) ? string.Empty : value;
        }

        /// <summary>
        /// Gets or sets the sort order.
        /// </summary>
        public virtual string SortOrder
        {
            get => _sortOrder;
            set => _sortOrder = value is not null && value.Equals("DESC", StringComparison.InvariantCultureIgnoreCase)
                ? "DESC"
                : "ASC";
        }
    }
}
