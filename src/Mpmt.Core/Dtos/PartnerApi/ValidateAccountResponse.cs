using Mpmt.Core.Domain;

namespace Mpmt.Core.Dtos.PartnerApi
{
    public class ValidateAccountResponse : ApiResponse
    {
        private string _branchId;
        private string _matchPercentage;
        private string _walletAccountName;
        private string _walletAccountNumber;
        private string _walletAccountStatus;
        private bool _isWalletKycVerified;

        public string BranchId { get => _branchId ?? string.Empty; set => _branchId = value; }
        public string MatchPercentage { get => string.IsNullOrWhiteSpace(_matchPercentage) ? "0" : _matchPercentage; set => _matchPercentage = value; }

        // for wallet verification
        public string WalletAccountName { get => _walletAccountName ?? string.Empty; set => _walletAccountName = value; }
        public string WalletAccountNumber { get => _walletAccountNumber ?? string.Empty; set => _walletAccountNumber = value; }
        public string WalletAccountStatus { get => _walletAccountStatus ?? string.Empty; set => _walletAccountStatus = value; }
        public bool IsWalletKycVerified { get => _isWalletKycVerified; set => _isWalletKycVerified = value; }
    }
}
