using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Mpmt.Core.Common;
using Mpmt.Core.Dtos;
using Mpmt.Core.Dtos.Partner;
using Mpmt.Data.Common;
using Mpmt.Data.Repositories.Partner.IRepository;
using Mpmt.Services.Common;
using Mpmt.Services.Partner.IService;
using Mpmts.Core.Dtos;
using System.Security.Claims;

namespace Mpmt.Services.Partner.Service
{
    /// <summary>
    /// The partner wallet currency services.
    /// </summary>
    public class PartnerWalletCurrencyServices : IPartnerWalletCurrencyServices
    {
        private readonly IPartnerWalletCurrencyRepo _walletCurrencyRepo;


        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _config;
        private readonly IFileProviderService _fileProviderService; 
        private readonly IWebHostEnvironment _hostEnv;
        /// <summary>
        /// Initializes a new instance of the <see cref="PartnerWalletCurrencyServices"/> class.
        /// </summary>
        /// <param name="walletCurrencyRepo">The wallet currency repo.</param>
        /// <param name="httpContextAccessor">The http context accessor.</param>
        /// <param name="config">The config.</param>
        /// <param name="hostEnv">The host env.</param>
        public PartnerWalletCurrencyServices(IPartnerWalletCurrencyRepo walletCurrencyRepo, IHttpContextAccessor httpContextAccessor,
            IConfiguration config, IWebHostEnvironment hostEnv, IFileProviderService fileProviderService)

        {
            _walletCurrencyRepo = walletCurrencyRepo;
            _httpContextAccessor = httpContextAccessor;
            _config = config;
            _hostEnv = hostEnv;
            _fileProviderService = fileProviderService;
        }

        /// <summary>
        /// Adds the fund request async.
        /// </summary>
        /// <param name="addUpdateFundRequest">The add update fund request.</param>
        /// <param name="user">The user.</param>
        /// <returns>A Task.</returns>
        public async Task<MpmtResult> AddFundRequestAsync(AddUpdateFundRequest addUpdateFundRequest, ClaimsPrincipal user)
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

                var folderPath = _config["Folder:FundRequestVoucherImage"];
                _fileProviderService.TryUploadFile(addUpdateFundRequest.VoucherImg, folderPath, out imagePath);
                //imagePath = await FileUploadHandler.UploadFile(_hostEnv, folderPath, addUpdateFundRequest.VoucherImg);
                addUpdateFundRequest.VoucherImgPath = imagePath;
            }

            var addFundStatus = await _walletCurrencyRepo.AddUpdateFundRequestAsync(addUpdateFundRequest);
            if (addFundStatus.StatusCode != 200)
            {
                // Delete image file for failed operations
                if (!string.IsNullOrEmpty(imagePath))
                {
                    var imageToDeletePath = Path.Combine("" + _hostEnv.WebRootPath, imagePath[1..]);
                    File.Delete(imageToDeletePath);
                }

                result.AddError(addFundStatus.StatusCode, addFundStatus.MsgText);
                return result;
            }

            result.AddSuccess(addFundStatus.StatusCode, addFundStatus.MsgText);
            return result;
        }

        /// <summary>
        /// Updates the fund request async.
        /// </summary>
        /// <param name="addUpdateFundRequest">The add update fund request.</param>
        /// <param name="user">The user.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> UpdateFundRequestAsync(AddUpdateFundRequest addUpdateFundRequest, ClaimsPrincipal user)
        {
            if (addUpdateFundRequest.Id < 0)
            {
                return new SprocMessage { StatusCode = 400, MsgText = "Id cann't be null or less then zero", MsgType = "Error", IdentityVal = 0 };
            }

            addUpdateFundRequest.Event = 'U';
            addUpdateFundRequest.LoggedInUser = user?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            addUpdateFundRequest.UserType = user?.Claims.FirstOrDefault(x => x.Type == "UserType")?.Value;
            var response = await _walletCurrencyRepo.AddUpdateFundRequestAsync(addUpdateFundRequest);
            return response;
        }

        /// <summary>
        /// Adds the wallet currency async.
        /// </summary>
        /// <param name="addUpdateWalletcurrency">The add update walletcurrency.</param>
        /// <param name="user">The user.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> AddWalletCurrencyAsync(IUDUpdateWalletCurrency addUpdateWalletcurrency, ClaimsPrincipal user)
        {



            addUpdateWalletcurrency.OperationMode = "A";
            addUpdateWalletcurrency.LoggedInUser = user?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            addUpdateWalletcurrency.UserType = user?.Claims.FirstOrDefault(x => x.Type == "UserType")?.Value;
            var response = await _walletCurrencyRepo.AddUpdateWalletCurrencyAsync(addUpdateWalletcurrency);

            return response;
        }

        public async Task<SprocMessage> AddFeeWalletAsync(IUDUpdateWalletCurrency addUpdateWalletcurrency, ClaimsPrincipal user)
        {
            addUpdateWalletcurrency.OperationMode = "A";
            addUpdateWalletcurrency.LoggedInUser = user?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            addUpdateWalletcurrency.UserType = user?.Claims.FirstOrDefault(x => x.Type == "UserType")?.Value;
            var response = await _walletCurrencyRepo.AddUpdateFeeWalletAsync(addUpdateWalletcurrency);
            return response;
        }

        /// <summary>
        /// Gets the partner wallet currency.
        /// </summary>
        /// <param name="partnercode">The partnercode.</param>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<WalletCurrencyDetails>> GetPartnerWalletCurrency(string partnercode)
        {
            var data = await _walletCurrencyRepo.GetPartnerWalletCurrency(partnercode);
            return data;
        }

        /// <summary>
        /// Gets the partner wallet currency by id.
        /// </summary>
        /// <param name="Id">The id.</param>
        /// <returns>A Task.</returns>
        public async Task<WalletCurrencyDetails> GetPartnerWalletCurrencyById(int Id)
        {
            var data = await _walletCurrencyRepo.GetPartnerWalletCurrencyById(Id);
            return data;
        }

        public async Task<WalletCurrencyDetails> GetFeeWalletCurrencyById(int Id)
        {
            var data = await _walletCurrencyRepo.GetFeeWalletCurrencyById(Id);
            return data;
        }

        /// <summary>
        /// Removes the wallet currency async.
        /// </summary>
        /// <param name="addUpdateWalletcurrency">The add update walletcurrency.</param>
        /// <param name="user">The user.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> RemoveWalletCurrencyAsync(IUDUpdateWalletCurrency addUpdateWalletcurrency, ClaimsPrincipal user)
        {
            addUpdateWalletcurrency.LoggedInUser = user?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            addUpdateWalletcurrency.UserType = user?.Claims.FirstOrDefault(x => x.Type == "UserType")?.Value;
            var response = await _walletCurrencyRepo.RemoveWalletCurrencyAsync(addUpdateWalletcurrency);
            return response;
        }

        /// <summary>
        /// Updates the wallet currency async.
        /// </summary>
        /// <param name="addUpdateWalletcurrency">The add update walletcurrency.</param>
        /// <param name="user">The user.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> UpdateWalletCurrencyAsync(IUDUpdateWalletCurrency addUpdateWalletcurrency, ClaimsPrincipal user)
        {
            if (addUpdateWalletcurrency.Id < 0)
            {
                return new SprocMessage { StatusCode = 400, MsgText = "Id cann't be null or less then zero", MsgType = "Error", IdentityVal = 0 };
            }
            addUpdateWalletcurrency.OperationMode = "U";
            addUpdateWalletcurrency.LoggedInUser = user?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            addUpdateWalletcurrency.UserType = user?.Claims.FirstOrDefault(x => x.Type == "UserType")?.Value;
            var response = await _walletCurrencyRepo.AddUpdateWalletCurrencyAsync(addUpdateWalletcurrency);
            return response;
        }

        public async Task<SprocMessage> UpdateFeeWalletAsync(IUDUpdateWalletCurrency addUpdateWalletcurrency, ClaimsPrincipal user)
        {
            if (addUpdateWalletcurrency.Id < 0)
            {
                return new SprocMessage { StatusCode = 400, MsgText = "Id cann't be null or less then zero", MsgType = "Error", IdentityVal = 0 };
            }
            addUpdateWalletcurrency.OperationMode = "U";
            addUpdateWalletcurrency.LoggedInUser = user?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            addUpdateWalletcurrency.UserType = user?.Claims.FirstOrDefault(x => x.Type == "UserType")?.Value;
            var response = await _walletCurrencyRepo.AddUpdateFeeWalletAsync(addUpdateWalletcurrency);
            return response;
        }

        public async Task<IEnumerable<WalletCurrencyBalance>> GetPartnerWalletCurrencyBalance(string partnercode)
        {
            var data = await _walletCurrencyRepo.GetPartnerWalletCurrencyBalance(partnercode);
            return data;
        }

        public async Task<SprocMessage> AddFeeBalanceAsync(AddUpdateFundRequest addUpdateFundRequest, ClaimsPrincipal user)
        {
            addUpdateFundRequest.Event = 'A';
            addUpdateFundRequest.LoggedInUser = user?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            addUpdateFundRequest.UserType = user?.Claims.FirstOrDefault(x => x.Type == "UserType")?.Value;

            var response = await _walletCurrencyRepo.AddFeeBalanceAsync(addUpdateFundRequest);
            return response;
        }
    }
}
