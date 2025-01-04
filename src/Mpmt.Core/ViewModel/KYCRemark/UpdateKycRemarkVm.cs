namespace Mpmt.Core.ViewModel.KYCRemark
{
    /// <summary>
    /// The update kyc remark vm.
    /// </summary>
    public class UpdateKycRemarkVm
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the remarks name.
        /// </summary>
        public string RemarksName { get; set; }
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
