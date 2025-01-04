using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Mpmt.Core.Common;
using Mpmt.Core.Dtos;
using Mpmt.Core.Dtos.AdminUser;
using Mpmt.Core.Dtos.Notification;
using Mpmt.Core.Dtos.Paging;
using Mpmt.Core.Dtos.Users;
using Mpmt.Core.ViewModel.AdminUser;
using Mpmt.Data.Common;
using Mpmt.Data.Repositories.Users.AdminUser;
using Mpmt.Services.Common;
using Mpmt.Services.Services.Common;
using Mpmts.Core.Dtos;
using System.Security.Claims;

namespace Mpmt.Services.Services.AdminUser
{
    /// <summary>
    /// The adminuser services.
    /// </summary>
    public class AdminuserServices : BaseService, IAdminUserServices
    {
        private readonly IAdminUserRepo _adminuserRepo;
        private readonly IConfiguration _config;
        private readonly IFileProviderService _fileProviderService; 
        private readonly IWebHostEnvironment _hostEnv;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminuserServices"/> class.
        /// </summary>
        /// <param name="adminuserRepo">The adminuser repo.</param>
        public AdminuserServices(IAdminUserRepo adminuserRepo, IMapper mapper, IConfiguration config, IWebHostEnvironment hostEnv, IFileProviderService fileProviderService)
        {
            _adminuserRepo = adminuserRepo;
            _mapper = mapper;
            _config = config;
            _hostEnv = hostEnv;
            _fileProviderService = fileProviderService;
        }

        public async Task<MpmtResult> AddAdminUserAsync(AdminUserVm adminUser, ClaimsPrincipal claim)
        {
            var result = new MpmtResult();
            var passwordSalt = CryptoUtils.GenerateKeySalt();
            var passwordHash = CryptoUtils.HashHmacSha512Base64(adminUser.Password, passwordSalt);

            var ProfileImagePath = string.Empty;
            if (adminUser.ProfileImage is not null && adminUser.ProfileImage.Length > 0)
            {
                var (isValidProfileImage, _) = await FileValidatorUtils.IsValidImageAsync(adminUser.ProfileImage, FileTypes.ImageFiles);
                if (!isValidProfileImage)
                {
                    result.AddError(400, "Invalid provile image.");
                    return result;
                }

                var folderPath = _config["Folder:AdminProfileImage"];
                _fileProviderService.TryUploadFile(adminUser.ProfileImage, folderPath, out ProfileImagePath);
                //ProfileImagePath = await FileUploadHandler.UploadFile(_hostEnv, folderPath, adminUser.ProfileImage);
            }

            var mappedData = _mapper.Map<IUDAdminUser>(adminUser);
            mappedData.ProfileImageUrlPath = ProfileImagePath;
            mappedData.PasswordHash = passwordHash;
            mappedData.PasswordSalt = passwordSalt;
            mappedData.Event = 'I';
            mappedData.LoggedInUser = claim?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

            var addResult = await _adminuserRepo.IUDAdminUserAsync(mappedData);
            return MapSprocMessageToMpmtResult(addResult);
        }

        public async Task<SprocMessage> DeleteAdminUserAsync(int id, string remarks)
        {
            var response = await _adminuserRepo.DeleteAdminUserAsync(id, remarks);
            return response;
        }

        /// <summary>
        /// Gets the admin user async.
        /// </summary>
        /// <param name="userFilter">The user filter.</param>
        /// <returns>A Task.</returns>
        public async Task<PagedList<AdminUserDetails>> GetAdminUserAsync(AdminUserFilter userFilter)
        {
            var data = await _adminuserRepo.GetAdminUserAsync(userFilter);
            return data;
        }

        public async Task<IUDAdminUser> GetAdminUserByIdAsync(int id)
        {
            var response = await _adminuserRepo.GetAdminUserById(id);
            return response;
        }

        public async Task<bool> VerifyUserNameAdmin(string userName)
        {
            var response = await _adminuserRepo.VerifyUserNameAdminAsync(userName);
            return response;
        }

        public async Task<bool> VerifyEmailAdmin(string Email)
        {
            var response = await _adminuserRepo.VerifyEmailAdminAsync(Email);
            return response;
        }

        public async Task<MpmtResult> UpdateAdminUserAsync(AdminUserVm adminUser, ClaimsPrincipal claim)
        {
            var result = new MpmtResult();
            var ProfileImagePath = adminUser.ProfileImageUrlPath;

            if (adminUser.ProfileImage is not null && adminUser.ProfileImage.Length > 0)
            {
                var (isValidProfileImage, _) = await FileValidatorUtils.IsValidImageAsync(adminUser.ProfileImage, FileTypes.ImageFiles);
                if (!isValidProfileImage)
                {
                    result.AddError(400, "Invalid profile image.");
                    return result;
                }

                var folderPath = _config["Folder:AdminProfileImage"];
                _fileProviderService.TryUploadFile(adminUser.ProfileImage, folderPath, out ProfileImagePath);
                //ProfileImagePath = await FileUploadHandler.UploadFile(_hostEnv, folderPath, adminUser.ProfileImage);
            }

            var mappedData = _mapper.Map<IUDAdminUser>(adminUser);
            mappedData.ProfileImageUrlPath = ProfileImagePath;
            mappedData.Event = 'U';
            mappedData.LoggedInUser = claim?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

            var updateResult = await _adminuserRepo.IUDAdminUserAsync(mappedData);
            return MapSprocMessageToMpmtResult(updateResult);
        }

        public async Task<SprocMessage> AssignUserRole(int user_id, int[] roleids)
        {
            var response = await _adminuserRepo.AssignRoletoUser(user_id, roleids);
            return response;
        }

        public async Task<SprocMessage> AssignPartnerRole(int user_id, int[] roleids)
        {
            var response = await _adminuserRepo.AssignPartnerRole(user_id, roleids);    
            return response;
        }
    }
}
