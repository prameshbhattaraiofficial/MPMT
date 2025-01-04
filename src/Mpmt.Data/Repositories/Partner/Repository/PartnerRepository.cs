using AutoMapper;
using Dapper;
using DocumentFormat.OpenXml.EMMA;
using Microsoft.AspNetCore.Http;
using Mpmt.Core.Dtos.Adjustment;
using Mpmt.Core.Dtos.FeeFundRequest;
using Mpmt.Core.Dtos.Paging;
using Mpmt.Core.Dtos.Partner;
using Mpmt.Core.Dtos.Roles;
using Mpmt.Core.Dtos.Users;
using Mpmt.Core.ViewModel.ForgotPassword;
using Mpmt.Data.Common;
using Mpmts.Core.Dtos;
using System.Data;
using System.Security.Claims;

namespace Mpmt.Data.Repositories.Partner;

public class PartnerRepository : IPartnerRepository
{
    private readonly IMapper _mapper;
    private readonly ClaimsPrincipal _loggedInUser;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PartnerRepository(IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _loggedInUser = _httpContextAccessor.HttpContext?.User;
    }

    public async Task<IEnumerable<FeeAccount>> GetFeeAccountAsync(string partnerCode, string sourceCurrency)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();
        var param = new DynamicParameters();
        param.Add("@PartnerCode", partnerCode);
        param.Add("@SourceCurrency", sourceCurrency);
        var data = await connection
            .QueryAsync<FeeAccount>("[dbo].[usp_get_feeaccount]", param: param, commandType: CommandType.StoredProcedure);
        return data;
    }

    public async Task<bool> VerifyUserNameAsync(string userName)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();
        var param = new DynamicParameters();
        param.Add("@UserName", userName);
        param.Add("@Check", dbType: DbType.Boolean, direction: ParameterDirection.Output);

        var _ = await connection.ExecuteAsync("[dbo].[usp_Check_userName_validation]", param, commandType: CommandType.StoredProcedure);

        var CheckResult = param.Get<bool>("@Check");
        return CheckResult;
    }

    public async Task<bool> VerifyEmailAsync(string Email)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();
        var param = new DynamicParameters();
        param.Add("@Email", Email);
        param.Add("@Check", dbType: DbType.Boolean, direction: ParameterDirection.Output);

        var _ = await connection.ExecuteAsync("[dbo].[usp_Check_Email_validation]", param, commandType: CommandType.StoredProcedure);

        var CheckResult = param.Get<bool>("@Check");
        return CheckResult;
    }

    public async Task<bool> VerifyShortnameAsync(string Shortname)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();
        var param = new DynamicParameters();
        param.Add("@Shortname", Shortname);
        param.Add("@Check", dbType: DbType.Boolean, direction: ParameterDirection.Output);

        var _ = await connection.ExecuteAsync("[dbo].[usp_Check_Shortname_validation]", param, commandType: CommandType.StoredProcedure);

        var CheckResult = param.Get<bool>("@Check");
        return CheckResult;
    }

    public async Task<bool> VerifyEmailConfirmed(string Email)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();
        var param = new DynamicParameters();
        param.Add("@Email", Email);
        param.Add("@Check", dbType: DbType.Boolean, direction: ParameterDirection.Output);

        var _ = await connection.ExecuteAsync("[dbo].[usp_check_remitpartner_register_approved]", param, commandType: CommandType.StoredProcedure);

        var CheckResult = param.Get<bool>("@Check");
        return CheckResult;
    }

    public async Task<SprocMessage> AddPartnerAsync(AppPartner user)
    {
        var dataTableRmp = DirectorTable();
        if (user.Directors != null)
        {
            foreach (var Director in user.Directors)
            {
                var row = dataTableRmp.NewRow();
                row["FirstName"] = Director.FirstName;
                row["ContactNumber"] = Director.ContactNumber;
                row["Email"] = Director.Email;

                dataTableRmp.Rows.Add(row);
            }
        }
        var ImagedataTable = ImageTable();
        if (user.LicensedocImgPath != null & user.LicensedocImgPath.Count > 0)
        {
            foreach (var path in user.LicensedocImgPath)
            {
                var row = ImagedataTable.NewRow();
                row["DocumentImgPath"] = path;
                ImagedataTable.Rows.Add(row);
            }
        }

        using var connection = DbConnectionManager.GetDefaultConnection();

        var param = new DynamicParameters();
        param.Add("@Event", user.Event);
        param.Add("@Id", user.Id);
        param.Add("@PartnerCode", user.PartnerCode);
        param.Add("@FirstName", user.FirstName);
        param.Add("@ShortName", user.ShortName);
        param.Add("@SurName", user.SurName);
        param.Add("@IsFirstNamePresent", user.IsFirstNamePresent);
        param.Add("@MobileNumber", user.MobileNumber);
        param.Add("@MobileConfirmed", user.MobileConfirmed);
        param.Add("@Email", user.Email);
        param.Add("@Directors", dataTableRmp.AsTableValuedParameter("[dbo].[PartnersDirectorsType]"));
        param.Add("@DocumentImgPaths", ImagedataTable.AsTableValuedParameter("[dbo].[DocumentImgs]"));
        param.Add("@EmailConfirmed", user.EmailConfirmed);
        param.Add("@Post", user.Post);
        param.Add("@GenderId", user.GenderId);
        param.Add("@Address", user.Address);
        param.Add("@UserName", user.UserName);
        param.Add("@Remarks", user.Remarks);
        param.Add("@PasswordHash", user.PasswordHash);
        param.Add("@PasswordSalt", user.PasswordSalt);
        param.Add("@AccessCodeHash", user.AccessCodeHash);
        param.Add("@AccessCodeSalt", user.AccessCodeSalt);
        param.Add("@MPINHash", user.MPINHash);
        param.Add("@MPINSalt", user.MPINSalt);
        param.Add("@ChargeCategoryId", user.ChargeCategoryId);
        param.Add("@FundType", user.FundTypeId);
        param.Add("@CreditUptoLimitPerc", user.CreditUptoLimitPerc);
        param.Add("@CreditSendTxnLimit", user.CreditSendTxnLimit);
        param.Add("@CashPayoutSendTxnLimit", user.CashPayoutSendTxnLimit);
        param.Add("@WalletSendTxnLimit", user.WalletSendTxnLimit);
        param.Add("@BankSendTxnLimit", user.BankSendTxnLimit);
        param.Add("@NotificationBalanceLimit", user.NotificationBalanceLimit);
        param.Add("@TransactionApproval", user.TransactionApproval);
        param.Add("@OrganizationName", user.OrganizationName);
        param.Add("@OrgEmail", user.OrgEmail);
        param.Add("@OrgEmailConfirmed", user.OrgEmailConfirmed);
        param.Add("@CountryCode", user.CountryCode);
        param.Add("@City", user.City);
        param.Add("@FullAddress", user.FullAddress);
        param.Add("@GMTTimeZone", user.GMTTimeZone);
        param.Add("@GMTTimeZoneId", user.GMTTimeZoneId);
        param.Add("@RegistrationNumber", user.RegistrationNumber);
        param.Add("@SourceCurrency", user.SourceCurrency);
        param.Add("@Website", user.Website);
        param.Add("@IpAddress", user.IpAddress);
        param.Add("@CompanyLogoImgPath", user.CompanyLogoImgPath);
        //param.Add("@LicensedocImgPath", user.LicensedocImgPath);
        param.Add("@DocumentTypeId", user.DocumentTypeId);
        param.Add("@DocumentNumber", user.DocumentNumber);
        param.Add("@IdFrontImgPath", user.IdFrontImgPath);
        param.Add("@IdBackImgPath", user.IdBackImgPath);
        param.Add("@ExpiryDate", user.ExpiryDate);
        param.Add("@IssueDate", user.IssueDate);
        param.Add("@BusinessNumber", user.BusinessNumber);
        param.Add("@FinancialTransactionRegNo", user.FinancialTransactionRegNo);
        param.Add("@RemittancRegNumber", user.RemittancRegNumber);
        param.Add("@LicenseNumber", user.LicenseNumber);
        param.Add("@ZipCode", user.ZipCode);
        param.Add("@OrgState", user.OrgState);
        param.Add("@IsFirstNamePresent", user.IsFirstNamePresent);
        param.Add("@AddressProofTypeId", user.AddressProofTypeId);
        param.Add("@AddressProofImgPath", user.AddressProofImgPath);
        param.Add("@LastIpAddress", user.LastIpAddress);
        param.Add("@DeviceId", user.DeviceId);
        param.Add("@IsActive", user.IsActive);
        param.Add("@IsDeleted", user.IsDeleted);
        param.Add("@IsBlocked", user.IsBlocked);
        param.Add("@FailedLoginAttempt", user.FailedLoginAttempt);
        param.Add("@TemporaryLockedTillUtcDate", user.TemporaryLockedTillUtcDate);
        param.Add("@LastLoginDateUtc", user.LastLoginDateUtc);
        param.Add("@LastActivityDateUtc", user.LastActivityDateUtc);
        param.Add("@KycStatusCode", user.KycStatusCode);
        param.Add("@Is2FAAuthenticated", user.Is2FAAuthenticated);
        param.Add("@AccountSecretKey", user.AccountSecretKey);
        //param.Add("@PartnerTimeZone", user.PartnerTimeZone);
        param.Add("@Maker", user.Maker);
        param.Add("@Checker", user.Checker);
        param.Add("@CreatedById", user.CreatedById);
        param.Add("@CreatedByName", user.CreatedByName);
        param.Add("@CreatedDate", user.CreatedDate);
        param.Add("@UpdatedById", user.UpdatedById);
        param.Add("@UpdatedByName", user.UpdatedByName);
        param.Add("@UpdatedDate", user.UpdatedDate);

        param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
        param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
        param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
        param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

        try
        {
            var _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_remit_partners]", param, commandType: CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            return new SprocMessage { IdentityVal = 0, StatusCode = 400, MsgType = "Error", MsgText = "Issue Inserting Data" };
        }

        var identityVal = param.Get<int>("@IdentityVal");
        var statusCode = param.Get<int>("@StatusCode");
        var msgType = param.Get<string>("@MsgType");
        var msgText = param.Get<string>("@MsgText");

        return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
    }

    public async Task<SprocMessage> PartnerChangePasswordAsync(AppPartner user)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();

        var param = new DynamicParameters();
        param.Add("@Event", user.Event);
        param.Add("@Id", user.Id);
        param.Add("@PartnerCode", user.PartnerCode);
        param.Add("@UserName", user.UserName);
        param.Add("@PasswordHash", user.PasswordHash);
        param.Add("@PasswordSalt", user.PasswordSalt);
        param.Add("@AccessCodeHash", user.AccessCodeHash);
        param.Add("@AccessCodeSalt", user.AccessCodeSalt);
        param.Add("@MPINHash", user.MPINHash);
        param.Add("@MPINSalt", user.MPINSalt);
        param.Add("@UpdatedById", user.UpdatedById);
        param.Add("@UpdatedByName", user.UpdatedByName);

        param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
        param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
        param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
        param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

        try
        {
            var _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_remit_partners_changePassword]", param, commandType: CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            return new SprocMessage { IdentityVal = 0, StatusCode = 400, MsgType = "Error", MsgText = "Issue Inserting Data" };
        }

        var identityVal = param.Get<int>("@IdentityVal");
        var statusCode = param.Get<int>("@StatusCode");
        var msgType = param.Get<string>("@MsgType");
        var msgText = param.Get<string>("@MsgText");

        return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
    }

    private DataTable DirectorTable()
    {
        var dataTableRmp = new DataTable();
        dataTableRmp.Columns.Add("FirstName", typeof(string));
        dataTableRmp.Columns.Add("ContactNumber", typeof(string));
        dataTableRmp.Columns.Add("Email", typeof(string));

        return dataTableRmp;
    }

    private DataTable ImageTable()
    {
        var dataTableRmp = new DataTable();
        dataTableRmp.Columns.Add("DocumentImgPath", typeof(string));

        return dataTableRmp;
    }

    public async Task<bool> CheckPartnerExistsByEmailAsync(string email)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();

        var param = new DynamicParameters();
        param.Add("@Email", email);

        return await connection
            .QueryFirstAsync<bool>("[dbo].[usp_check_Partner_exists_by_email]", param: param, commandType: CommandType.StoredProcedure);
    }

    public async Task<bool> CheckPartnerExistsByUserNameAsync(string userName)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();

        var param = new DynamicParameters();
        param.Add("@UserName", userName);

        return await connection
            .QueryFirstAsync<bool>("[dbo].[usp_check_partner_exists_by_username]", param: param, commandType: CommandType.StoredProcedure);
    }

    public async Task<AppPartner> GetPartnerByEmailAsync(string email)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();

        var param = new DynamicParameters();
        param.Add("@Email", email);

        return await connection
            .QueryFirstOrDefaultAsync<AppPartner>("[dbo].[usp_get_partner_by_email]", param, commandType: CommandType.StoredProcedure);
    }

    public async Task<(SprocMessage, PasswordResetModel)> ResetTokenValidationAsync(PasswordResetModel resetModel)
    {
        try
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@ResetToken", resetModel.ResetToken);
            param.Add("@ResetRequestToken", resetModel.ResetRequestToken);

            var data = await connection
            .QueryMultipleAsync("[dbo].[usp_reset_token_validation]", param: param, commandType: CommandType.StoredProcedure);
            var userData = await data.ReadFirstOrDefaultAsync<PasswordResetModel>();
            //var sprocMessage = await data.ReadSingleOrDefaultAsync<SprocMessage>();
            var sprocMessage = await data.ReadFirstOrDefaultAsync<SprocMessage>();
            return (sprocMessage, userData);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<(SprocMessage, PasswordResetModel)> RequestTokenValidationAsync(string token)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();
        var param = new DynamicParameters();
        param.Add("@ResetRequestToken", token);
        var data = await connection
            .QueryMultipleAsync("[dbo].[usp_request_token_validation]", param: param, commandType: CommandType.StoredProcedure);
        var userData = await data.ReadFirstOrDefaultAsync<PasswordResetModel>();
        //var sprocMessage = await data.ReadSingleOrDefaultAsync<SprocMessage>();
        var sprocMessage = await data.ReadSingleOrDefaultAsync<SprocMessage>();
        return (sprocMessage, userData);
    }

    public async Task<SprocMessage> ForgotPasswordAsync(PasswordResetModel model)
    {
        try
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@UserId", model.UserId);
            param.Add("@UserType", model.UserType);
            param.Add("@ResetRequestToken", model.ResetRequestToken);
            param.Add("@ResetToken", model.ResetToken);

            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[usp_forgot_password_request]", param: param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<AppPartner> GetPartnerByIdAsync(int id)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();

        var param = new DynamicParameters();
        param.Add("@Id", id);
        var response = new AppPartner();
        try
        {
            var partnerdetail = await connection
               .QueryMultipleAsync("[dbo].[usp_get_remit_partner_ById]", param, commandType: CommandType.StoredProcedure);

            response = await partnerdetail.ReadFirstAsync<AppPartner>();
            var images = await partnerdetail.ReadAsync<string>();
            var directors = await partnerdetail.ReadAsync<Director>();
            var roles = await partnerdetail.ReadAsync<PartnerRoleDetail>();
            response.LicensedocImgPath = images.ToList();
            response.Directors = directors.ToList();
            response.PartnerRoles = roles.ToList();
        }
        catch (Exception ex)
        {
            var res = new Exception(ex.Message);
        }
        return response;
    }

    public async Task<AppPartner> GetPartnerByUserNameAsync(string userName)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();

        var param = new DynamicParameters();
        param.Add("@UserName", userName);

        return await connection
            .QueryFirstOrDefaultAsync<AppPartner>("[dbo].[usp_get_partner_by_username]", param, commandType: CommandType.StoredProcedure);
    }

    public async Task UpdatePartnerLoginActivityAsync(UserLoginActivity loginActivity)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();

        var param = new DynamicParameters();
        param.Add("@UserId", loginActivity.UserId);
        param.Add("@FailedLoginAttempt", loginActivity.FailedLoginAttempt);
        param.Add("@TemporaryLockedTillUtcDate", loginActivity.TemporaryLockedTillUtcDate);
        param.Add("@LastIpAddress", loginActivity.LastIpAddress);
        param.Add("@DeviceId", loginActivity.DeviceId);
        param.Add("@IsActive", loginActivity.IsActive);
        param.Add("@IsBlocked", loginActivity.IsBlocked);
        param.Add("@LastLoginDateUtc", loginActivity.LastLoginDateUtc);
        param.Add("@LastActivityDateUtc", loginActivity.LastActivityDateUtc);

        _ = await connection.ExecuteAsync("[dbo].[usp_update_Partner_login_activity]", param, commandType: CommandType.StoredProcedure);
    }

    public async Task<SprocMessage> UpdateStatusAsync(PartnerUpdateStatus user)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();

        var param = new DynamicParameters();
        param.Add("@PartnerCode", user.PartnerCode);
        param.Add("@IsActive", user.IsActive);
        param.Add("@LoggedInUser", user.LoggedInUser);
        param.Add("@userType", user.userType);
        param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
        param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
        param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
        param.Add("@MsgType", dbType: DbType.String, size: 50, direction: ParameterDirection.Output);

        _ = await connection.ExecuteAsync("[dbo].[usp_Update_RemitPartnerStatus]", param, commandType: CommandType.StoredProcedure);

        var identityVal = param.Get<int>("@IdentityVal");
        var statusCode = param.Get<int>("@StatusCode");
        var msgType = param.Get<string>("@MsgType");
        var msgText = param.Get<string>("@MsgText");

        return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
    }

    public async void UpdateAccountSecretKey(string email, string accountsecretkey)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();

        var param = new DynamicParameters();
        param.Add("@Email", email);
        param.Add("@AccountSecretKey", accountsecretkey);
        param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
        param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
        param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
        param.Add("@MsgType", dbType: DbType.String, size: 50, direction: ParameterDirection.Output);
        var result = await connection.ExecuteAsync("[dbo].[usp_Update_AccountSecretKey_partner_By_Email]", param, commandType: CommandType.StoredProcedure);
    }

    public async void UpdateIs2FAAuthenticated(string email, bool Is2FAAuthenticated)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();

        var param = new DynamicParameters();
        param.Add("@Email", email);
        param.Add("@Is2FAAuthenticated", Is2FAAuthenticated);
        param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
        param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
        param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
        param.Add("@MsgType", dbType: DbType.String, size: 50, direction: ParameterDirection.Output);
        var result = await connection.ExecuteAsync("[dbo].[usp_Is2FAAuthenticated_partner_By_Email]", param, commandType: CommandType.StoredProcedure);
    }

    public async Task<PagedList<PartnerDetails>> GetPartnerList(PartnerFilter partnerFilter)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();

        var param = new DynamicParameters();
        param.Add("@PartnerCode", partnerFilter.PartnerCode);
        param.Add("@Name", partnerFilter.Name);
        param.Add("@Email", partnerFilter.Email);
        param.Add("@StartDate", partnerFilter.StartDate);
        param.Add("@EndDate", partnerFilter.EndDate);
        param.Add("@PageNumber", partnerFilter.PageNumber);
        param.Add("@PageSize", partnerFilter.PageSize);
        param.Add("@SortingCol", partnerFilter.SortBy);
        param.Add("@SortType", partnerFilter.SortOrder);
        param.Add("@SearchVal", partnerFilter.SearchVal);
        var data = await connection
            .QueryMultipleAsync("[dbo].[usp_get_remit_partners]", param: param, commandType: CommandType.StoredProcedure);

        var List = await data.ReadAsync<PartnerDetails>();
        var pagedInfo = await data.ReadFirstAsync<PageInfo>();
        var mappeddata = _mapper.Map<PagedList<PartnerDetails>>(pagedInfo);
        mappeddata.Items = List;
        return mappeddata;
    }

    public async Task<IEnumerable<PartnerDirectors>> GetPartnerdirectors(string PartnerCode)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();

        var param = new DynamicParameters();

        param.Add("@PartnerCode", PartnerCode);

        var data = await connection
            .QueryAsync<PartnerDirectors>("[dbo].[usp_get_partner_directors]", param: param, commandType: CommandType.StoredProcedure);

        return data;
    }

    public async Task<IEnumerable<AppPartner>> GetPartnerRole()

    {
        using var connection = DbConnectionManager.GetDefaultConnection();

        var param = new DynamicParameters();

        var email = _loggedInUser.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
        param.Add("@Email", email);

        return await connection
            .QueryAsync<AppPartner>("[dbo].[usp_get_partner_by_id]", param, commandType: CommandType.StoredProcedure);
    }

    public async Task<PartnerWithCredentials> GetPartnerWithCredentialsByApiUserNameAsync(string apiUserName)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();

        var param = new DynamicParameters();
        param.Add("@ApiUserName", apiUserName);

        return await connection.QueryFirstOrDefaultAsync<PartnerWithCredentials>(
            "[dbo].[usp_get_remit_partner_credentials_byapiusername]", param, commandType: CommandType.StoredProcedure);
    }

    public async Task<SprocMessage> AddLoginOtpAsync(TokenVerification tokenVerification)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();

        var param = new DynamicParameters();
        param.Add("@UserId", tokenVerification.UserId);
        param.Add("@PartnerCode", tokenVerification.PartnerCode);
        param.Add("@Username", tokenVerification.UserName);
        param.Add("@Email", tokenVerification.Email);
        param.Add("@CountryCode", tokenVerification.CountryCallingCode);
        param.Add("@Mobile", tokenVerification.Mobile);
        param.Add("@VerificationCode", tokenVerification.VerificationCode);
        param.Add("@VerificationType", tokenVerification.VerificationType);
        param.Add("@SendToMobile", tokenVerification.SendToMobile);
        param.Add("@SendToEmail", tokenVerification.SendToEmail);
        param.Add("@ExpiryDate", tokenVerification.ExpiredDate);
        param.Add("@IsConsumed", tokenVerification.IsConsumed);
        param.Add("@OtpVerificationFor", tokenVerification.OtpVerificationFor);

        param.Add("@ReturnPrimaryId", dbType: DbType.Int32, direction: ParameterDirection.Output);
        param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
        param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
        param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

        try
        {
            var _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_Token_Verification]", param, commandType: CommandType.StoredProcedure);
        }
        catch (Exception)
        {
            return new SprocMessage { IdentityVal = 0, StatusCode = 400, MsgType = "Error", MsgText = "Issue Inserting Data" };
        }

        var identityVal = param.Get<int>("@ReturnPrimaryId");
        var statusCode = param.Get<int>("@StatusCode");
        var msgType = param.Get<string>("@MsgType");
        var msgText = param.Get<string>("@MsgText");

        return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
    }

    public async Task<TokenVerification> GetOtpBypartnerCodeAsync(string partnercode, string UserName, string OtpVerificationFor)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();

        var param = new DynamicParameters();

        param.Add("@PartnerCode", partnercode);
        param.Add("@UserName", UserName);
        param.Add("@OtpVerificationFor", OtpVerificationFor);

        var data = await connection
            .QueryFirstOrDefaultAsync<TokenVerification>("[dbo].[usp_getverificationtoken_bypartnercode_Username]", param: param, commandType: CommandType.StoredProcedure);

        return data;
    }

    public async void UpdateEmailConfirmAsync(string partnercode)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();

        var param = new DynamicParameters();

        param.Add("@PartnerCode", partnercode);

        await connection
            .ExecuteAsync("[dbo].[UpdateEmailConfirm_bypartnercode]", param: param, commandType: CommandType.StoredProcedure);
    }

    public async Task<AppPartner> GetPartnerEmployeeByEmailAsync(string email)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();

        var param = new DynamicParameters();
        param.Add("@Email", email);
        var response = new AppPartner();
        try
        {
            var partnerdetail = await connection
               .QueryMultipleAsync("[dbo].[usp_get_remit_partner_employee_ByEmail]", param, commandType: CommandType.StoredProcedure);

            response = await partnerdetail.ReadFirstAsync<AppPartner>();
            var images = await partnerdetail.ReadAsync<string>();
            var directors = await partnerdetail.ReadAsync<Director>();
            var partnerRoles = await partnerdetail.ReadAsync<PartnerRoleDetail>();
            response.LicensedocImgPath = images.ToList();
            response.PartnerRoles = partnerRoles.ToList();
            response.Directors = directors.ToList();
        }
        catch (Exception ex)
        {
            var res = new Exception(ex.Message);
        }
        return response;
    }

    public async Task<SprocMessage> PartnerWalletAdjustment(AdjustmentWalletDTO adjustment)
    {
        try
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@PartnerCode", adjustment.PartnerCode);
            param.Add("@WalletCurrency", adjustment.WalletCurrency);
            param.Add("@Amount", adjustment.Amount);
            param.Add("@Type", adjustment.Type);
            param.Add("@Remarks", adjustment.Remarks);

            param.Add("@OperationMode", adjustment.OperationMode);
            param.Add("@LoggedInUser", adjustment.LoggedInUser);
            param.Add("@UserType", adjustment.UserType);

            param.Add("@ReturnPrimaryId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[usp_balance_adjustment]", param: param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@ReturnPrimaryId");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }
        catch (Exception)
        {
            throw;
        }
    }
}