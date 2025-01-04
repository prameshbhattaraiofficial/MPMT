namespace Mpmt.Core.Domain.Payout
{
    public static class PayoutTypes
    {
        /// <summary>
        /// Payout to Bank
        /// </summary>
        public const string Bank = "BANK";
        
        /// <summary>
        /// Payout to E-Wallet
        /// </summary>
        public const string Wallet = "WALLET";

        /// <summary>
        /// Payout via Cash
        /// </summary>
        public const string Cash = "CASH";
    }
}
