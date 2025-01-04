namespace Mpmt.Core.Dtos.Menu
{
    /// <summary>
    /// The i u d menu.
    /// </summary>
    public class IUDMenu
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
        public int ParentId { get; set; } = 0;
        /// <summary>
        /// Gets or sets the menu url.
        /// </summary>
        public string MenuUrl { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether is active.
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Gets or sets the display order.
        /// </summary>
        public int DisplayOrder { get; set; }
        /// <summary>
        /// Gets or sets the image path.
        /// </summary>
        public string ImagePath { get; set; }
        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// Gets or sets the created nepali date.
        /// </summary>
        public string CreatedNepaliDate { get; set; }
        /// <summary>
        /// Gets or sets the updated by.
        /// </summary>
        public string UpdatedBy { get; set; }
        /// <summary>
        /// Gets or sets the updated nepali date.
        /// </summary>
        public string UpdatedNepaliDate { get; set; }
    }
}
