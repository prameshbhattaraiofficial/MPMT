namespace Mpmt.Core.ViewModel.ServiceChargeCategory
{
    /// <summary>
    /// The add service category vm.
    /// </summary>
    public class AddServiceCategoryVm
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
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether is active.
        /// </summary>
        public bool IsActive { get; set; }
    }
}
