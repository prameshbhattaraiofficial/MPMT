namespace Mpmt.Core.Dtos.Paging
{
    /// <summary>
    /// The paged list.
    /// </summary>
    public class PagedList<T> : PageInfo
    {
        private IEnumerable<T> _items = new List<T>();

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        public IEnumerable<T> Items
        {
            get => _items;
            set => _items = value ?? new List<T>();
        }
    }
}
