namespace Mpmt.Core.ViewModel.Menu
{
    /// <summary>
    /// The menuwithchield.
    /// </summary>
    public class Menuwithchield
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Gets or sets the parent id.
        /// </summary>
        public int ParentId { get; set; }
        /// <summary>
        /// Gets or sets the menu url.
        /// </summary>
        public string MenuUrl { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether is active.
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// Gets or sets the display order.
        /// </summary>
        public int DisplayOrder { get; set; }
        /// <summary>
        /// Gets or sets the image path.
        /// </summary>
        public string ImagePath { get; set; }
        /// <summary>
        /// Gets or sets the child.
        /// </summary>
        public List<Menuwithchield> child { get; set; }
    }
}
