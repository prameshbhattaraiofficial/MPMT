namespace Mpmt.Core.ViewModel.FundType
{
    /// <summary>
    /// The update fund type vm.
    /// </summary>
    public class UpdateFundTypeVm
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the fund type.
        /// </summary>
        public string FundType { get; set; }
        /// <summary>
        /// Gets or sets the fund type code.
        /// </summary>
        public string FundTypeCode { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether is active.
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Gets or sets the remarks.
        /// </summary>
        public string Remarks { get; set; }
    }
}
