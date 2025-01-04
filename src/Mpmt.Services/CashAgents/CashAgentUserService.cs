using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Mpmt.Core.Common;
using Mpmt.Core.Dtos;
using Mpmt.Core.Dtos.CashAgent;
using Mpmt.Core.Dtos.Paging;
using Mpmt.Core.Dtos.Partner;
using Mpmt.Core.Dtos.SuperAgent;
using Mpmt.Core.Events;
using Mpmt.Core.ViewModel.CashAgent;
using Mpmt.Data.Common;
using Mpmt.Data.Repositories.CashAgent;
using Mpmt.Services.Authentication;
using Mpmt.Services.Common;
using Mpmt.Services.Services.Common;
using Mpmts.Core.Dtos;
using System.Security.Claims;
using static QRCoder.PayloadGenerator;

namespace Mpmt.Services.CashAgents
{
    public class CashAgentUserService : BaseService, ICashAgentUserService
    {
        private readonly ICashAgentRepository _cashAgentRepository;
        private readonly IMapper _mapper;
        private readonly ICashAgentEmployeeRepository _employeeRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _hostEnv;
        private readonly IFileProviderService _fileProviderService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClaimsPrincipal _loggedInUser;

        public CashAgentUserService(
            ICashAgentRepository cashAgentRepository,
            IMapper mapper,
            IConfiguration config,
            IWebHostEnvironment hostEnv,
            IHttpContextAccessor httpContextAccessor,
            IFileProviderService fileProviderService,
            ICashAgentEmployeeRepository employeeRepository,
            IEventPublisher eventPublisher)
        {
            _cashAgentRepository = cashAgentRepository;
            _mapper = mapper;
            _config = config;
            _hostEnv = hostEnv;
            _httpContextAccessor = httpContextAccessor;
            _loggedInUser = _httpContextAccessor.HttpContext?.User;
            _fileProviderService = fileProviderService;
            _employeeRepository = employeeRepository;
            _eventPublisher = eventPublisher;
        }

        public async Task<MpmtResult> AddAgentAsync(CashAgentUserVm cashAgentUserVm)
        {
            var result = new MpmtResult();
            var passwordSalt = CryptoUtils.GenerateKeySalt();
            var passwordHash = CryptoUtils.HashHmacSha512Base64(cashAgentUserVm.Password, passwordSalt);
            var mappedData = _mapper.Map<CashAgentUser>(cashAgentUserVm);
            var LicenseDocumentImagePath = string.Empty;

            if (cashAgentUserVm.LicenseDocument != null & cashAgentUserVm.LicenseDocument.Count > 0)
            {
                foreach (var image in cashAgentUserVm.LicenseDocument)
                {
                    var (isvalidlicenseImage, _) = await FileValidatorUtils.IsValidImageAsync(image, FileTypes.ImageFiles);
                    if (!isvalidlicenseImage)
                    {
                        result.AddError(400, "Invalid ID License image.");
                        continue;
                    }

                    var folderPath = _config["Folder:AgentLicenseDocument"];
                    _fileProviderService.TryUploadFile(image, folderPath, out LicenseDocumentImagePath);
                    //LicenseDocumentImagePath = await FileUploadHandler.UploadFile(_hostEnv, folderPath, image);
                    mappedData.LicensedocImgPath.Add(LicenseDocumentImagePath);
                }
                if (result.Errors.Count > 0)
                {
                    return result;
                }
            }

            var folderPaths = _config["Folder:CashAgentCompanyLogo"];
            _fileProviderService.TryUploadFile(cashAgentUserVm.DocumentImage, folderPaths, out var CompanyLogoImgPath);

            mappedData.Event = "I";
            mappedData.CompanyLogoImgPath = CompanyLogoImgPath;
            mappedData.PasswordHash = passwordHash;
            mappedData.PasswordSalt = passwordSalt;
            mappedData.LoginUserName = _loggedInUser.FindFirstValue(ClaimTypes.Name);
            mappedData.SuperAgentCode = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(claims => claims.Type == "AgentCode")?.Value;
            var addResult = await _cashAgentRepository.IUDAgentUserAsync(mappedData);
            if (addResult.StatusCode != 200)
            {
                if (!string.IsNullOrEmpty(LicenseDocumentImagePath))
                {
                    string imgPath = LicenseDocumentImagePath.Substring(1);
                    //string existingImage = Path.Combine("" + _hostEnv.WebRootPath, imgPath);
                    string pths = _config["Static:UserDataDirectory"];
                    string existingImage = Path.Combine(pths + imgPath);
                    File.Delete(existingImage);
                }
                if (!string.IsNullOrEmpty(CompanyLogoImgPath))
                {
                    string imgPath = CompanyLogoImgPath.Substring(1);
                    //string existingImage = Path.Combine("" + _hostEnv.WebRootPath, imgPath);
                    string pths = _config["Static:UserDataDirectory"];
                    string existingImage = Path.Combine(pths + imgPath);
                    File.Delete(existingImage);
                }
                if (mappedData.LicensedocImgPath != null & mappedData.LicensedocImgPath.Count > 0)
                {
                    foreach (var img in mappedData.LicensedocImgPath)
                    {
                        string imgPath = img.Substring(1);
                        //string existingImage = Path.Combine("" + _hostEnv.WebRootPath, imgPath);
                        string pths = _config["Static:UserDataDirectory"];
                        string existingImage = Path.Combine(pths + imgPath);
                        File.Delete(existingImage);
                    }
                }
                result.AddError(addResult.StatusCode, addResult.MsgText);
                return result;
            }
            return MapSprocMessageToMpmtResult(addResult);
        }

        public async Task<MpmtResult> AddAgentUserAsync(CashAgentUserVm cashAgentUserVm)
        {
            var result = new MpmtResult();
            var passwordSalt = CryptoUtils.GenerateKeySalt();
            var passwordHash = CryptoUtils.HashHmacSha512Base64(cashAgentUserVm.Password, passwordSalt);
            var mappedData = _mapper.Map<CashAgentUser>(cashAgentUserVm);
            var LicenseDocumentImagePath = string.Empty;

            if (cashAgentUserVm.LicenseDocument != null & cashAgentUserVm.LicenseDocument.Count > 0)
            {
                foreach (var image in cashAgentUserVm.LicenseDocument)
                {
                    var (isvalidlicenseImage, _) = await FileValidatorUtils.IsValidImageAsync(image, FileTypes.ImageFiles);
                    if (!isvalidlicenseImage)
                    {
                        result.AddError(400, "Invalid ID License image.");
                        continue;
                    }

                    var folderPath = _config["Folder:AgentLicenseDocument"];
                    _fileProviderService.TryUploadFile(image, folderPath, out LicenseDocumentImagePath);
                    //LicenseDocumentImagePath = await FileUploadHandler.UploadFile(_hostEnv, folderPath, image);
                    mappedData.LicensedocImgPath.Add(LicenseDocumentImagePath);
                }
                if (result.Errors.Count > 0)
                {
                    return result;
                }
            }

            var folderPaths = _config["Folder:CashAgentCompanyLogo"];
            _fileProviderService.TryUploadFile(cashAgentUserVm.DocumentImage, folderPaths, out var CompanyLogoImgPath);
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

            mappedData.CompanyLogoImgPath = CompanyLogoImgPath;
            //mappedData.LicenseDocImgPath = LicenseDocumentImagePath;
            mappedData.PasswordHash = passwordHash;
            mappedData.PasswordSalt = passwordSalt;
            mappedData.Event = "I";
            mappedData.LoginUserName = _loggedInUser.FindFirstValue(ClaimTypes.Name);

            var addResult = await _cashAgentRepository.IUDAgentUserAsync(mappedData);
            if (addResult.StatusCode != 200)
            {
                try
                {
                    if (!string.IsNullOrEmpty(LicenseDocumentImagePath))
                    {
                        string imgPath = LicenseDocumentImagePath.Substring(1);
                        //string existingImage = Path.Combine("" + _hostEnv.WebRootPath, imgPath);
                        string pths = _config["Static:UserDataDirectory"];
                        string existingImage = Path.Combine(pths + imgPath);
                        File.Delete(existingImage);
                    }
                    if (!string.IsNullOrEmpty(CompanyLogoImgPath))
                    {
                        string imgPath = CompanyLogoImgPath.Substring(1);
                        //string existingImage = Path.Combine("" + _hostEnv.ContentRootPath, imgPath);
                        string pths = _config["Static:UserDataDirectory"];
                        string existingImage = Path.Combine(pths + imgPath);
                        File.Delete(existingImage);
                    }
                    if (mappedData.LicensedocImgPath != null & mappedData.LicensedocImgPath.Count > 0)
                    {
                        foreach (var img in mappedData.LicensedocImgPath)
                        {
                            string imgPath = img.Substring(1);
                            string pths = _config["Static:UserDataDirectory"];
                            string existingImage = Path.Combine(pths + imgPath);
                            //string existingImage = Path.Combine("" + _hostEnv.WebRootPath, imgPath);
                            File.Delete(existingImage);
                        }
                    }
                    result.AddError(addResult.StatusCode, addResult.MsgText);
                    return result;
                }
                catch (Exception ex)
                {

                }
            }
            return MapSprocMessageToMpmtResult(addResult);
        }

        public async Task<SprocMessage> ActivateAgentUserAsync(ActivateAgent activateAgent)
        {
            var mappedData = new CashAgentUser
            {
                IsActive = activateAgent.IsActive,
                AgentCode = activateAgent.AgentCode,
                Event = "AI",
                LoginUserName = _loggedInUser.FindFirstValue(ClaimTypes.Name)
            };

            var activeInactiveResult = await _cashAgentRepository.IUDAgentUserAsync(mappedData);

            // logout user
            if (activeInactiveResult.StatusCode == 200 && !mappedData.IsActive)
            {
                var agent = await _cashAgentRepository.GetCashAgentByAgentCodeAsync(mappedData.AgentCode);

                await _eventPublisher.PublishAsync(new EntityUpdatedEvent<SessionAuthExpiration>(
                    new SessionAuthExpiration { UserUniqueId = agent.UserGuid.ToString(), ExpireBefore = DateTime.UtcNow }));
            }

            return activeInactiveResult;
        }

        public async Task<SprocMessage> MarkasSuperAgent(ActivateAgent activateAgent)
        {
            var mappedData = new CashAgentUser
            {
                IsActive = activateAgent.IsActive,
                AgentCode = activateAgent.AgentCode,
                Event = "SI",
                LoginUserName = _loggedInUser.FindFirstValue(ClaimTypes.Name)
            };

            var addResult = await _cashAgentRepository.IUDAgentUserAsync(mappedData);
            return addResult;
        }

        public async Task<SprocMessage> DeleteUser(ActivateAgent activateAgent)
        {
            var mappedData = new CashAgentUser
            {
                AgentCode = activateAgent.AgentCode,
                Remarks = activateAgent.Remarks,
                Event = "D",
                LoginUserName = _loggedInUser.FindFirstValue(ClaimTypes.Name)
            };

            var deleteResult = await _cashAgentRepository.IUDAgentUserAsync(mappedData);

            // logout user
            if (deleteResult.StatusCode == 200)
            {
                var agent = await _cashAgentRepository.GetCashAgentByAgentCodeAsync(mappedData.AgentCode);

                await _eventPublisher.PublishAsync(new EntityUpdatedEvent<SessionAuthExpiration>(
                    new SessionAuthExpiration { UserUniqueId = agent.UserGuid.ToString(), ExpireBefore = DateTime.UtcNow }));
            }

            return deleteResult;
        }

        public async Task<PagedList<AgentUserModel>> GetAgentAsync(AgentFilterModel agentFilter)
        {
            agentFilter.AgentCode = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(claims => claims.Type == "AgentCode")?.Value;
            var data = await _cashAgentRepository.GetAgentAsync(agentFilter);
            return data;
        }

        public async Task<AgentUser> GetAgentByPhoneNumberAsync(string PhoneNumber)
        {
            var data = await _cashAgentRepository.GetAgentUserByPhonenumber(PhoneNumber);
            return data;
        }

        public async Task<AgentUser> GetAgentByUserNameAsync(string UserName)
        {
            var data = await _cashAgentRepository.GetAgentUserByUserName(UserName);
            return data;
        }

        public async Task<PagedList<AgentDetail>> GetAgentUserAsync(AgentFilter AgentFilter)
        {
            var data = await _cashAgentRepository.GetAgentUserAsync(AgentFilter);

            return data;
        }

        public async Task UpdateAccountSecretKeyAsync(string AgentCode, string accountsecretkey)
        {
            await _cashAgentRepository.UpdateAccountSecretKeyAsync(AgentCode, accountsecretkey);
        }

        public async Task<AgentUser> GetCashAgentByAgentCodeAsync(string agentCode)
        {
            var data = await _cashAgentRepository.GetCashAgentByAgentCodeAsync(agentCode);
            return data;
        }

        public Task UpdateAgentLoginActivityAsync(AgentLoginActivity agentLoginActivity)
        {
            ArgumentNullException.ThrowIfNull(agentLoginActivity, nameof(agentLoginActivity));

            return _cashAgentRepository.UpdateAgentLoginActivityAsync(agentLoginActivity);
        }

        public async Task<MpmtResult> UpdateAgentUserAsync(CashAgentUpdateVm cashAgentUserVm)
        {
            var result = new MpmtResult();
            var mappedData = _mapper.Map<CashAgentUser>(cashAgentUserVm);
            var agent = await _cashAgentRepository.GetCashAgentByAgentCodeAsync(cashAgentUserVm.AgentCode);

            var CompanyLogoImgPath = cashAgentUserVm.DocumentImagepath;
            if (cashAgentUserVm.DocumentImagepath == null || cashAgentUserVm.DocumentImage != null)
            {
                var folderPaths = _config["Folder:CashAgentCompanyLogo"];
                _fileProviderService.TryUploadFile(cashAgentUserVm.DocumentImage, folderPaths, out CompanyLogoImgPath);
            }

            var LicenseDocumentImagePath = string.Empty;
            var licenseImagesuploaded = new List<string>();

            if (cashAgentUserVm.LicenseDocument != null & cashAgentUserVm.LicenseDocument.Count > 0)
            {
                foreach (var image in cashAgentUserVm.LicenseDocument)
                {
                    var (isvalidlicenseImage, _) = await FileValidatorUtils.IsValidImageAsync(image, FileTypes.ImageFiles);
                    if (!isvalidlicenseImage)
                    {
                        result.AddError(400, "Invalid ID License image.");
                        continue;
                    }

                    var folderPath = _config["Folder:AgentLicenseDocument"];
                    _fileProviderService.TryUploadFile(image, folderPath, out LicenseDocumentImagePath);
                    //LicenseDocumentImagePath = await FileUploadHandler.UploadFile(_hostEnv, folderPath, image);
                    //mappedData.LicensedocImgPath.Add(LicenseDocumentImagePath);
                    licenseImagesuploaded.Add(LicenseDocumentImagePath);
                }
                if (result.Errors.Count > 0)
                {
                    return result;
                }
            }
            mappedData.LicensedocImgPath = licenseImagesuploaded;
            if (cashAgentUserVm.LicensedocImgPath is not null && cashAgentUserVm.LicensedocImgPath.Count > 0)
            {
                foreach (var licenseimgPath in cashAgentUserVm.LicensedocImgPath)
                {
                    mappedData.LicensedocImgPath.Add(licenseimgPath);
                }
            }

            mappedData.LoginUserName = _loggedInUser.FindFirstValue(ClaimTypes.Name);
            mappedData.CompanyLogoImgPath = CompanyLogoImgPath;
            mappedData.Event = "U";
            var addResult = await _cashAgentRepository.IUDAgentUserAsync(mappedData);
            if (addResult.StatusCode != 200)
            {
                if (!string.IsNullOrEmpty(LicenseDocumentImagePath))
                {
                    string imgPath = LicenseDocumentImagePath.Substring(1);
                    //string existingImage = Path.Combine("" + _hostEnv.WebRootPath, imgPath);
                    string pths = _config["Static:UserDataDirectory"];
                    string existingImage = Path.Combine(pths + imgPath);
                    File.Delete(existingImage);
                }
                if (!string.IsNullOrEmpty(CompanyLogoImgPath) && cashAgentUserVm.DocumentImage != null)
                {
                    string imgPath = CompanyLogoImgPath.Substring(1);
                    //string existingImage = Path.Combine("" + _hostEnv.WebRootPath, imgPath);
                    string pths = _config["Static:UserDataDirectory"];
                    string existingImage = Path.Combine(pths + imgPath);
                    File.Delete(existingImage);
                }
                if (mappedData.LicensedocImgPath != null && mappedData.LicensedocImgPath.Count > 0 && cashAgentUserVm.LicenseDocument != null && cashAgentUserVm.LicenseDocument.Count > 0)
                {
                    foreach (var img in mappedData.LicensedocImgPath)
                    {
                        string imgPath = img[1..];
                        //string existingImage = Path.Combine("" + _hostEnv.WebRootPath, imgPath);
                        string pths = _config["Static:UserDataDirectory"];
                        string existingImage = Path.Combine(pths + imgPath);
                        File.Delete(existingImage);
                    }
                }
                result.AddError(addResult.StatusCode, addResult.MsgText);
                return result;
            }

            // log out user
            if (!cashAgentUserVm.IsActive)
            {
                await _eventPublisher.PublishAsync(new EntityUpdatedEvent<SessionAuthExpiration>(
                    new SessionAuthExpiration { UserUniqueId = agent.UserGuid.ToString(), ExpireBefore = DateTime.UtcNow }));
            }

            //delete old image only when new image
            if (!string.IsNullOrEmpty(agent.CompanyLogoImgPath) && cashAgentUserVm.DocumentImage is not null)
            {
                string imgPath = agent.CompanyLogoImgPath[1..];
                //string existingImage = Path.Combine("" + _hostEnv.WebRootPath, imgPath);
                string pths = _config["Static:UserDataDirectory"];
                string existingImage = Path.Combine(pths + imgPath);
                File.Delete(existingImage);
            }
            if (cashAgentUserVm.DeletedLicensedocImgPath is not null && cashAgentUserVm.DeletedLicensedocImgPath.Count > 0)
            {
                foreach (var image in cashAgentUserVm.DeletedLicensedocImgPath)
                {
                    if (!string.IsNullOrEmpty(image))
                    {
                        string imgPath = image.Substring(1);
                        //string existingImage = Path.Combine("" + _hostEnv.WebRootPath, imgPath);
                        string pths = _config["Static:UserDataDirectory"];
                        string existingImage = Path.Combine(pths + imgPath);
                        File.Delete(existingImage);
                    }
                }
            }

            return MapSprocMessageToMpmtResult(addResult);
        }

        public async Task<MpmtResult> UpdateAgentAsync(CashAgentUserVm cashAgentUserVm)
        {
            var result = new MpmtResult();
            var mappedData = _mapper.Map<CashAgentUser>(cashAgentUserVm);
            var agent = await _cashAgentRepository.GetCashAgentByAgentCodeAsync(cashAgentUserVm.AgentCode);
            var folderPaths = _config["Folder:CashAgentCompanyLogo"];
            _fileProviderService.TryUploadFile(cashAgentUserVm.DocumentImage, folderPaths, out var CompanyLogoImgPath);
            var LicenseDocumentImagePath = string.Empty;
            var licenseImagesuploaded = new List<string>();
            if (cashAgentUserVm.LicenseDocument != null & cashAgentUserVm.LicenseDocument.Count > 0)
            {
                foreach (var image in cashAgentUserVm.LicenseDocument)
                {
                    var (isvalidlicenseImage, _) = await FileValidatorUtils.IsValidImageAsync(image, FileTypes.ImageFiles);
                    if (!isvalidlicenseImage)
                    {
                        result.AddError(400, "Invalid ID License image.");
                        continue;
                    }

                    var folderPath = _config["Folder:AgentLicenseDocument"];
                    _fileProviderService.TryUploadFile(image, folderPath, out LicenseDocumentImagePath);
                    //LicenseDocumentImagePath = await FileUploadHandler.UploadFile(_hostEnv, folderPath, image);
                    //mappedData.LicensedocImgPath.Add(LicenseDocumentImagePath);
                    licenseImagesuploaded.Add(LicenseDocumentImagePath);
                }
                if (result.Errors.Count > 0)
                {
                    return result;
                }
            }
            mappedData.LicensedocImgPath = licenseImagesuploaded;
            if (cashAgentUserVm.LicensedocImgPath is not null && cashAgentUserVm.LicensedocImgPath.Count > 0)
            {
                foreach (var licenseimgPath in cashAgentUserVm.LicensedocImgPath)
                {
                    mappedData.LicensedocImgPath.Add(licenseimgPath);
                }
            }
            mappedData.LoginUserName = _loggedInUser.FindFirstValue(ClaimTypes.Name);
            mappedData.CompanyLogoImgPath = CompanyLogoImgPath;
            mappedData.Event = "U";
            var addResult = await _cashAgentRepository.IUDAgentUserAsync(mappedData);
            if (addResult.StatusCode != 200)
            {
                if (!string.IsNullOrEmpty(LicenseDocumentImagePath))
                {
                    string imgPath = LicenseDocumentImagePath.Substring(1);
                    //string existingImage = Path.Combine("" + _hostEnv.WebRootPath, imgPath);
                    string pths = _config["Static:UserDataDirectory"];
                    string existingImage = Path.Combine(pths + imgPath);
                    File.Delete(existingImage);
                }
                if (!string.IsNullOrEmpty(CompanyLogoImgPath))
                {
                    string imgPath = CompanyLogoImgPath.Substring(1);
                    //string existingImage = Path.Combine("" + _hostEnv.WebRootPath, imgPath);
                    string pths = _config["Static:UserDataDirectory"];
                    string existingImage = Path.Combine(pths + imgPath);
                    File.Delete(existingImage);
                }
                if (mappedData.LicensedocImgPath != null & mappedData.LicensedocImgPath.Count > 0)
                {
                    foreach (var img in mappedData.LicensedocImgPath)
                    {
                        string imgPath = img[1..];
                        //string existingImage = Path.Combine("" + _hostEnv.WebRootPath, imgPath);
                        string pths = _config["Static:UserDataDirectory"];
                        string existingImage = Path.Combine(pths + imgPath);
                        File.Delete(existingImage);
                    }
                }
                result.AddError(addResult.StatusCode, addResult.MsgText);
                return result;
            }

            // log out user
            if (!cashAgentUserVm.IsActive)
            {
                await _eventPublisher.PublishAsync(new EntityUpdatedEvent<SessionAuthExpiration>(
                    new SessionAuthExpiration { UserUniqueId = agent.UserGuid.ToString(), ExpireBefore = DateTime.UtcNow }));
            }

            //delete old image only when new image
            if (!string.IsNullOrEmpty(agent.CompanyLogoImgPath) && cashAgentUserVm.DocumentImage is not null)
            {
                string imgPath = agent.CompanyLogoImgPath.Substring(1);
                //string existingImage = Path.Combine("" + _hostEnv.WebRootPath, imgPath);
                string pths = _config["Static:UserDataDirectory"];
                string existingImage = Path.Combine(pths + imgPath);
                File.Delete(existingImage);
            }
            if (cashAgentUserVm.DeletedLicensedocImgPath is not null && cashAgentUserVm.DeletedLicensedocImgPath.Count > 0)
            {
                foreach (var image in cashAgentUserVm.DeletedLicensedocImgPath)
                {
                    if (!string.IsNullOrEmpty(image))
                    {
                        string imgPath = image[1..];
                        //string existingImage = Path.Combine("" + _hostEnv.WebRootPath, imgPath);
                        string pths = _config["Static:UserDataDirectory"];
                        string existingImage = Path.Combine(pths + imgPath);
                        File.Delete(existingImage);
                    }
                }
            }
            return MapSprocMessageToMpmtResult(addResult);
        }

        public async Task<SprocMessage> ChangeAgentPassword(AgentChangePasswordVM changepassword)
        {
            var response = new SprocMessage()
            {
                StatusCode = 400,
                MsgText = "Something Went wrong",
            };
            var userType = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserType")?.Value;
            var agentCode = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "AgentCode")?.Value;
            var UserName = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

            bool PasswordCheck = false;
            string AgentCode = "";

            if (userType == "Agent" || userType == "SuperAgent")
            {
                var agent = await _cashAgentRepository.GetCashAgentByAgentCodeAsync(agentCode);
                if (agent == null)
                {
                    response.StatusCode = 400;
                    response.MsgText = "Invalid Credentials";
                    return response;
                }

                AgentCode = agent.AgentCode;
                PasswordCheck = HashUtils.CheckEqualBase64HashHmacSha512(changepassword.OldPassword, agent.PasswordHash, agent.PasswordSalt);

                var newpasswordsameasold = HashUtils.CheckEqualBase64HashHmacSha512(changepassword.NewPassword, agent.PasswordHash, agent.PasswordSalt);

                if (newpasswordsameasold)
                {
                    response.StatusCode = 400;
                    response.MsgText = "New Password cant be same as old one";
                    return response;
                }

                if (PasswordCheck)
                {
                    var passwordSalt = CryptoUtils.GenerateKeySalt();
                    var passwordHash = CryptoUtils.HashHmacSha512Base64(changepassword.NewPassword, passwordSalt);

                    var request = new CashAgentUser()
                    {
                        Event = "CP",
                        UserName = agent.UserName,
                        PasswordHash = passwordHash,
                        PasswordSalt = passwordSalt,
                        UserType = userType,
                        AgentCode = AgentCode,
                        LoginUserId = "1",
                        LoginUserName = UserName
                    };
                    response = await _cashAgentRepository.AgentChangePasswordAsync(request);

                    // log out user
                    if (response.StatusCode == 200)
                    {
                        await _eventPublisher.PublishAsync(new EntityUpdatedEvent<SessionAuthExpiration>(
                            new SessionAuthExpiration { UserUniqueId = agent.UserGuid.ToString(), ExpireBefore = DateTime.UtcNow }));
                    }
                }
                else
                {
                    response.StatusCode = 400;
                    response.MsgText = "Wrong Password";
                }

                return response;
            }
            else if (userType == "Employee")
            {
                var employeeId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "EmployeeId")?.Value;
                //var employeeId = _loggedInUser.FindFirstValue("EmployeeId");
                var employee = await _employeeRepository.GetAgentEmployeeUserByEmployeeIdAsync(employeeId, agentCode);
                if (employee == null)
                {
                    response.StatusCode = 400;
                    response.MsgText = "Invalid Credentials";
                    return response;
                }
                AgentCode = employee.AgentCode;
                PasswordCheck = HashUtils.CheckEqualBase64HashHmacSha512(changepassword.OldPassword, employee.PasswordHash, employee.PasswordSalt);

                var newpasswordsameasold = HashUtils.CheckEqualBase64HashHmacSha512(changepassword.NewPassword, employee.PasswordHash, employee.PasswordSalt);

                if (newpasswordsameasold)
                {
                    response.StatusCode = 400;
                    response.MsgText = "New Password cant be same as old one";
                    return response;
                }

                if (PasswordCheck)
                {
                    var passwordSalt = CryptoUtils.GenerateKeySalt();
                    var passwordHash = CryptoUtils.HashHmacSha512Base64(changepassword.NewPassword, passwordSalt);

                    var request = new CashAgentUser()
                    {
                        Event = "CP",
                        EmployeeId = employeeId,
                        UserName = employee.UserName,
                        PasswordHash = passwordHash,
                        PasswordSalt = passwordSalt,
                        UserType = userType,
                        AgentCode = AgentCode,
                        LoginUserId = employeeId,
                        LoginUserName = UserName
                    };
                    response = await _employeeRepository.AgentEmployeeChangePasswordAsync(request);

                    // log out user
                    if (response.StatusCode == 200)
                    {
                        await _eventPublisher.PublishAsync(new EntityUpdatedEvent<SessionAuthExpiration>(
                            new SessionAuthExpiration { UserUniqueId = employee.UserGuid.ToString(), ExpireBefore = DateTime.UtcNow }));
                    }
                }
                else
                {
                    response.StatusCode = 400;
                    response.MsgText = "Wrong Password";
                }
                return response;
            }

            response.StatusCode = 400;
            response.MsgText = "Permission Denied";
            return response;
        }

        public async Task<bool> VerifyUserName(string userName)
        {
            var response = await _cashAgentRepository.VerifyUserNameAsync(userName);
            return response;
        }

        public async Task<bool> VerifyContactNumber(string contactNumber)
        {
            var response = await _cashAgentRepository.VerifyContactNumber(contactNumber);
            return response;
        }

        public async Task<bool> VerifyRegistrationNumber(string registrationNumber)
        {
            var response = await _cashAgentRepository.VerifyRegistrationNumber(registrationNumber);
            return response;
        }

        public async Task<PagedList<AgentLedger>> GetAgentLedgerAsync(AgentLedgerFilter AgentLedgerFilter)
        {
            var response = await _cashAgentRepository.GetAgentLedgerAsync(AgentLedgerFilter);
            return response;
        }

        public async Task<PagedList<AgentDetail>> GetAgentBySuperAgentAsync(AgentFilter agentFilter)
        {
            var response = await _cashAgentRepository.GetAgentBySuperAgentAsync(agentFilter);
            return response;
        }

        public async Task<PagedList<CashAgentRegister>> GetRemitAgentRegisterAsync(AgentRegisterFilter request)
        {
            var data = await _cashAgentRepository.GetRemitAgentRegisterAsync(request);
            return data;
        }

        public async Task<SprocMessage> ApprovedAgentRequest(CashAgentRequest request, ClaimsPrincipal claimsPrincipal)
        {
            request.OperationMode = "A";
            request.LoggedInUser = claimsPrincipal?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            request.UserType = claimsPrincipal?.Claims.FirstOrDefault(x => x.Type == "UserType")?.Value;
            var Response = await _cashAgentRepository.ApprovedRejectAgentRequest(request);
            return Response;
        }

        public async Task<SprocMessage> RejectAgentRequest(CashAgentRequest request, ClaimsPrincipal claimsPrincipal)
        {
            request.OperationMode = "R";
            request.LoggedInUser = claimsPrincipal?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            request.UserType = claimsPrincipal?.Claims.FirstOrDefault(x => x.Type == "UserType")?.Value;
            var Response = await _cashAgentRepository.ApprovedRejectAgentRequest(request);
            return Response;
        }

        public async Task<AgentDetailSignUp> GetAgentDetail(string Email, string phoneNumber)
        {
            var data = await _cashAgentRepository.GetAgentDetail(Email, phoneNumber);
            return data;
        }

        public async Task<MpmtResult> AddFundRequestAsync(AddAgentFundRequest addUpdateFundRequest, ClaimsPrincipal user)
        {
            var result = new MpmtResult();
            addUpdateFundRequest.Event = 'A';
            addUpdateFundRequest.LoggedInUser = user?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            addUpdateFundRequest.UserType = user?.Claims.FirstOrDefault(x => x.Type == "UserType")?.Value;

            string imagePath = null;
            if (addUpdateFundRequest.VoucherImg != null)
            {
                var (isValidVoucherImg, _) = await FileValidatorUtils.IsValidImageAsync(addUpdateFundRequest.VoucherImg, FileTypes.ImageFiles);
                if (!isValidVoucherImg)
                {
                    result.AddError(400, "Invalid voucher image.");
                    return result;
                }

                var folderPath = _config["Folder:AgentFundRequestVoucherImage"];
                _fileProviderService.TryUploadFile(addUpdateFundRequest.VoucherImg, folderPath, out imagePath);
                //imagePath = await FileUploadHandler.UploadFile(_hostEnv, folderPath, addUpdateFundRequest.VoucherImg);
                addUpdateFundRequest.VoucherImgPath = imagePath;
            }

            var addFundStatus = await _cashAgentRepository.AddUpdateFundRequestAsync(addUpdateFundRequest);
            if (addFundStatus.StatusCode != 200)
            {
                // Delete image file for failed operations
                if (!string.IsNullOrEmpty(imagePath))
                {
                    string imgPath = imagePath.Substring(1);
                    //string existingImage = Path.Combine("" + _hostEnv.WebRootPath, imgPath);
                    string pths = _config["Static:UserDataDirectory"];
                    string existingImage = Path.Combine(pths + imgPath);
                    File.Delete(existingImage);
                }

                result.AddError(addFundStatus.StatusCode, addFundStatus.MsgText);
                return result;
            }

            result.AddSuccess(addFundStatus.StatusCode, addFundStatus.MsgText);
            return result;
        }

        public async Task<AgentPrefundDetail> GetAgentPrefundByAgentCode(string AgentCode)
        {
            var data = await _cashAgentRepository.GetAgentPrefundByAgentCode(AgentCode);
            return data;
        }

        public async Task<PagedList<AgentAccountStatement>> GetAgentAccountSettlementReport(AgentStatementFilter filter)
        {
            var data = await _cashAgentRepository.GetAgentAccountSettlementReport(filter);
            return data;
        }

        public async Task<SprocMessage> Withdraw(Withdraw withdraw, ClaimsPrincipal user)
        {
            withdraw.LoggedInUser = user?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            withdraw.UserType = user?.Claims.FirstOrDefault(x => x.Type == "UserType")?.Value;
            var result = await _cashAgentRepository.WithdrawPrefundAsync(withdraw);
            return result;
        }
    }
}
