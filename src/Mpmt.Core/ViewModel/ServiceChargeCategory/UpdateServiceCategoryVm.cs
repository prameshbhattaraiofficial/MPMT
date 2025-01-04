namespace Mpmt.Core.ViewModel.ServiceChargeCategory
{
    /// <summary>
    /// The update service category vm.
    /// </summary>
    public class UpdateServiceCategoryVm
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
    }
}
