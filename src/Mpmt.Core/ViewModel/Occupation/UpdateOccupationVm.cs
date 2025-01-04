namespace Mpmt.Core.ViewModel.Occupation
{
    /// <summary>
    /// The update occupation vm.
    /// </summary>
    public class UpdateOccupationVm
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the occupation name.
        /// </summary>
        public string OccupationName { get; set; }
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
