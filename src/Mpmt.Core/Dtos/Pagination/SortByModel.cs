namespace Mpmt.Core.Dtos.Pagination
{
    /// <summary>
    /// The sort by model.
    /// </summary>
    public class SortByModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SortByModel"/> class.
        /// </summary>
        /// <param name="sortByNameValues">The sort by name values.</param>
        /// <param name="defaultSortByValue">The default sort by value.</param>
        public SortByModel(Dictionary<string, string> sortByNameValues, string defaultSortByValue)
        {
            NameValues = sortByNameValues ?? new();
            DefaultSortBy = defaultSortByValue ?? string.Empty;
        }

        /// <summary>
        /// Gets or sets the name values.
        /// </summary>
        public Dictionary<string, string> NameValues { get; set; }
        /// <summary>
        /// Gets or sets the default sort by.
        /// </summary>
        public string DefaultSortBy { get; set; }
    }
}
