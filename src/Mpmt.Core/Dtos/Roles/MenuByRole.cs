namespace Mpmt.Core.Dtos.Roles
{
    public class MenuByRole
    {
        /// <summary>
        /// Gets or sets the menu id.
        /// </summary>
        public int menuId { get; set; }
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// Gets or sets the parent id.
        /// </summary>
        public int parentId { get; set; }
        /// <summary>
        /// Gets or sets the menu url.
        /// </summary>
        public string menuUrl { get; set; }
        /// <summary>
        /// Gets or sets the display order.
        /// </summary>
        public int displayOrder { get; set; }
        /// <summary>
        /// Gets or sets the image path.
        /// </summary>
        public string imagePath { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether view per.
        /// </summary>
        public bool viewPer { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether create per.
        /// </summary>
        public bool createPer { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether update per.
        /// </summary>
        public bool updatePer { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether delete per.
        /// </summary>
        public bool deletePer { get; set; }
    }
}
