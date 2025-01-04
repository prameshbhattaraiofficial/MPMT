namespace Mpmt.Core.ViewModel.Bank
{
    /// <summary>
    /// The add bank vm.
    /// </summary>
    public class AddBankVm
    {
        /// <summary>
        /// Gets or sets the bank name.
        /// </summary>
        public string BankName { get; set; }
        /// <summary>
        /// Gets or sets the bank code.
        /// </summary>
        public string BankCode { get; set; }
        /// <summary>
        /// Gets or sets the branch code.
        /// </summary>
        public string BranchCode { get; set; }
        /// <summary>
        /// Gets or sets the country code.
        /// </summary>
        public string CountryCode { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether is active.
        /// </summary>
        public bool IsActive { get; set; }
    }
}
