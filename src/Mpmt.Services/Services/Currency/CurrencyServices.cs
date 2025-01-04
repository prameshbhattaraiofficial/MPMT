using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Mpmt.Core.Dtos.Currency;
using Mpmt.Core.ViewModel.Currency;
using Mpmt.Data.Common;
using Mpmt.Data.Repositories.Currency;
using Mpmt.Services.Common;
using Mpmt.Services.Services.Common;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.Services.Currency
{
    /// <summary>
    /// The currency services.
    /// </summary>
    public class CurrencyServices : BaseService, ICurrencyServices
    {
        private readonly ICurrencyRepo _currencyRepo;
        private readonly IMapper _mapper;
        private readonly IFileProviderService _fileProviderService;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _hostEnv;

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrencyServices"/> class.
        /// </summary>
        /// <param name="currencyRepo">The currency repo.</param>
        /// <param name="mapper">The mapper.</param>
        public CurrencyServices(ICurrencyRepo currencyRepo, IMapper mapper, IHttpContextAccessor httpContextAccessor, IConfiguration config, IWebHostEnvironment hostEnv, IFileProviderService fileProviderService)
        {
            _currencyRepo = currencyRepo;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _config = config;
            _hostEnv = hostEnv;
            _fileProviderService = fileProviderService;
        }

        public async Task<SprocMessage> AddCurrencyAsync(AddCurrencyVm addCurrency)
        {
            var mappeddata = _mapper.Map<IUDCurrency>(addCurrency);

            string imagePath = null;
            if (addCurrency.CurrencyImage != null)
            {
                var folderPath = _config["Folder:CurrencyImage"];
                _fileProviderService.TryUploadFile(addCurrency.CurrencyImage, folderPath, out imagePath);
                //imagePath = await FileUploadHandler.UploadFile(_hostEnv, folderPath, addCurrency.CurrencyImage);
                mappeddata.CurrencyImgPath = imagePath;
            }

            var response = await _currencyRepo.AddCurrencyAsync(mappeddata);

            if (response.StatusCode != 200)
            {
                // Delete image file for failed operations
                if (!string.IsNullOrEmpty(imagePath))
                {
                    var imageToDeletePath = Path.Combine("" + _hostEnv.WebRootPath, imagePath[1..]);
                    File.Delete(imageToDeletePath);
                }
                return response;
            }

            return response;
        }

        /// <summary>
        /// Gets the currency async.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<CurrencyDetails>> GetCurrencyAsync()
        {
            var response = await _currencyRepo.GetCurrencyAsync();
            return response;
        }

        /// <summary>
        /// Gets the currency by id async.
        /// </summary>
        /// <param name="CurrencyId">The currency id.</param>
        /// <returns>A Task.</returns>
        public async Task<CurrencyDetails> GetCurrencyByIdAsync(int CurrencyId)
        {
            var response = await _currencyRepo.GetCurrencyByIdAsync(CurrencyId);
            return response;
        }

        /// <summary>
        /// Removes the currency async.
        /// </summary>
        /// <param name="RemoveCurrency">The remove currency.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> RemoveCurrencyAsync(UpdateCurrencyVm RemoveCurrency)
        {
            var mappeddata = _mapper.Map<IUDCurrency>(RemoveCurrency);
            var response = await _currencyRepo.RemoveCurrencyAsync(mappeddata);
            var currencyDetails = await _currencyRepo.GetCurrencyByIdAsync(RemoveCurrency.Id);
            if (response.StatusCode == 200)
                if (currencyDetails != null)
                {
                    if (!string.IsNullOrEmpty(currencyDetails.CurrencyImagePath))
                    {
                        var ImageToDeletePath = Path.Combine("" + _hostEnv.WebRootPath, currencyDetails.CurrencyImagePath.Substring(1));
                        if (File.Exists(ImageToDeletePath))
                            File.Delete(ImageToDeletePath);
                    }
                }

            return response;
        }

        /// <summary>
        /// Updates the currency async.
        /// </summary>
        /// <param name="updateCurrency">The update currency.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> UpdateCurrencyAsync(UpdateCurrencyVm updateCurrency)
        {
            //var mappeddata = _mapper.Map<IUDCurrency>(updateCurrency);

            //var currencyDetails = await _currencyRepo.GetCurrencyByIdAsync(updateCurrency.Id);
            //var existingbankImagePath = currencyDetails.CurrencyImagePath;

            //string CurrencyImage = null;
            //if (updateCurrency.CurrencyImage != null)
            //{
            //    if (!string.IsNullOrEmpty(existingbankImagePath))
            //    {
            //        string imgPath = existingbankImagePath.Substring(1);
            //        string existingImage = Path.Combine("" + _hostEnv.WebRootPath, imgPath);
            //        File.Delete(existingImage);
            //    }

            //    var folderPath = _config["Folder:CurrencyImage"];
            //    _fileProviderService.TryUploadFile(updateCurrency.CurrencyImage, folderPath, out CurrencyImage);
            //    //CurrencyImage = await FileUploadHandler.UploadFile(_hostEnv, folderPath, updateCurrency.CurrencyImage);
            //}
            //mappeddata.CurrencyImgPath = CurrencyImage;

            //var response = await _currencyRepo.UpdateCurrencyAsync(mappeddata);
            //if (response.StatusCode != 200)
            //{
            //    // Delete image file for failed operations
            //    var ImageToDeletePath = Path.Combine("" + _hostEnv.WebRootPath, CurrencyImage.Substring(1));
            //    if (File.Exists(ImageToDeletePath))
            //        File.Delete(ImageToDeletePath);
            //}

            //return response;

            var mappedData = _mapper.Map<IUDCurrency>(updateCurrency);
            var currencyDetail = await _currencyRepo.GetCurrencyByIdAsync(updateCurrency.Id);
            var existingImagePath = currencyDetail.CurrencyImagePath;

            if (updateCurrency.CurrencyImage != null)
            {
                if (!string.IsNullOrEmpty(existingImagePath))
                {
                    string imgPath = existingImagePath.Substring(1);
                    string pths = _config["Static:UserDataDirectory"];
                    string existingImage = Path.Combine(pths + imgPath);
                    File.Delete(existingImage);
                }

                var folderPath = _config["Folder:CurrencyImage"];
                _fileProviderService.TryUploadFile(updateCurrency.CurrencyImage, folderPath, out existingImagePath);
            }
            mappedData.CurrencyImgPath = existingImagePath;
            var response = await _currencyRepo.UpdateCurrencyAsync(mappedData);
            if(response.StatusCode != 200)
            {
                if (!string.IsNullOrEmpty(existingImagePath) && updateCurrency.CurrencyImage != null)
                {
                    string imgPath = existingImagePath.Substring(1);
                    string pths = _config["Static:UserDataDirectory"];
                    string existingImage = Path.Combine(pths + imgPath);
                    File.Delete(existingImage);
                }
            }

            return response;
        }
    }
}
