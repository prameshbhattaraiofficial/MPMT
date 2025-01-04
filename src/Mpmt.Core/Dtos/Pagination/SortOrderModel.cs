namespace Mpmt.Core.Dtos.Pagination
{
    /// <summary>
    /// The sort order model.
    /// </summary>
    public class SortOrderModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SortOrderModel"/> class.
        /// </summary>
        /// <param name="sortOrderOptions">The sort order options.</param>
        /// <param name="sortOrder">The sort order.</param>
        public SortOrderModel(string[] sortOrderOptions, string sortOrder)
        {
            Options = sortOrderOptions ?? Array.Empty<string>();
            SortOrder = string.IsNullOrWhiteSpace(sortOrder) ? "ASC" : sortOrder;
        }

        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        public string[] Options { get; set; }
        /// <summary>
        /// Gets or sets the sort order.
        /// </summary>
        public string SortOrder { get; set; }
    }
}
