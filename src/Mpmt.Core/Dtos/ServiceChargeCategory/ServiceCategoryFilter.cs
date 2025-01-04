namespace Mpmt.Core.Dtos.ServiceChargeCategory
{
    /// <summary>
    /// The service category filter.
    /// </summary>
    public class ServiceCategoryFilter
    {
        /// <summary>
        /// Gets or sets the category name.
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// Gets or sets the category code.
        /// </summary>
        public string CategoryCode { get; set; }
        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public int Status { get; set; }
    }
}
