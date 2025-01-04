using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Mpmt.Core.Common;
using Mpmt.Core.Dtos;
using Mpmt.Core.Dtos.Adjustment;
using Mpmt.Core.Dtos.FeeFundRequest;
using Mpmt.Core.Dtos.Paging;
using Mpmt.Core.Dtos.Partner;
using Mpmt.Core.Dtos.PartnerEmployee;
using Mpmt.Core.Dtos.Users;
using Mpmt.Core.Extensions;
using Mpmt.Core.Models.Mail;
using Mpmt.Core.ViewModel.ForgotPassword;
using Mpmt.Data.Common;
using Mpmt.Data.Repositories.Partner;
using Mpmt.Data.Repositories.PartnerEmployee;
using Mpmt.Services.Common;
using Mpmt.Services.Partner.IService;
using Mpmt.Services.Services.Common;
using Mpmt.Services.Services.MailingService;
using Mpmts.Core.Dtos;
using System.Security.Claims;

namespace Mpmt.Services.Partner.Service;

public class PartnerService : BaseService, IPartnerService
{
    private readonly IPartnerRepository _partnerRepo;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly IWebHelper _webHelper;
    private readonly IFileProviderService _fileProviderService;
    private readonly IMailService _mailService;
    private readonly IConfiguration _config;
    private readonly IWebHostEnvironment _hostEnv;
    private readonly IPartnerEmployeeRepo _partnerEmployeeRepo;
    private readonly IPartnerRepository _partnerRepository;

    public PartnerService(IPartnerRepository partnerRepo,
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        IWebHelper webHelper,
        IConfiguration config,
        IWebHostEnvironment hostEnv,
        IMailService mailService,
        IPartnerEmployeeRepo partnerEmployeeRepo,
        IPartnerRepository partnerRepository,
        IFileProviderService fileProviderService)
    {

        _partnerRepo = partnerRepo;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _webHelper = webHelper;
        _config = config;
        _hostEnv = hostEnv;
        _mailService = mailService;
        _partnerEmployeeRepo = partnerEmployeeRepo;
        _partnerRepository = partnerRepository;
        _fileProviderService = fileProviderService;
    }

    public async Task<MpmtResult> AddPartnerAsync(AppPartner user)
    {
        var addPartnerStatus = await _partnerRepo.AddPartnerAsync(user);

        return addPartnerStatus.MapToMpmtResult();
    }

    public async Task<bool> CheckPartnerExistsByEmailAsync(string email)
    {
        ArgumentNullException.ThrowIfNull(email, nameof(email));

        return await _partnerRepo.CheckPartnerExistsByEmailAsync(email);
    }

    public async Task<bool> CheckPartnerExistsByUserNameAsync(string userName)
    {
        ArgumentNullException.ThrowIfNull(userName, nameof(userName));

        return await _partnerRepo.CheckPartnerExistsByUserNameAsync(userName);
    }

    public async Task<AppPartner> GetPartnerByEmailAsync(string email)
    {
        ArgumentNullException.ThrowIfNull(email, nameof(email));

        return await _partnerRepo.GetPartnerByEmailAsync(email);
    }

    public string GenrateMailBodyForPasswordReset(string link)
    {
        var companyName = "MyPay Money Transfer Pvt. Ltd.";
        var companyEmail = "info@MPMT.com";

        string mailBody =
            $@"
                <p>We got a request to reset your Password.</p>
                
                <P>Tap to redirect to Password reset link: <a href=""{link}"">{link}</a><p>
                            
                <p>If you ignore this message, your password will not be changed. If you didn't request a password reset, let us know.</p>
                <p>ATTN : Please do not reply to this email. This mailbox is not monitored and you will not receive a response.</p>
                <p>{companyName},<br>
                Radhe Radhe Bhaktapur, Nepal.<br>
                {companyEmail}</p>

                <p>Warm Regards,<br>
                {companyName}</p>

                <p>CONFIDENTIALITY NOTICE: This transmittal is a confidential communication or may otherwise be privileged. If you are not the intended recipient, you are hereby notified that you have received this transmittal in error and that any review, dissemination, distribution or copying of this transmittal is strictly prohibited. If you have received this communication in error, please notify this office, and immediately delete this message and all its attachments, if any.</p>";

        return mailBody;
    }

    public async Task SendPasswordResetList(string link, string emails)
    {
        var baseurl = _webHelper.GetbaseUrl();
        var mailRequest = new MailRequestModel
        {
            MailFor = "password-reset",
            MailTo = emails,
            MailSubject = "Password Reset",
            RecipientName = "",
            Content = GenrateMailBodyForPasswordReset($"{baseurl}/partner/login/passwordreset?token=" + link)
        };
        var mailServiceModel = await _mailService.EmailSettings(mailRequest);
        Thread email = new(delegate ()
        {
            _mailService.SendMail(mailServiceModel);
        });
        email.Start();
    }

    public async Task<SprocMessage> ForgotPasswordAsync(PasswordResetModel reset)
    {
        var UsersStatus = await _partnerRepo.ForgotPasswordAsync(reset);
        return UsersStatus;
    }

    public async Task<(SprocMessage, PasswordResetModel)> RequestTokenValidationAsync(string token)
    {
        var UsersStatus = await _partnerRepo.RequestTokenValidationAsync(token);
        return UsersStatus;
    }

    public async Task<(SprocMessage, PasswordResetModel)> ResetTokenValidationAsync(PasswordResetModel resetModel)
    {
        var UsersStatus = await _partnerRepo.ResetTokenValidationAsync(resetModel);
        return UsersStatus;
    }

    public async Task<SprocMessage> ResetPasswordAsync(PasswordResetModel request)
    {
        var response = new SprocMessage()
        {
            StatusCode = 400,
            MsgText = "Something Went wrong",
        };
        var partnerId = request.UserId;
        var userType = request.UserType;
        var UserName = request.UserName;
        var PartnerCode = request.PartnerCode;
        if (userType == "PARTNER")
        {
            var passwordSalt = CryptoUtils.GenerateKeySalt();
            var passwordHash = CryptoUtils.HashHmacSha512Base64(request.NewPassword, passwordSalt);

            var requests = new AppPartner()
            {
                Event = "CP",
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Id = partnerId,
                PartnerCode = PartnerCode,
                UpdatedById = partnerId,
                UpdatedByName = UserName

            };
            response = await _partnerRepository.PartnerChangePasswordAsync(requests);
            return response;
        }
        if (userType == "EMPLOYEE")
        {
            var passwordSalt = CryptoUtils.GenerateKeySalt();
            var passwordHash = CryptoUtils.HashHmacSha512Base64(request.NewPassword, passwordSalt);

            var requests = new IUDPartnerEmployee()
            {
                Event = "CP",
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Id = partnerId,
                PartnerCode = PartnerCode,
                UpdatedById = partnerId,
                UpdatedByName = UserName
            };
            response = await _partnerEmployeeRepo.PartnerEmployeeChangePasswordAsync(requests);
            return response;
        }
        response.StatusCode = 400;
        response.MsgText = "Permission Denied";
        return response;
    }

    public async Task<AppPartner> GetPartnerByIdAsync(int id)
    {

        if (id < 0) return null;

        return await _partnerRepo.GetPartnerByIdAsync(id);
    }

    public Task<AppPartner> GetPartnerByUserNameAsync(string userName)
    {
        ArgumentNullException.ThrowIfNull(userName, nameof(userName));

        return _partnerRepo.GetPartnerByUserNameAsync(userName);
    }

    public Task UpdatePartnerLoginActivityAsync(UserLoginActivity loginActivity)
    {
        ArgumentNullException.ThrowIfNull(loginActivity, nameof(loginActivity));

        return _partnerRepo.UpdatePartnerLoginActivityAsync(loginActivity);
    }

    public void UpdateAccountSecretKey(string email, string accountsecretkey)
    {
        _partnerRepo.UpdateAccountSecretKey(email, accountsecretkey);
    }

    public async Task<PagedList<PartnerDetails>> GetPartnerListAsync(PartnerFilter partnerFilter)
    {
        var data = await _partnerRepo.GetPartnerList(partnerFilter);
        return data;
    }

    public async Task<IEnumerable<PartnerDirectors>> GetPartnerdirectorsListAsync(string PartnerCode)
    {
        var data = await _partnerRepo.GetPartnerdirectors(PartnerCode);
        return data;
    }

    public async Task<MpmtResult> UpdatePartnerAsync(UpdatePartnerrequest user)
    {
        var result = new MpmtResult();
        var mappeddata = _mapper.Map<AppPartner>(user);
        var partner = await _partnerRepo.GetPartnerByIdAsync(user.Id);
        if (partner is null)
        {
            result.AddError(400, "Partner not found.");
            return result;
        }

        //save image
        var ComapnayLogoImagePath = partner.CompanyLogoImgPath;
        var LicenseDocumentImagePath = string.Empty;
        var IDBackImagePath = partner.IdBackImgPath;
        var IDFrontImagePath = partner.IdFrontImgPath;
        var AddressProofImagePath = partner.AddressProofImgPath;
        var licenseImagesuploaded = new List<string>();
        if (user.CompanyLogo is not null && user.CompanyLogo.Length > 0)
        {
            var (isValidCompanyLogo, _) = await FileValidatorUtils.IsValidImageAsync(user.CompanyLogo, FileTypes.ImageFiles);
            if (!isValidCompanyLogo)
            {
                result.AddError(400, "Invalid company logo image.");
                return result;
            }

            var folderPath = _config["Folder:PartnersCompanylogo"];
            _fileProviderService.TryUploadFile(user.CompanyLogo, folderPath, out ComapnayLogoImagePath);
            //ComapnayLogoImagePath = await FileUploadHandler.UploadFile(_hostEnv, folderPath, user.CompanyLogo);
        }
        if (user.LicenseDocument != null && user.LicenseDocument.Count > 0)
        {
            foreach (var item in user.LicenseDocument)
            {
                if (item is not null && item.Length > 0)
                {
                    var (isValidLicenseDocument, _) = await FileValidatorUtils.IsValidImageAsync(item, FileTypes.ImageFiles);
                    if (!isValidLicenseDocument)
                    {
                        result.AddError(400, "Invalid license document image.");

                    }

                    var folderPath = _config["Folder:PartnersLicenseDocument"];
                    _fileProviderService.TryUploadFile(item, folderPath, out LicenseDocumentImagePath);
                    //LicenseDocumentImagePath = await FileUploadHandler.UploadFile(_hostEnv, folderPath, item);
                    licenseImagesuploaded.Add(LicenseDocumentImagePath);
                }
            }
            if (result.Errors.Count > 0)
            {
                return result;
            }
        }
        mappeddata.LicensedocImgPath = licenseImagesuploaded;
        if (user.LicensedocImgPath is not null && user.LicensedocImgPath.Count > 0)
        {
            foreach (var licenseimgPath in user.LicensedocImgPath)
            {
                mappeddata.LicensedocImgPath.Add(licenseimgPath);
            }
        }


        if (user.IDBack is not null)
        {
            var (isValidIDBackImage, _) = await FileValidatorUtils.IsValidImageAsync(user.IDBack, FileTypes.ImageFiles);
            if (!isValidIDBackImage)
            {
                result.AddError(400, "Invalid ID back image.");
                return result;
            }

            var folderPath = _config["Folder:PartnersIdImage"];
            _fileProviderService.TryUploadFile(user.IDBack, folderPath, out IDBackImagePath);
            //IDBackImagePath = await FileUploadHandler.UploadFile(_hostEnv, folderPath, user.IDBack);
        }

        if (user.IDFront is not null && user.IDFront.Length > 0)
        {
            var (isValidIDFrontImage, _) = await FileValidatorUtils.IsValidImageAsync(user.IDFront, FileTypes.ImageFiles);
            if (!isValidIDFrontImage)
            {
                result.AddError(400, "Invalid ID front image.");
                return result;
            }

            var folderPath = _config["Folder:PartnersIdImage"];
            _fileProviderService.TryUploadFile(user.IDFront, folderPath, out IDFrontImagePath);
            //IDFrontImagePath = await FileUploadHandler.UploadFile(_hostEnv, folderPath, user.IDFront);
        }

        if (user.AddressProfImage is not null && user.AddressProfImage.Length > 0)
        {
            var (isValidAddressProofImage, _) = await FileValidatorUtils.IsValidImageAsync(user.AddressProfImage, FileTypes.ImageFiles);
            if (!isValidAddressProofImage)
            {
                result.AddError(400, "Invalid ID address proof image.");
                return result;
            }

            var folderPath = _config["Folder:PartnersAddressProof"];
            _fileProviderService.TryUploadFile(user.AddressProfImage, folderPath, out AddressProofImagePath);
            //AddressProofImagePath = await FileUploadHandler.UploadFile(_hostEnv, folderPath, user.AddressProfImage);
        }

        mappeddata.GenderId = 1;
        mappeddata.Event = "U";
        mappeddata.CompanyLogoImgPath = ComapnayLogoImagePath;
        mappeddata.GMTTimeZoneId = user.GMTTimeZoneId;
        mappeddata.IdBackImgPath = IDBackImagePath;
        mappeddata.IdFrontImgPath = IDFrontImagePath;
        mappeddata.AddressProofImgPath = AddressProofImagePath;
        mappeddata.IsFirstNamePresent = !user.IsFirstNamePresent;
        mappeddata.MobileNumber = user.CallingCode + "-" + user.MobileNumber;

        var addPartnerResult = await _partnerRepo.AddPartnerAsync(mappeddata);
        if (addPartnerResult.StatusCode != 200)
        {
            //delete saved image
            if (user.CompanyLogoImgPath is not null && !string.IsNullOrEmpty(ComapnayLogoImagePath))
            {
                string imgPath = ComapnayLogoImagePath.Substring(1);
                string existingImage = Path.Combine("" + _hostEnv.WebRootPath, imgPath);
                File.Delete(existingImage);
            }

            if (licenseImagesuploaded is not null && licenseImagesuploaded.Count > 0)
            {
                foreach (var img in licenseImagesuploaded)
                {
                    string imgPath = img.Substring(1);
                    string existingImage = Path.Combine("" + _hostEnv.WebRootPath, imgPath);
                    File.Delete(existingImage);
                }

            }

            if (user.IDBack is not null && !string.IsNullOrEmpty(IDBackImagePath))
            {
                string imgPath = IDBackImagePath.Substring(1);
                string existingImage = Path.Combine("" + _hostEnv.WebRootPath, imgPath);
                File.Delete(existingImage);
            }


            if (user.IDFront is not null && !string.IsNullOrEmpty(IDFrontImagePath))
            {
                string imgPath = IDFrontImagePath.Substring(1);
                string existingImage = Path.Combine("" + _hostEnv.WebRootPath, imgPath);
                File.Delete(existingImage);
            }

            if (user.AddressProfImage is not null && !string.IsNullOrEmpty(AddressProofImagePath))
            {
                string imgPath = AddressProofImagePath.Substring(1);
                string existingImage = Path.Combine("" + _hostEnv.WebRootPath, imgPath);
                File.Delete(existingImage);
            }

            result.AddError(addPartnerResult.StatusCode, addPartnerResult.MsgText);
            return result;
        }

        //delete old image only when new image
        if (!string.IsNullOrEmpty(partner.CompanyLogoImgPath) && user.CompanyLogo is not null)
        {
            string imgPath = partner.CompanyLogoImgPath.Substring(1);
            string existingImage = Path.Combine("" + _hostEnv.WebRootPath, imgPath);
            File.Delete(existingImage);
        }
        if (user.DeletedLicensedocImgPath is not null && user.DeletedLicensedocImgPath.Count > 0)
        {
            foreach (var image in user.DeletedLicensedocImgPath)
            {
                if (!string.IsNullOrEmpty(image))
                {
                    string imgPath = image.Substring(1);
                    string existingImage = Path.Combine("" + _hostEnv.WebRootPath, imgPath);
                    System.IO.File.Delete(existingImage);
                }
            }
        }

        if (!string.IsNullOrEmpty(partner.IdBackImgPath) && user.IDBack is not null)
        {
            string imgPath = partner.IdBackImgPath.Substring(1);
            string existingImage = Path.Combine("" + _hostEnv.WebRootPath, imgPath);
            File.Delete(existingImage);
        }

        if (!string.IsNullOrEmpty(partner.IdFrontImgPath) && user.IDFront is not null)
        {
            string imgPath = partner.IdFrontImgPath.Substring(1);
            string existingImage = Path.Combine("" + _hostEnv.WebRootPath, imgPath);
            File.Delete(existingImage);
        }
        if (!string.IsNullOrEmpty(partner.AddressProofImgPath) && user.AddressProfImage is not null)
        {
            string imgPath = partner.AddressProofImgPath.Substring(1);
            string existingImage = Path.Combine("" + _hostEnv.WebRootPath, imgPath);
            File.Delete(existingImage);
        }

        result.AddSuccess(addPartnerResult.StatusCode, addPartnerResult.MsgText);
        return result;
    }

    public async Task<SprocMessage> UpdateRemitPartnerStatusAsync(bool action, int Id)
    {
        var partner = await _partnerRepo.GetPartnerByIdAsync(Id);
        var data = new PartnerUpdateStatus()
        {
            PartnerCode = partner.PartnerCode,
            IsActive = action,

        };

        var response = await _partnerRepo.UpdateStatusAsync(data);
        return response;
    }

    public async Task<MpmtResult> AddPartnerAsync(AddPatnerRequest addPartner)
    {
        var result = new MpmtResult();
        var mappeddata = _mapper.Map<AppPartner>(addPartner);
        mappeddata.LicensedocImgPath = new List<string>();
        //create password hash
        var passwordSalt = CryptoUtils.GenerateKeySalt();
        var passwordHash = CryptoUtils.HashHmacSha512Base64(addPartner.Password, passwordSalt);

        //save image and get its path
        var ComapnayLogoImagePath = string.Empty;
        var LicenseDocumentImagePath = string.Empty;
        var IDBackImagePath = string.Empty;
        var IDFrontImagePath = string.Empty;
        var AddressProofImagePath = string.Empty;
        if (addPartner.CompanyLogo is not null && addPartner.CompanyLogo.Length > 0)
        {
            var (isValidCompanyLogo, _) = await FileValidatorUtils.IsValidImageAsync(addPartner.CompanyLogo, FileTypes.ImageFiles);
            if (!isValidCompanyLogo)
            {
                result.AddError(400, "Invalid company logo image.");
                return result;
            }

            var folderPath = _config["Folder:PartnersCompanylogo"];
            _fileProviderService.TryUploadFile(addPartner.CompanyLogo, folderPath, out ComapnayLogoImagePath);
            //ComapnayLogoImagePath = await FileUploadHandler.UploadFile(_hostEnv, folderPath, addPartner.CompanyLogo);
        }
        if (addPartner.IDBack is not null && addPartner.IDBack.Length > 0)
        {
            var (isValidIDBackImage, _) = await FileValidatorUtils.IsValidImageAsync(addPartner.IDBack, FileTypes.ImageFiles);
            if (!isValidIDBackImage)
            {
                result.AddError(400, "Invalid ID back image.");
                return result;
            }

            var folderPath = _config["Folder:PartnersIdImage"];
            _fileProviderService.TryUploadFile(addPartner.IDBack, folderPath, out IDBackImagePath);
            //IDBackImagePath = await FileUploadHandler.UploadFile(_hostEnv, folderPath, addPartner.IDBack);
        }
        if (addPartner.IDFront is not null && addPartner.IDFront.Length > 0)
        {
            var (isValidIDFrontImage, _) = await FileValidatorUtils.IsValidImageAsync(addPartner.IDFront, FileTypes.ImageFiles);
            if (!isValidIDFrontImage)
            {
                result.AddError(400, "Invalid ID front image.");
                return result;
            }

            var folderPath = _config["Folder:PartnersIdImage"];
            _fileProviderService.TryUploadFile(addPartner.IDFront, folderPath, out IDFrontImagePath);
            //IDFrontImagePath = await FileUploadHandler.UploadFile(_hostEnv, folderPath, addPartner.IDFront);
        }
        if (addPartner.AddressProfImage is not null && addPartner.AddressProfImage.Length > 0)
        {
            var (isValidAddressProofImage, _) = await FileValidatorUtils.IsValidImageAsync(addPartner.AddressProfImage, FileTypes.ImageFiles);
            if (!isValidAddressProofImage)
            {
                result.AddError(400, "Invalid ID address proof image.");
                return result;
            }

            var folderPath = _config["Folder:PartnersAddressProof"];
            _fileProviderService.TryUploadFile(addPartner.AddressProfImage, folderPath, out AddressProofImagePath);
            //AddressProofImagePath = await FileUploadHandler.UploadFile(_hostEnv, folderPath, addPartner.AddressProfImage);
        }
        if (addPartner.LicenseDocument != null & addPartner.LicenseDocument.Count > 0)
        {
            foreach (var image in addPartner.LicenseDocument)
            {
                var (isvalidlicenseImage, _) = await FileValidatorUtils.IsValidImageAsync(image, FileTypes.ImageFiles);
                if (!isvalidlicenseImage)
                {
                    result.AddError(400, "Invalid ID License image.");
                    continue;
                }

                var folderPath = _config["Folder:PartnersLicenseDocument"];
                _fileProviderService.TryUploadFile(image, folderPath, out LicenseDocumentImagePath);
                //LicenseDocumentImagePath = await FileUploadHandler.UploadFile(_hostEnv, folderPath, image);
                mappeddata.LicensedocImgPath.Add(LicenseDocumentImagePath);

            }
            if (result.Errors.Count > 0)
            {
                return result;
            }
        }

        mappeddata.CompanyLogoImgPath = ComapnayLogoImagePath;
        //mappeddata.LicensedocImgPath = LicenseDocumentImagePath;
        mappeddata.IdBackImgPath = IDBackImagePath;
        mappeddata.IdFrontImgPath = IDFrontImagePath;
        mappeddata.AddressProofImgPath = AddressProofImagePath;
        mappeddata.IsFirstNamePresent = !addPartner.IsFirstNamePresent;
        mappeddata.FirstName = addPartner.FirstName;
        mappeddata.GMTTimeZoneId = addPartner.GMTTimeZoneId;
        mappeddata.ShortName = addPartner.Shortname;
        mappeddata.SurName = addPartner.LastName;
        mappeddata.IsFirstNamePresent = !addPartner.ContinueWithoutFirstName;
        mappeddata.MobileNumber = addPartner.CallingCode + "-" + addPartner.Number;
        //mappeddata.Address = addPartner.FullAddress;
        mappeddata.UserName = addPartner.Username;
        mappeddata.CreditUptoLimitPerc = addPartner.CreditSendTxnLimitPercent;
        mappeddata.CreditSendTxnLimit = addPartner.CreditSendTxnLimit;
        mappeddata.CashPayoutSendTxnLimit = addPartner.SendTxnLimitCashPayout;
        mappeddata.WalletSendTxnLimit = addPartner.SendTxnLimitWallet;
        mappeddata.BankSendTxnLimit = addPartner.SendTxnLimitBank;
        mappeddata.TransactionApproval = addPartner.IsTxnApproveActive;
        mappeddata.OrganizationName = addPartner.OrganizationName;
        mappeddata.OrgEmail = addPartner.CompanyEmail;
        mappeddata.CountryCode = addPartner.Country;
        mappeddata.City = addPartner.City;
        mappeddata.FullAddress = addPartner.FullAddress;
        mappeddata.GMTTimeZone = addPartner.GMTTimeZone;
        mappeddata.RegistrationNumber = addPartner.CompanyRegistrationNumber;
        mappeddata.SourceCurrency = addPartner.LocalSourceCurrency;
        mappeddata.ChargeCategoryId = addPartner.Service;
        mappeddata.FundType = addPartner.BalanceType;
        mappeddata.GenderId = 1;
        mappeddata.Directors = addPartner.Directors;
        //mappeddata.Website
        mappeddata.IpAddress = addPartner.IPAddress;
        mappeddata.DocumentTypeId = addPartner.DocumentType;
        mappeddata.DocumentNumber = addPartner.DocumentNumber;
        mappeddata.ExpiryDate = addPartner.ExpiryDate;
        mappeddata.IssueDate = addPartner.IssueDate;
        mappeddata.AddressProofTypeId = addPartner.AddressProfType;
        mappeddata.PasswordHash = passwordHash;
        mappeddata.PasswordSalt = passwordSalt;
        mappeddata.IsActive = addPartner.IsActive;

        //Create operation
        mappeddata.Event = "I";

        var addPartnerResult = await _partnerRepo.AddPartnerAsync(mappeddata);

        //delete image in case of fail to save data 
        if (addPartnerResult.StatusCode != 200)
        {
            if (!string.IsNullOrEmpty(ComapnayLogoImagePath))
            {
                string imgPath = ComapnayLogoImagePath.Substring(1);
                string existingImage = Path.Combine("" + _hostEnv.WebRootPath, imgPath);
                File.Delete(existingImage);
            }
            if (!string.IsNullOrEmpty(LicenseDocumentImagePath))
            {
                string imgPath = LicenseDocumentImagePath.Substring(1);
                string existingImage = Path.Combine("" + _hostEnv.WebRootPath, imgPath);
                File.Delete(existingImage);
            }
            if (!string.IsNullOrEmpty(IDBackImagePath))
            {
                string imgPath = IDBackImagePath.Substring(1);
                string existingImage = Path.Combine("" + _hostEnv.WebRootPath, imgPath);
                File.Delete(existingImage);
            }
            if (!string.IsNullOrEmpty(IDFrontImagePath))
            {
                string imgPath = IDFrontImagePath.Substring(1);
                string existingImage = Path.Combine("" + _hostEnv.WebRootPath, imgPath);
                File.Delete(existingImage);
            }
            if (!string.IsNullOrEmpty(AddressProofImagePath))
            {
                string imgPath = AddressProofImagePath.Substring(1);
                string existingImage = Path.Combine("" + _hostEnv.WebRootPath, imgPath);
                File.Delete(existingImage);
            }
            if (mappeddata.LicensedocImgPath != null & mappeddata.LicensedocImgPath.Count > 0)
            {
                foreach (var img in mappeddata.LicensedocImgPath)
                {
                    string imgPath = img.Substring(1);
                    string existingImage = Path.Combine("" + _hostEnv.WebRootPath, imgPath);
                    File.Delete(existingImage);
                }
            }

            result.AddError(addPartnerResult.StatusCode, addPartnerResult.MsgText);
            return result;
        }

        result.AddSuccess(addPartnerResult.StatusCode, addPartnerResult.MsgText);
        return result;
    }

    public async Task<SprocMessage> DeletePartnerAsync(AppPartner Partner)
    {
        //Delete operation
        Partner.Event = "D";

        var response = await _partnerRepo.AddPartnerAsync(Partner);
        //incase of success delete all old image 
        if (response.StatusCode == 200)
        {
            if (!string.IsNullOrEmpty(Partner.CompanyLogoImgPath))
            {
                string imgPath = Partner.CompanyLogoImgPath.Substring(1);
                string existingImage = Path.Combine("" + _hostEnv.WebRootPath, imgPath);
                System.IO.File.Delete(existingImage);
            }
            if (!string.IsNullOrEmpty(Partner.IdBackImgPath))
            {
                string imgPath = Partner.IdBackImgPath.Substring(1);
                string existingImage = Path.Combine("" + _hostEnv.WebRootPath, imgPath);
                System.IO.File.Delete(existingImage);
            }
            if (!string.IsNullOrEmpty(Partner.IdFrontImgPath))
            {
                string imgPath = Partner.IdFrontImgPath.Substring(1);
                string existingImage = Path.Combine("" + _hostEnv.WebRootPath, imgPath);
                System.IO.File.Delete(existingImage);
            }
            if (!string.IsNullOrEmpty(Partner.AddressProofImgPath))
            {
                string imgPath = Partner.AddressProofImgPath.Substring(1);
                string existingImage = Path.Combine("" + _hostEnv.WebRootPath, imgPath);
                System.IO.File.Delete(existingImage);
            }
        }
        return response;
    }

    public async Task<bool> VerifyUserName(string userName)
    {
        var response = await _partnerRepo.VerifyUserNameAsync(userName);
        return response;
    }

    public async Task<bool> VerifyEmail(string Email)
    {
        var response = await _partnerRepo.VerifyEmailAsync(Email);
        return response;
    }

    public async Task<bool> VerifyShortname(string Shortname)
    {
        var response = await _partnerRepo.VerifyShortnameAsync(Shortname);
        return response;
    }

    public async Task<IEnumerable<FeeAccount>> GetFeeAccountAsync(string partnerCode, string sourceCurrency)
    {
        var data = await _partnerRepo.GetFeeAccountAsync(partnerCode, sourceCurrency);
        return data;
    }

    public async Task<SprocMessage> AddLoginOtpAsync(TokenVerification tokenVerification)
    {
        var response = await _partnerRepo.AddLoginOtpAsync(tokenVerification);
        return response;
    }

    public async Task<TokenVerification> GetOtpBypartnerCodeAsync(string partnercode, string UserName, string OtpVerificationFor)
    {
        var data = await _partnerRepo.GetOtpBypartnerCodeAsync(partnercode, UserName, OtpVerificationFor);
        return data;
    }

    public async void UpdateEmailConfirmAsync(string partnercode)
    {
        _partnerRepo.UpdateEmailConfirmAsync(partnercode);

    }

    public async Task<bool> VerifyEmailConfirmed(string Email)
    {
        var response = await _partnerRepo.VerifyEmailConfirmed(Email);
        return response;
    }

    public async Task<AppPartner> GetPartnerEmployeeByEmailAsync(string email)
    {
        return await _partnerRepo.GetPartnerEmployeeByEmailAsync(email);
    }

    public async Task<SprocMessage> PartnerWalletAdjustment(AdjustmentWallet adjustment, ClaimsPrincipal claim)
    {
        var mappedData = _mapper.Map<AdjustmentWalletDTO>(adjustment);
        mappedData.OperationMode = 'A';
        mappedData.LoggedInUser = claim?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
        mappedData.UserType = claim?.Claims.FirstOrDefault(x => x.Type == "UserType")?.Value;
        var response = await _partnerRepo.PartnerWalletAdjustment(mappedData);
        return response;
    }
}
