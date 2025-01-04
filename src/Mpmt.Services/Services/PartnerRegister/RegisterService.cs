using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Mpmt.Core.Common;
using Mpmt.Core.Domain.Partners.Register;
using Mpmt.Core.Dtos.PartnerSignUp;
using Mpmt.Data.Common;
using Mpmt.Data.Repositories.PartnerRegioster;
using Mpmt.Services.Common;
using Mpmts.Core.Dtos;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;

namespace Mpmt.Services.Services.PartnerRegister
{
    /// <summary>
    /// The register service.
    /// </summary>
    public class RegisterService : IRegisterService
    {
        private readonly IRegisterRepository _registerRepository;
        private readonly IMapper _mapper;
        private readonly IFileProviderService _fileProviderService;
        private readonly IConfiguration _config;
        //private readonly IRegisterService _registerService;
        private readonly IWebHostEnvironment _hostEnv;
        private readonly HttpClient _httpClient;

       

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterService"/> class.
        /// </summary>
        /// <param name="registerRepository">The register repository.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="config">The config.</param>
        /// <param name="hostEnv">The host env.</param>
        public RegisterService(IRegisterRepository registerRepository, IMapper mapper,
             IConfiguration config, HttpClient httpClient,
            IWebHostEnvironment hostEnv, IFileProviderService fileProviderService)
        {
            _registerRepository = registerRepository;
            _mapper = mapper;
            _config = config;
            _httpClient = httpClient;
            _hostEnv = hostEnv;
            _fileProviderService = fileProviderService;
        }

        /// <summary>
        /// Registers the partner.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> RegisterPartner(SignUpPartnerdetail data)
        {
            var passwordSalt = CryptoUtils.GenerateKeySalt();
            var passwordHash = CryptoUtils.HashHmacSha512Base64(data.Password, passwordSalt);
            var registerdetail = _mapper.Map<RegisterPartner>(data);
            registerdetail.Withoutfirstname = data.Withoutfirstname;
            registerdetail.Event = "I";
            registerdetail.OtpExipiryDate = DateTime.UtcNow.AddMinutes(2);
            registerdetail.OTP = data.Otp;
            registerdetail.SurName = data.LastName;
            //registerdetail.ShortName = data.ShortName;
            registerdetail.FormNumber = 1;
            registerdetail.Post = data.Position;
            registerdetail.MobileNumber = data.PhoneNumber;
            registerdetail.PasswordSalt = passwordSalt;
            registerdetail.PasswordHash = passwordHash;

            var response = await _registerRepository.RegisterPartner(registerdetail);
            return response;


        }
        /// <summary>
        /// Resets the otp.
        /// </summary>
        /// <param name="Email">The email.</param>
        /// <param name="Opt">The opt.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> ResetOtp(string Email, string Opt)
        {
            var exp = DateTime.UtcNow.AddMinutes(5);
            var response = await _registerRepository.ResetOtpAsync(Email, Opt, exp);
            return response;

        }
        /// <summary>
        /// Validates the otp.
        /// </summary>
        /// <param name="Email">The email.</param>
        /// <param name="Otp">The otp.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> ValidateOtp(string Email, string Otp)
        {

            var response = await _registerRepository.ValidateOtpAsync(Email, Otp);
            return response;


        }

        /// <summary>
        /// Registers the partnerstep1.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> RegisterPartnerstep1(SignUpStep1 data)
        {
            var registerdetail = _mapper.Map<RegisterPartner>(data);
            registerdetail.Event = "U";
            registerdetail.FormNumber = 2;
            var response = await _registerRepository.RegisterPartner(registerdetail);
            return response;

        }
        /// <summary>
        /// Registers the partnerstep2.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> RegisterPartnerstep2(SignUpStep2 data)
        {
            var detail = await _registerRepository.GetPartnerDetail(data.Email);

            var registerdetail = _mapper.Map<RegisterPartner>(data);
            registerdetail.Event = "U";
            registerdetail.FormNumber = 3;


            var ComapnayLogoImagePath = detail.CompanyLogoImgPath;
            var LicenseDocumentImagePathList = new List<string>();
            if (data.CompanyLogoImg is not null)
            {
                if (data.CompanyLogoImg.Length > 0)
                {
                    var folderPath = _config["Folder:PartnersCompanylogo"];
                    _fileProviderService.TryUploadFile(data.CompanyLogoImg, folderPath, out ComapnayLogoImagePath);
                    //ComapnayLogoImagePath = await FileUploadHandler.UploadFile(_hostEnv, folderPath, data.CompanyLogoImg);
                }

            }
            if (data.LicensedocImg is not null && data.LicensedocImg.Count > 0)
            {

                foreach (var image in data.LicensedocImg)
                {
                    if (image.Length > 0)
                    {
                        var (isValidLicenseDocument, _) = await FileValidatorUtils.IsValidImageAsync(image, FileTypes.ImageFiles);
                        if (!isValidLicenseDocument)
                        {
                            return new SprocMessage() { StatusCode = 400, MsgType = "Error", MsgText = "Invalid Image Type" };

                        }
                        var folderPath = _config["Folder:PartnersLicenseDocument"];
                        _fileProviderService.TryUploadFile(image, folderPath, out var IDFrontImagePath);
                        LicenseDocumentImagePathList.Add(IDFrontImagePath);
                        //LicenseDocumentImagePathList.Add(await FileUploadHandler.UploadFile(_hostEnv, folderPath, image));
                    }
                }

            }
            registerdetail.LicensedocImgPath = LicenseDocumentImagePathList;
            if (data.LicensedocImgPath is not null && data.LicensedocImgPath.Count > 0)
            {
                foreach (var image in data.LicensedocImgPath)
                {
                    registerdetail.LicensedocImgPath.Add(image);
                }
            }


            //registerdetail.LicensedocImgPath = LicenseDocumentImagePath;
            registerdetail.CompanyLogoImgPath = ComapnayLogoImagePath;
            var response = await _registerRepository.RegisterPartner(registerdetail);

            if (response.StatusCode != 200)
            {
                //delete saved image
                if (registerdetail.CompanyLogoImgPath is not null)
                {
                    if (!string.IsNullOrEmpty(ComapnayLogoImagePath))
                    {
                        string imgPath = ComapnayLogoImagePath.Substring(1);
                        string existingImage = Path.Combine("" + _hostEnv.WebRootPath, imgPath);
                        System.IO.File.Delete(existingImage);
                    }
                }
                if (LicenseDocumentImagePathList is not null && LicenseDocumentImagePathList.Count > 0)
                {
                    foreach (var image in LicenseDocumentImagePathList)
                    {
                        if (!string.IsNullOrEmpty(image))
                        {
                            string imgPath = image.Substring(1);
                            string existingImage = Path.Combine("" + _hostEnv.WebRootPath, imgPath);
                            System.IO.File.Delete(existingImage);
                        }
                    }
                }

            }
            if (response.StatusCode == 200)
            {
                //delete old image only when new image
                if (!string.IsNullOrEmpty(detail.CompanyLogoImgPath) && data.CompanyLogoImg is not null)
                {
                    string imgPath = detail.CompanyLogoImgPath.Substring(1);
                    string existingImage = Path.Combine("" + _hostEnv.WebRootPath, imgPath);
                    System.IO.File.Delete(existingImage);
                }
                if (data.DeletedLicensedocImgPath is not null && data.DeletedLicensedocImgPath.Count > 0)
                {
                    foreach (var image in data.DeletedLicensedocImgPath)
                    {
                        if (!string.IsNullOrEmpty(image))
                        {
                            string imgPath = image.Substring(1);
                            string existingImage = Path.Combine("" + _hostEnv.WebRootPath, imgPath);
                            System.IO.File.Delete(existingImage);
                        }
                    }
                }

            }
            return response;

        }
        /// <summary>
        /// Registers the partnerstep3.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> RegisterPartnerstep3(SignUpStep3 data)
        {
            var detail = await _registerRepository.GetPartnerDetail(data.Email);
            var registerdetail = _mapper.Map<RegisterPartner>(data);
            registerdetail.Event = "U";
            registerdetail.FormNumber = 4;
            //save image

            var IDBackImagePath = detail.IdBackImgPath;
            var IDFrontImagePath = detail.IdFrontImgPath;
            var AddressProofImagePath = detail.AddressProofImgPath;


            if (data.IdBackImg is not null)
            {
                if (data.IdBackImg.Length > 0)
                {
                    var folderPath = _config["Folder:PartnersIdImage"];
                    _fileProviderService.TryUploadFile(data.IdBackImg, folderPath, out IDBackImagePath);
                    //IDBackImagePath = await FileUploadHandler.UploadFile(_hostEnv, folderPath, data.IdBackImg);
                }
            }
            if (data.IdFrontImg is not null)
            {
                if (data.IdFrontImg.Length > 0)
                {
                    var folderPath = _config["Folder:PartnersIdImage"];
                    _fileProviderService.TryUploadFile(data.IdFrontImg, folderPath, out IDFrontImagePath);
                    //IDFrontImagePath = await FileUploadHandler.UploadFile(_hostEnv, folderPath, data.IdFrontImg);
                }
            }
            if (data.AddressProofImg is not null)
            {
                if (data.AddressProofImg.Length > 0)
                {
                    var folderPath = _config["Folder:PartnersAddressProof"];
                    _fileProviderService.TryUploadFile(data.AddressProofImg, folderPath, out AddressProofImagePath);
                    //AddressProofImagePath = await FileUploadHandler.UploadFile(_hostEnv, folderPath, data.AddressProofImg);
                }
            }

            registerdetail.IdBackImgPath = IDBackImagePath;
            registerdetail.IdFrontImgPath = IDFrontImagePath;
            registerdetail.AddressProofImgPath = AddressProofImagePath;


            var response = await _registerRepository.RegisterPartner(registerdetail);


            if (response.StatusCode != 200)
            {
                //delete saved image

                if (data.IdBackImg is not null)
                {
                    if (!string.IsNullOrEmpty(IDBackImagePath))
                    {
                        string imgPath = IDBackImagePath.Substring(1);
                        string existingImage = Path.Combine("" + _hostEnv.WebRootPath, imgPath);
                        System.IO.File.Delete(existingImage);
                    }

                }
                if (data.IdFrontImg is not null)
                {

                    if (!string.IsNullOrEmpty(IDFrontImagePath))
                    {
                        string imgPath = IDFrontImagePath.Substring(1);
                        string existingImage = Path.Combine("" + _hostEnv.WebRootPath, imgPath);
                        System.IO.File.Delete(existingImage);
                    }
                }
                if (data.AddressProofImg is not null)
                {
                    if (!string.IsNullOrEmpty(AddressProofImagePath))
                    {
                        string imgPath = AddressProofImagePath.Substring(1);
                        string existingImage = Path.Combine("" + _hostEnv.WebRootPath, imgPath);
                        System.IO.File.Delete(existingImage);
                    }
                }


            }
            if (response.StatusCode == 200)
            {
                //delete old image only when new image

                if (!string.IsNullOrEmpty(detail.IdBackImgPath) && data.IdBackImg is not null)
                {
                    string imgPath = detail.IdBackImgPath.Substring(1);
                    string existingImage = Path.Combine("" + _hostEnv.WebRootPath, imgPath);
                    System.IO.File.Delete(existingImage);
                }
                if (!string.IsNullOrEmpty(detail.IdFrontImgPath) && data.IdFrontImg is not null)
                {
                    string imgPath = detail.IdFrontImgPath.Substring(1);
                    string existingImage = Path.Combine("" + _hostEnv.WebRootPath, imgPath);
                    System.IO.File.Delete(existingImage);
                }
                if (!string.IsNullOrEmpty(detail.AddressProofImgPath) && data.AddressProofImg is not null)
                {
                    string imgPath = detail.AddressProofImgPath.Substring(1);
                    string existingImage = Path.Combine("" + _hostEnv.WebRootPath, imgPath);
                    System.IO.File.Delete(existingImage);
                }

            }
            return response;

        }
        /// <summary>
        /// Gets the register partner.
        /// </summary>
        /// <param name="Email">The email.</param>
        /// <returns>A Task.</returns>
        public async Task<PartnerDetailSignup> GetRegisterPartner(string Email)
        {
            var response = await _registerRepository.GetPartnerDetail(Email);
            return response;

        }
        /// <summary>
        /// Gets the register partner by i d.
        /// </summary>
        /// <param name="Id">The id.</param>
        /// <returns>A Task.</returns>
        public async Task<PartnerDetailSignup> GetRegisterPartnerByID(string Id)
        {
            var response = await _registerRepository.GetPartnerDetailById(Id);
            return response;

        }

        


        public async Task<AddressDetails> GetPlaceDetails(string placeId)
        {
            
            var apikey = _config["GoogleAddress:Apikey"];
            string apiUrl = $"https://maps.googleapis.com/maps/api/place/details/json?place_id={placeId}&key={apikey}";

            using (HttpResponseMessage response = await _httpClient.GetAsync(apiUrl))
            {
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    dynamic responseObject = JsonConvert.DeserializeObject(jsonResponse);
                    var result = responseObject.result;

                    var addressDetails = new AddressDetails
                    {
                        Country = GetAddressComponent(result.address_components, "country"),
                        State = GetAddressComponent(result.address_components, "administrative_area_level_1"),
                        City = GetAddressComponent(result.address_components, "locality"),
                        ZipCode = GetAddressComponent(result.address_components, "postal_code")
                    };

                    return addressDetails;
                }
                else
                {
                    throw new Exception($"API request failed with status code: {response.StatusCode}");
                }
            }
        }

        public async Task<List<PlaceSuggestion>> AddressSearch(string query)
        {
            string encodedQuery = WebUtility.UrlEncode(query);
            var apiKey = _config["GoogleAddress:Apikey"];
            string apiUrl = $"https://maps.googleapis.com/maps/api/place/autocomplete/json?key={apiKey}&input={encodedQuery}";

            try
            {
                using (HttpResponseMessage response = await _httpClient.GetAsync(apiUrl))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        if (string.IsNullOrWhiteSpace(jsonResponse))
                        {
                            throw new Exception("Response from API is empty.");
                        }

                        dynamic responseObject = JsonConvert.DeserializeObject(jsonResponse);
                        if (responseObject == null)
                        {
                            throw new Exception("Deserialized response object is null.");
                        }

                        List<PlaceSuggestion> suggestions = new List<PlaceSuggestion>();
                        if (responseObject.predictions == null)
                        {
                            throw new Exception("Predictions property is null in API response.");
                        }

                        foreach (var prediction in responseObject.predictions)
                        {
                            if (prediction == null || prediction.description == null || prediction.place_id == null)
                            {
                                throw new Exception("Prediction or its properties are null.");
                            }

                            suggestions.Add(new PlaceSuggestion
                            {
                                Description = prediction.description.ToString(),
                                PlaceId = prediction.place_id.ToString()
                            });
                        }

                        return suggestions;
                    }
                    else
                    {
                        string errorResponse = await response.Content.ReadAsStringAsync();
                        throw new Exception($"API request failed with status code: {response.StatusCode}, Response: {errorResponse}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception details for troubleshooting
                Console.WriteLine($"Exception in AddressSearch: {ex.Message}");
                throw;
            }
        }
        private string GetAddressComponent(dynamic components, string type)
        {
            foreach (var component in components)
            {
                foreach (var componentType in component.types)
                {
                    if (componentType == type)
                    {
                        return component.long_name.ToString();
                    }
                }
            }
            return string.Empty;
        }







    }
}
