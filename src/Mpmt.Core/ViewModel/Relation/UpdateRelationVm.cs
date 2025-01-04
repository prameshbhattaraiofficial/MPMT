namespace Mpmt.Core.ViewModel.Relation
{
    /// <summary>
    /// The update relation vm.
    /// </summary>
    public class UpdateRelationVm
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the relation name.
        /// </summary>
        public string RelationName { get; set; }
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
