namespace Mpmt.Core.ViewModel.Module
{
    /// <summary>
    /// The module with child.
    /// </summary>
    public class ModuleWithChild
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the module.
        /// </summary>
        public string Module { get; set; }
        /// <summary>
        /// Gets or sets the parent id.
        /// </summary>
        public int ParentId { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether is active.
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Gets or sets the displayer order.
        /// </summary>
        public int DisplayOrder { get; set; }
        /// <summary>
        /// Gets or sets the child.
        /// </summary>
        public List<ModuleWithChild> child { get; set; }
    }
}
