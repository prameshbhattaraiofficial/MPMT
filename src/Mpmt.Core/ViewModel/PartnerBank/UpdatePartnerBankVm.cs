namespace Mpmt.Core.ViewModel.PartnerBank
{
    /// <summary>
    /// The update partner bank vm.
    /// </summary>
    public class UpdatePartnerBankVm
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }
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
