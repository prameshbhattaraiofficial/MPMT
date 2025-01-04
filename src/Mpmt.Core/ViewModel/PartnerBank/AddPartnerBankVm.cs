namespace Mpmt.Core.ViewModel.PartnerBank
{
    /// <summary>
    /// The add partner bank vm.
    /// </summary>
    public class AddPartnerBankVm
    {
        /// <summary>
        /// Gets or sets the partner code.
        /// </summary>
        public string PartnerCode { get; set; }
        /// <summary>
        /// Gets or sets the bank code.
        /// </summary>
        public string BankCode { get; set; }
        /// <summary>
        /// Gets or sets the account number.
        /// </summary>
        public string AccountNumber { get; set; }
        /// <summary>
        /// Gets or sets the account name.
        /// </summary>
        public string AccountName { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether is active.
        /// </summary>
        public bool IsActive { get; set; }
    }
}
