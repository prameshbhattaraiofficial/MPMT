namespace Mpmt.Core.Domain.Payout
{
    public static class CreditStatusCodes
    {
        /// <summary>
        /// Credit success for amount
        /// </summary>
        public const string Success = "000";

        /// <summary>
        /// Amount deferal for cashout, but not received yet by benificiary
        /// </summary>
        public const string Defer = "DEFER";

        /// <summary>
        /// Amount credited for cashout and successfully received by benificiary
        /// </summary>
        public const string CashoutCompleted = "999";

        /// <summary>
        /// Credit not initiated
        /// </summary>
        public const string NotInitiated = "901";

        /// <summary>
        /// Credit pending due to manual review/inspection process
        /// </summary>
        public const string Pending = "902";

        /// <summary>
        /// Failed to credit to destination/benificiary account due to invalid account or any other reasons in credit process.
        /// </summary>
        public const string Failed = "903";

        /// <summary>
        /// Credit cancelled
        /// </summary>
        public const string Cancelled = "904";
    }
}
