namespace Mpmt.Core.Dtos.ServiceChargeCategory
{
    /// <summary>
    /// The service category details.
    /// </summary>
    public class ServiceCategoryDetails
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the category name.
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// Gets or sets the category code.
        /// </summary>
        public string CategoryCode { get; set; }
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether is active.
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Gets or sets the created by name.
        /// </summary>
        public string CreatedByName { get; set; }
        /// <summary>
        /// Gets or sets the updated by name.
        /// </summary>
        public string UpdatedByName { get; set; }
    }
}
