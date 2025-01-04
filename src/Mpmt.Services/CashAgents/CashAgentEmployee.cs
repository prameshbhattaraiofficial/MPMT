using AutoMapper;
using Dapper;
using DocumentFormat.OpenXml.EMMA;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Mpmt.Core.Common;
using Mpmt.Core.Common.Helpers;
using Mpmt.Core.Dtos;
using Mpmt.Core.Dtos.CashAgent;
using Mpmt.Core.Dtos.Paging;
using Mpmt.Core.Dtos.Partner;
using Mpmt.Core.Dtos.Roles;
using Mpmt.Core.Dtos.SuperAgent;
using Mpmt.Core.Events;
using Mpmt.Core.ViewModel.CashAgent;
using Mpmt.Data.Common;
using Mpmt.Data.Repositories.CashAgent;
using Mpmt.Services.Authentication;
using Mpmt.Services.Common;
using Mpmt.Services.Services.Common;
using Mpmts.Core.Dtos;
using System.Data;
using System.Security.Claims;

namespace Mpmt.Services.CashAgents
{
    public class CashAgentEmployee : BaseService, ICashAgentEmployee
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICashAgentEmployeeRepository _employeeRepository;
        private readonly IConfiguration _config;
        private readonly IFileProviderService _fileProviderService;
        private readonly IMapper _mapper;
        private readonly IEventPublisher _eventPublisher;
        private readonly ClaimsPrincipal _loggedInUser;

        public CashAgentEmployee(
            IHttpContextAccessor httpContextAccessor,
            ICashAgentEmployeeRepository EmployeeRepository,
            IConfiguration config,
            IFileProviderService fileProviderService,
            IMapper mapper,
            IEventPublisher eventPublisher)
        {
            _httpContextAccessor = httpContextAccessor;
            _employeeRepository = EmployeeRepository;
            _config = config;
            _fileProviderService = fileProviderService;
            _mapper = mapper;
            _eventPublisher = eventPublisher;
            _loggedInUser = _httpContextAccessor.HttpContext?.User;
        }

        public async Task<SprocMessage> ActivateAgentUserAsync(ActivateAgentEmployee activateAgentEmployee)
        {
            var mappedData = new CashAgentUser
            {
                IsActive = activateAgentEmployee.IsActive,
                EmployeeId = activateAgentEmployee.EmployeeId,
                UserName = activateAgentEmployee.UserName,
                Event = "AI",
                LoginUserName = _loggedInUser.FindFirstValue(ClaimTypes.Name),
                AgentCode = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(claims => claims.Type == "AgentCode")?.Value
            };

            var addResult = await _employeeRepository.IUDAgentEmployeeUserAsync(mappedData); ;

            // log out user in case of de-activation
            if (addResult.StatusCode == 200 && !mappedData.IsActive)
            {
                var emp = await _employeeRepository.GetAgentEmployeeUserByEmployeeIdAsync(mappedData.EmployeeId, mappedData.AgentCode);

                await _eventPublisher.PublishAsync(new EntityUpdatedEvent<SessionAuthExpiration>(
                    new SessionAuthExpiration { UserUniqueId = emp.UserGuid.ToString(), ExpireBefore = DateTime.UtcNow }));
            }

            return addResult;
        }

        public async Task<MpmtResult> AddAgentEmployeeUserAsync(CashAgentEmployeeVm cashAgentUserVm)
        {
            var result = new MpmtResult();
            var passwordSalt = CryptoUtils.GenerateKeySalt();
            var passwordHash = CryptoUtils.HashHmacSha512Base64(cashAgentUserVm.Password, passwordSalt);

            var ProfileImage = string.Empty;
            if (cashAgentUserVm.ProfileImage is not null && cashAgentUserVm.ProfileImage.Length > 0)
            {
                var (isValidProfileImage, _) = await FileValidatorUtils.IsValidImageAsync(cashAgentUserVm.ProfileImage, FileTypes.ImageFiles);
                if (!isValidProfileImage)
                {
                    result.AddError(400, "Invalid profile image.");
                    return result;
                }

                var folderPath = _config["Folder:AgentEmployeeCompanyLogo"];
                _fileProviderService.TryUploadFile(cashAgentUserVm.ProfileImage, folderPath, out ProfileImage);
            }

            //if (cashAgentUserVm.ProfileImage is not null && cashAgentUserVm.ProfileImage.Length > 0)
            //{
            //    var (isValidProfileImage, _) = await FileValidatorUtils.IsValidImageAsync(cashAgentUserVm.ProfileImage, FileTypes.ImageFiles);
            //    if (!isValidProfileImage)
            //    {
            //        result.AddError(400, "Invalid  image.");
            //        return result;
            //    }

            //    var folderPath = _config["Folder:AdminProfileImage"];
            //    ProfileImagePath = await FileUploadHandler.UploadFile(_hostEnv, folderPath, cashAgentUserVm.ProfileImage);
            //}

            var mappedData = _mapper.Map<CashAgentUser>(cashAgentUserVm);
            mappedData.LicenseDocImgPath = ProfileImage;
            mappedData.PasswordHash = passwordHash;
            mappedData.PasswordSalt = passwordSalt;
            mappedData.Event = "I";

            mappedData.LoginUserName = _loggedInUser.FindFirstValue(ClaimTypes.Name);
            mappedData.AgentCode = _loggedInUser.FindFirstValue("AgentCode");

            var addResult = await _employeeRepository.IUDAgentEmployeeUserAsync(mappedData);
            return MapSprocMessageToMpmtResult(addResult);
        }

        public async Task<SprocMessage> DeleteUser(ActivateAgentEmployee activateAgentEmployee)
        {
            var mappedData = new CashAgentUser
            {
                UserName = activateAgentEmployee.UserName,
                EmployeeId = activateAgentEmployee.EmployeeId,
                Remarks = activateAgentEmployee.Remarks,
                Event = "D",
                LoginUserName = _loggedInUser.FindFirstValue(ClaimTypes.Name),
                AgentCode = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(claims => claims.Type == "AgentCode")?.Value
            };

            var deleteResult = await _employeeRepository.IUDAgentEmployeeUserAsync(mappedData); ;

            // log out employee
            if (deleteResult.StatusCode == 200)
            {
                var emp = await _employeeRepository.GetAgentEmployeeUserByEmployeeIdAsync(mappedData.EmployeeId, mappedData.AgentCode);

                await _eventPublisher.PublishAsync(new EntityUpdatedEvent<SessionAuthExpiration>(
                    new SessionAuthExpiration { UserUniqueId = emp.UserGuid.ToString(), ExpireBefore = DateTime.UtcNow }));
            }

            return deleteResult;
        }

        public async Task<AgentUser> GetAgentEmployeeByPhoneNumberAsync(string PhoneNumber)
        {
            var data = await _employeeRepository.GetAgentEmployeeUserByPhonenumberAsync(PhoneNumber);
            return data;
        }

        public async Task<AgentUser> GetAgentEmployeeUserByEmployeeIdAsync(string EmployeeId, string agentCode)
        {
            var data = await _employeeRepository.GetAgentEmployeeUserByEmployeeIdAsync(EmployeeId, agentCode);
            return data;
        }

        public async Task<AgentUser> GetAgentEmployeeByUserNameAsync(string UserName)
        {
            var data = await _employeeRepository.GetAgentEmployeeUserByUserName(UserName);
            return data;
        }

        public async Task<PagedList<AgentDetail>> getAgentEmployeeList(AgentFilter AgentFilter)
        {
            AgentFilter.AgentCode = _loggedInUser.FindFirstValue("AgentCode");
            //AgentFilter.UserType = _loggedInUser.FindFirstValue("UserType");
            var data = await _employeeRepository.GetAgentEmployeeAsync(AgentFilter);
            return data;
        }

        public async Task UpdateAgentEmployeeLoginActivityAsync(AgentLoginActivity agentLoginActivity, int EmployeeId)
        {
            ArgumentNullException.ThrowIfNull(agentLoginActivity, nameof(agentLoginActivity));

            await _employeeRepository.UpdateAgentEmployeeLoginActivityAsync(agentLoginActivity, EmployeeId);
        }

        public async Task UpdateEmployeeAccountSecretKeyAsync(string AgentCode, string accountsecretkey, int employeeId)
        {
            await _employeeRepository.UpdateEmployeeAccountSecretKeyAsync(AgentCode, accountsecretkey, employeeId);
        }

        public async Task<MpmtResult> UpdateAgentEmployeeUserAsync(CashAgentEmployeeVm cashAgentUserVm)
        {
            var result = new MpmtResult();
            //var mappedData = _mapper.Map<CashAgentUser>(cashAgentUserVm);
            var mappedData = new CashAgentUser()
            {
                UserName = cashAgentUserVm.UserName,
                FirstName = cashAgentUserVm.FirstName,
                LastName = cashAgentUserVm.LastName,
                SuperAgentCode = cashAgentUserVm.SuperAgentCode,
                Email = cashAgentUserVm.Email,
                ContactNumber = cashAgentUserVm.ContactNumber,
                LicenseDocImgPath = cashAgentUserVm.LicenseDocImgPath,
                IsActive = cashAgentUserVm.IsActive,
            };

            //mappedData.LicenseDocImgPath = cashAgentUserVm.LicenseDocImgPath;
            if (cashAgentUserVm.ProfileImage is not null && cashAgentUserVm.ProfileImage.Length > 0)
            {
                var (isValidProfileImage, _) = await FileValidatorUtils.IsValidImageAsync(cashAgentUserVm.ProfileImage, FileTypes.ImageFiles);
                if (!isValidProfileImage)
                {
                    result.AddError(400, "Invalid profile image.");
                    return result;
                }
                var folderPath = _config["Folder:AgentEmployeeCompanyLogo"];
                _fileProviderService.TryUploadFile(cashAgentUserVm.ProfileImage, folderPath, out var ProfileImage);
                mappedData.LicenseDocImgPath = ProfileImage;
            }

            mappedData.EmployeeId = cashAgentUserVm.Id.ToString();
            mappedData.Event = "U";
            mappedData.LoginUserName = _loggedInUser.FindFirstValue(ClaimTypes.Name);
            mappedData.AgentCode = _loggedInUser.FindFirstValue("AgentCode");

            var addResult = await _employeeRepository.IUDAgentEmployeeUserAsync(mappedData);

            // log out employee
            if (addResult.StatusCode == 200)
            {
                var emp = await _employeeRepository.GetAgentEmployeeUserByEmployeeIdAsync(mappedData.EmployeeId, mappedData.AgentCode);

                await _eventPublisher.PublishAsync(new EntityUpdatedEvent<SessionAuthExpiration>(
                    new SessionAuthExpiration { UserUniqueId = emp.UserGuid.ToString(), ExpireBefore = DateTime.UtcNow }));
            }

            return MapSprocMessageToMpmtResult(addResult);
        }

        public async Task<string> CheckAgentOrEmployee(string usernameOrContactNumber)
        {
            var userType = CommonHelper.IsValidPin(usernameOrContactNumber, 10)
                ? await _employeeRepository.CheckAgentOrEmployeeByContactNumber(usernameOrContactNumber)
                : await _employeeRepository.CheckAgentOrEmployeeByUserName(usernameOrContactNumber);

            return userType.ToString();
        }

        public async Task<bool> VerifyUserName(string userName)
        {
            var response = await _employeeRepository.VerifyUserNameAsync(userName);
            return response;
        }

        public async Task<VerificationToken> GetOtpByAgentCodeAsync(string agentCode, string UserName, string phoneNumber)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();

            param.Add("@AgentCode", agentCode);
            param.Add("@PhoneNumber", phoneNumber);
            param.Add("@UserName", UserName);

            var data = await connection
                .QueryFirstOrDefaultAsync<VerificationToken>("[dbo].[usp_getverificationtoken_byagentcode]", param: param, commandType: CommandType.StoredProcedure);

            return data;
        }

        public async Task<SprocMessage> ForgotPasswordAsync(ForgotPasswordToken reset)
        {
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();

                var param = new DynamicParameters();
                param.Add("@AgentCode", reset.AgentCode);
                param.Add("@PhoneNumber", reset.PhoneNumber);
                param.Add("@OTP", reset.OTP);
                param.Add("@ResetToken", reset.ResetToken);

                param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
                param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                _ = await connection.ExecuteAsync("[dbo].[usp_forgot_password_reset]", param: param, commandType: CommandType.StoredProcedure);

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

        public async Task<SprocMessage> ResetTokenValidationAsync(ForgotPasswordToken reset)
        {
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();

                var param = new DynamicParameters();
                param.Add("@ResetToken", reset.ResetToken);
                param.Add("@OTP", reset.OTP);
                param.Add("@IsConsumed", reset.IsConsumed);
                param.Add("@PhoneNumber", reset.PhoneNumber);
                param.Add("@AgentCode", reset.AgentCode);

                param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
                param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                _ = await connection.ExecuteAsync("[dbo].[usp_resettoken_validation]", param: param, commandType: CommandType.StoredProcedure);

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
    }
}
