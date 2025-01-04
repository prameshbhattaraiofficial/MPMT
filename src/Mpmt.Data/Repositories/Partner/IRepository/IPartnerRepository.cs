using Mpmt.Core.Dtos.Adjustment;
using Mpmt.Core.Dtos.FeeFundRequest;
using Mpmt.Core.Dtos.Paging;
using Mpmt.Core.Dtos.Partner;
using Mpmt.Core.Dtos.Users;
using Mpmt.Core.ViewModel.ForgotPassword;
using Mpmts.Core.Dtos;

namespace Mpmt.Data.Repositories.Partner;

public interface IPartnerRepository
{
    Task<SprocMessage> AddPartnerAsync(AppPartner user);
    Task<bool> VerifyUserNameAsync(string userName);
    Task<bool> VerifyEmailAsync(string Email);
    Task<bool> VerifyShortnameAsync(string Shortname);
    Task<bool> VerifyEmailConfirmed(string Email);
    Task<SprocMessage> UpdateStatusAsync(PartnerUpdateStatus user);
    Task<SprocMessage> ForgotPasswordAsync(PasswordResetModel model);
    Task<(SprocMessage, PasswordResetModel)> RequestTokenValidationAsync(string token);
    Task<(SprocMessage, PasswordResetModel)> ResetTokenValidationAsync(PasswordResetModel resetModel);
    Task<bool> CheckPartnerExistsByEmailAsync(string email);
    Task<bool> CheckPartnerExistsByUserNameAsync(string userName);
    Task<AppPartner> GetPartnerByEmailAsync(string email);
    Task<AppPartner> GetPartnerByIdAsync(int id);
    Task<AppPartner> GetPartnerEmployeeByEmailAsync(string email);
    Task<IEnumerable<FeeAccount>> GetFeeAccountAsync(string partnerCode, string sourceCurrency);
    Task<AppPartner> GetPartnerByUserNameAsync(string userName);
    Task<PartnerWithCredentials> GetPartnerWithCredentialsByApiUserNameAsync(string apiUserName);
    Task UpdatePartnerLoginActivityAsync(UserLoginActivity loginActivity);
    void UpdateAccountSecretKey(string email, string accountsecretkey);
    void UpdateIs2FAAuthenticated(string email, bool Is2FAAuthenticated);
    Task<PagedList<PartnerDetails>> GetPartnerList(PartnerFilter partnerFilter);
    Task<IEnumerable<PartnerDirectors>> GetPartnerdirectors(string PartnerCode);
    Task<IEnumerable<AppPartner>> GetPartnerRole();
    Task<SprocMessage> AddLoginOtpAsync(TokenVerification tokenVerification);
    Task<TokenVerification> GetOtpBypartnerCodeAsync(string partnercode, string UserName, string OtpVerificationFor);
    void UpdateEmailConfirmAsync(string partnercode);
    Task<SprocMessage> PartnerChangePasswordAsync(AppPartner user);
    Task<SprocMessage> PartnerWalletAdjustment(AdjustmentWalletDTO adjustment);
}
