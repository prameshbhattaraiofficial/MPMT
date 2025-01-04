using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Mpmt.Core.Common;
using Mpmt.Core.Domain.Partners.Senders;
using Mpmt.Core.Dtos;
using Mpmt.Core.Dtos.Paging;
using Mpmt.Core.Dtos.Transaction;
using Mpmt.Core.ViewModel.User;
using Mpmt.Data.Common;
using Mpmt.Data.Repositories.Partner;
using Mpmt.Services.Common;
using Mpmts.Core.Dtos;
using System.Security.Claims;

namespace Mpmt.Services.Partner
{
    public class PartnerSenderService : IPartnerSenderService
    {
        private readonly IPartnerSenderRepository _partnerSenderRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IFileProviderService _fileProviderService;
        private readonly IWebHostEnvironment _hostEnv;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClaimsPrincipal _loggedInUser;

        public PartnerSenderService(IPartnerSenderRepository partnerSenderRepository,
             IMapper mapper,
            IConfiguration config,
            IWebHostEnvironment hostEnv,
             IHttpContextAccessor httpContextAccessor,
             IFileProviderService fileProviderService)
        {
            _partnerSenderRepository = partnerSenderRepository;
            _mapper = mapper;
            _config = config;
            _hostEnv = hostEnv;
            _httpContextAccessor = httpContextAccessor;
            _httpContextAccessor = httpContextAccessor;
            _loggedInUser = httpContextAccessor.HttpContext.User;
            _fileProviderService = fileProviderService;
        }

        public async Task<MpmtResult> AddSenderAsync(AddUserViewModel sender)
        {
            var result = new MpmtResult();
            var PartnerCode = _loggedInUser.Claims.FirstOrDefault(x => x.Type == "PartnerCode")?.Value;

            var ProfileImagePath = string.Empty;
            var IDBackImagePath = string.Empty;
            var IDFrontImagePath = string.Empty;

            if (sender.ProfileImage.Length > 0)
            {
                var (isValidProfileImage, _) = await FileValidatorUtils.IsValidImageAsync(sender.ProfileImage, FileTypes.ImageFiles);
                if (!isValidProfileImage)
                {
                    result.AddError("Invalid profile image.");
                    return result;
                }
                var folderPath = _config["Folder:PartnersSenderProfile"];
                _fileProviderService.TryUploadFile(sender.ProfileImage, folderPath, out ProfileImagePath);
                //ProfileImagePath = await FileUploadHandler.UploadFile(_hostEnv, folderPath, sender.ProfileImage);
            }
            if (sender.IdFrontImg.Length > 0)
            {
                var (isValidIdFrontImage, _) = await FileValidatorUtils.IsValidImageAsync(sender.IdFrontImg, FileTypes.ImageFiles);
                if (!isValidIdFrontImage)
                {
                    result.AddError("Invalid ID front image.");
                    return result;
                }

                var folderPath = _config["Folder:PartnersSenderId"];
                _fileProviderService.TryUploadFile(sender.IdFrontImg, folderPath, out IDFrontImagePath);
                //IDFrontImagePath = await FileUploadHandler.UploadFile(_hostEnv, folderPath, sender.IdFrontImg);
            }
            if (sender.IdBackImg.Length > 0)
            {
                var (isValidIdBackImage, _) = await FileValidatorUtils.IsValidImageAsync(sender.IdBackImg, FileTypes.ImageFiles);
                if (!isValidIdBackImage)
                {
                    result.AddError("Invalid ID back image.");
                    return result;
                }

                var folderPath = _config["Folder:PartnersSenderId"];
                _fileProviderService.TryUploadFile(sender.IdBackImg, folderPath, out IDBackImagePath);
                //IDBackImagePath = await FileUploadHandler.UploadFile(_hostEnv, folderPath, sender.IdBackImg);
            }

            var mappeddata = _mapper.Map<SenderAddUpdateDto>(sender);
            mappeddata.PartnerCode = PartnerCode;
            mappeddata.ProfileImagePath = ProfileImagePath;
            mappeddata.IdFrontImgPath = IDFrontImagePath;
            mappeddata.IdBackImgPath = IDBackImagePath;
            mappeddata.Address = "static address here";

            var addSenderStatus = await _partnerSenderRepository.AddSenderAsync(mappeddata);

            //delete image in case of fail to save data
            if (addSenderStatus.StatusCode != 200)
            {
                if (!string.IsNullOrEmpty(ProfileImagePath))
                {
                    string imgPath = ProfileImagePath.Substring(1);
                    string existingImage = Path.Combine("" + _hostEnv.WebRootPath, imgPath);
                    System.IO.File.Delete(existingImage);
                }
                if (!string.IsNullOrEmpty(IDFrontImagePath))
                {
                    string imgPath = IDFrontImagePath.Substring(1);
                    string existingImage = Path.Combine("" + _hostEnv.WebRootPath, imgPath);
                    System.IO.File.Delete(existingImage);
                }
                if (!string.IsNullOrEmpty(IDBackImagePath))
                {
                    string imgPath = IDBackImagePath.Substring(1);
                    string existingImage = Path.Combine("" + _hostEnv.WebRootPath, imgPath);
                    System.IO.File.Delete(existingImage);
                }

                result.AddError(addSenderStatus.StatusCode, addSenderStatus.MsgText);
                return result;
            }

            result.AddSuccess(addSenderStatus.StatusCode, addSenderStatus.MsgText);
            return result;
        }

        public async Task<PagedList<SenderDto>> GetSenderListAsync(SenderPagedRequest request)
        {
            var SenderList = await _partnerSenderRepository.GetSendersAsync(request);
            return SenderList;
        }
        public async Task<SenderDto> GetSenderByIdAsync(int senderId,string PartnerCode)
        {
            var Sender = await _partnerSenderRepository.GetSenderByIdAsync(senderId,PartnerCode);
            return Sender;
        }

        public async Task<SprocMessage> RemoveSenderAsync(int senderId)
        {
            var PartnerCode = _loggedInUser.Claims.FirstOrDefault(x => x.Type == "PartnerCode")?.Value;
            var Sender = await _partnerSenderRepository.GetSenderByIdAsync(senderId, PartnerCode);
            if (Sender == null)
            {
                return new SprocMessage() { StatusCode = 400, MsgText = "User Not Found", MsgType = "Error", IdentityVal = 0 };
            }
            var mappeddata = _mapper.Map<SenderAddUpdateDto>(Sender);
            var response = await _partnerSenderRepository.RemoveSenderAsync(mappeddata);
            return response;
        }

        public async Task<MpmtResult> UpdateSenderAsync(UpdateUserVM sender)
        {
            var result = new MpmtResult();

            var PartnerCode = _loggedInUser.Claims.FirstOrDefault(x => x.Type == "PartnerCode")?.Value;
            var oldSender = await _partnerSenderRepository.GetSenderByIdAsync(sender.Id, PartnerCode);
            var ProfileImagePath = oldSender.ProfileImagePath;
            var IDBackImagePath = oldSender.IdBackImgPath;
            var IDFrontImagePath = oldSender.IdFrontImgPath;

            if (sender.ProfileImage is not null && sender.ProfileImage.Length > 0)
            {
                var (isValidProfileImage, _) = await FileValidatorUtils.IsValidImageAsync(sender.ProfileImage, FileTypes.ImageFiles);
                if (!isValidProfileImage)
                {
                    result.AddError("Invalid profile image.");
                    return result;
                }

                var folderPath = _config["Folder:PartnersSenderProfile"];
                _fileProviderService.TryUploadFile(sender.ProfileImage, folderPath, out ProfileImagePath);
                //ProfileImagePath = await FileUploadHandler.UploadFile(_hostEnv, folderPath, sender.ProfileImage);
            }

            if (sender.IdFrontImg is not null && sender.IdFrontImg.Length > 0)
            {
                var (isValidIdFrontImage, _) = await FileValidatorUtils.IsValidImageAsync(sender.IdFrontImg, FileTypes.ImageFiles);
                if (!isValidIdFrontImage)
                {
                    result.AddError("Invalid ID front image.");
                    return result;
                }

                var folderPath = _config["Folder:PartnersSenderId"];
                _fileProviderService.TryUploadFile(sender.IdFrontImg, folderPath, out IDFrontImagePath);
                //IDFrontImagePath = await FileUploadHandler.UploadFile(_hostEnv, folderPath, sender.IdFrontImg);
            }

            if (sender.IdBackImg is not null && sender.IdBackImg.Length > 0)
            {
                var (isValidIdBackImage, _) = await FileValidatorUtils.IsValidImageAsync(sender.IdBackImg, FileTypes.ImageFiles);
                if (!isValidIdBackImage)
                {
                    result.AddError("Invalid ID back image.");
                    return result;
                }

                var folderPath = _config["Folder:PartnersSenderId"];
                _fileProviderService.TryUploadFile(sender.IdBackImg, folderPath, out IDBackImagePath);
                //IDBackImagePath = await FileUploadHandler.UploadFile(_hostEnv, folderPath, sender.IdBackImg);
            }

            var mappeddata = _mapper.Map<SenderAddUpdateDto>(sender);
            mappeddata.PartnerCode = PartnerCode;
            mappeddata.ProfileImagePath = ProfileImagePath;
            mappeddata.IdFrontImgPath = IDFrontImagePath;
            mappeddata.IdBackImgPath = IDBackImagePath;
            mappeddata.Address = "static address here";

            var senderUpdateStatus = await _partnerSenderRepository.UpdateSenderAsync(mappeddata);

            //delete image in case of fail to save data 
            if (senderUpdateStatus.StatusCode != 200)
            {
                if (!string.IsNullOrEmpty(ProfileImagePath))
                {
                    string imgPath = ProfileImagePath.Substring(1);
                    string existingImage = Path.Combine("" + _hostEnv.WebRootPath, imgPath);
                    System.IO.File.Delete(existingImage);
                }
                if (!string.IsNullOrEmpty(IDFrontImagePath))
                {
                    string imgPath = IDFrontImagePath.Substring(1);
                    string existingImage = Path.Combine("" + _hostEnv.WebRootPath, imgPath);
                    System.IO.File.Delete(existingImage);
                }
                if (!string.IsNullOrEmpty(IDBackImagePath))
                {
                    string imgPath = IDBackImagePath.Substring(1);
                    string existingImage = Path.Combine("" + _hostEnv.WebRootPath, imgPath);
                    System.IO.File.Delete(existingImage);
                }

                result.AddError(senderUpdateStatus.StatusCode, senderUpdateStatus.MsgText);
                return result;
            }

            // if success, delete old image files
            if (sender.ProfileImage is not null && !string.IsNullOrEmpty(oldSender.ProfileImagePath))
            {
                string imgPath = oldSender.ProfileImagePath.Substring(1);
                string existingImage = Path.Combine("" + _hostEnv.WebRootPath, imgPath);
                File.Delete(existingImage);
            }

            if (sender.IdFrontImg is not null && !string.IsNullOrEmpty(oldSender.IdFrontImgPath))
            {
                string imgPath = oldSender.IdFrontImgPath.Substring(1);
                string existingImage = Path.Combine("" + _hostEnv.WebRootPath, imgPath);
                System.IO.File.Delete(existingImage);
            }

            if (sender.IdBackImg is not null && !string.IsNullOrEmpty(oldSender.IdBackImgPath))
            {
                string imgPath = oldSender.IdBackImgPath.Substring(1);
                string existingImage = Path.Combine("" + _hostEnv.WebRootPath, imgPath);
                File.Delete(existingImage);
            }

            result.AddSuccess(senderUpdateStatus.StatusCode, senderUpdateStatus.MsgText);
            return result;
        }

        public async Task<IEnumerable<ExistingSender>> GetExistingSendersByPartnercode(string PartnerCode, string MemberId, string FullName)
        {
            var ExistingSender = await _partnerSenderRepository.GetExistingSendersByPartnercodeAsync(PartnerCode, MemberId, FullName);
            return ExistingSender;
        }
        public async Task<IEnumerable<ExistingRecipients>> GetExistingRecipientsByPartnercode(string MemberId)
        {
            var ExistingRecipients = await _partnerSenderRepository.GetExistingRecipientsByPartnercodeAsync(MemberId);
            return ExistingRecipients;
        }

        public Task<MpmtResult> ValidateBulkTxnApiAsync(BulkTransactionDetailsModel bulkTransactionDetailsModel)
        {
            throw new NotImplementedException();
        }
    }
}
