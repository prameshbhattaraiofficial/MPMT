using Mpmt.Core.Dtos;
using Mpmt.Core.Dtos.Adjustment;
using Mpmt.Core.Dtos.FeeFundRequest;
using Mpmt.Core.Dtos.Paging;
using Mpmt.Core.Dtos.Partner;
using Mpmt.Core.Dtos.Users;
using Mpmt.Core.ViewModel.ForgotPassword;
using Mpmts.Core.Dtos;
using System.Security.Claims;

namespace Mpmt.Services.Partner.IService;

public interface IPartnerService
{
    Task<MpmtResult> AddPartnerAsync(AppPartner user);
    Task<bool> CheckPartnerExistsByEmailAsync(string email);
    Task<bool> CheckPartnerExistsByUserNameAsync(string userName);
    Task<AppPartner> GetPartnerByEmailAsync(string email);
    Task<AppPartner> GetPartnerByIdAsync(int id);
    Task<AppPartner> GetPartnerEmployeeByEmailAsync(string email);
    Task<AppPartner> GetPartnerByUserNameAsync(string userName);
    Task UpdatePartnerLoginActivityAsync(UserLoginActivity loginActivity);
    void UpdateAccountSecretKey(string email, string accountsecretkey);
    Task<PagedList<PartnerDetails>> GetPartnerListAsync(PartnerFilter partnerFilter);
    Task SendPasswordResetList(string link, string emails);
    Task<SprocMessage> ForgotPasswordAsync(PasswordResetModel reset);
    Task<(SprocMessage, PasswordResetModel)> ResetTokenValidationAsync(PasswordResetModel resetModel);
    Task<SprocMessage> ResetPasswordAsync(PasswordResetModel request);
    Task<(SprocMessage, PasswordResetModel)> RequestTokenValidationAsync(string token);
    Task<IEnumerable<PartnerDirectors>> GetPartnerdirectorsListAsync(string PartnerCode);
    Task<MpmtResult> AddPartnerAsync(AddPatnerRequest addPartnerBank);
    Task<SprocMessage> UpdateRemitPartnerStatusAsync(bool action, int Id);
    Task<MpmtResult> UpdatePartnerAsync(UpdatePartnerrequest user);
    Task<SprocMessage> DeletePartnerAsync(AppPartner addPartnerBank);
    Task<bool> VerifyUserName(string userName);
    Task<bool> VerifyEmail(string Email);
    Task<bool> VerifyShortname(string Shortname);
    Task<bool> VerifyEmailConfirmed(string Email);
    Task<IEnumerable<FeeAccount>> GetFeeAccountAsync(string partnerCode, string sourceCurrency);
    Task<SprocMessage> AddLoginOtpAsync(TokenVerification tokenVerification);
    Task<TokenVerification> GetOtpBypartnerCodeAsync(string partnercode, string UserName, string OtpVerificationFor);
    void UpdateEmailConfirmAsync(string partnercode);
    Task<SprocMessage> PartnerWalletAdjustment(AdjustmentWallet adjustment, ClaimsPrincipal claim);       
}
