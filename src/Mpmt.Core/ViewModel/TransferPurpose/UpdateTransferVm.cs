namespace Mpmt.Core.ViewModel.TransferPurpose
{
    /// <summary>
    /// The update transfer vm.
    /// </summary>
    public class UpdateTransferVm
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the purpose name.
        /// </summary>
        public string PurposeName { get; set; }
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
