using AutoMapper;
using Microsoft.AspNetCore.Http;
using Mpmt.Core.Dtos.ConversionRate;
using Mpmt.Core.Dtos.ConversionRateHistory;
using Mpmt.Core.Dtos.Paging;
using Mpmt.Core.Dtos.WebCrawler;
using Mpmt.Core.ViewModel.ConversionRate;
using Mpmt.Data.Repositories.ConversionRate;
using Mpmt.Services.Services.Common;
using Mpmts.Core.Dtos;
using System.Security.Claims;

namespace Mpmt.Services.Services.ConversionRate
{
    /// <summary>
    /// The conversion rate services.
    /// </summary>
    public class ConversionRateServices : BaseService, IConversionRateServices
    {
        private readonly IConversionRateRepo _conversionRateRepo;
        private readonly IMapper _mapper;
        private readonly ClaimsPrincipal _loggedInUser;
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConversionRateServices"/> class.
        /// </summary>
        /// <param name="conversionRateRepo">The conversion rate repo.</param>
        /// <param name="mapper">The mapper.</param>
        public ConversionRateServices(IConversionRateRepo conversionRateRepo, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _conversionRateRepo = conversionRateRepo;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _loggedInUser = _httpContextAccessor.HttpContext?.User;
        }

        /// <summary>
        /// Adds the conversion rate async.
        /// </summary>
        /// <param name="addConversionRate">The add conversion rate.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> AddConversionRateAsync(AddConversionRateVm addConversionRate)
        {
            var mappedData = _mapper.Map<IUDConversionRate>(addConversionRate);
            mappedData.UserType = _loggedInUser.FindFirstValue("UserType");
            mappedData.LoggedInUserName = _loggedInUser.FindFirstValue(ClaimTypes.Name);
            var response = await _conversionRateRepo.AddConversionRateAsync(mappedData);
            return response;
        }

        /// <summary>
        /// Gets the conversion rate async.
        /// </summary>
        /// <param name="conversionRateFilter">The conversion rate filter.</param>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<ConversionRateDetails>> GetConversionRateAsync(ConversionRateFilter conversionRateFilter)
        {
            var response = await _conversionRateRepo.GetConversionRateAsync(conversionRateFilter);
            return response;
        }

        /// <summary>
        /// Gets the conversion rate by id async.
        /// </summary>
        /// <param name="conversionRateId">The conversion rate id.</param>
        /// <returns>A Task.</returns>
        public async Task<ConversionRateDetails> GetConversionRateByIdAsync(int conversionRateId)
        {
            var response = await _conversionRateRepo.GetConversionRateByIdAsync(conversionRateId);
            return response;
        }

        public async Task<PagedList<ExchangeRateHistoryDetails>> GetExchangeRateHistoryAsync(ExchangeRateFilter exchangeRateFilter)
        {
            var response = await _conversionRateRepo.GetExchangeRateHistoryAsync(exchangeRateFilter);
            return response;
        }

        /// <summary>
        /// Removes the conversion rate async.
        /// </summary>
        /// <param name="removeConversionRate">The remove conversion rate.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> RemoveConversionRateAsync(UpdateConversionRateVm removeConversionRate)
        {
            var mappedData = _mapper.Map<IUDConversionRate>(removeConversionRate);
            mappedData.UserType = _loggedInUser.FindFirstValue("UserType");
            mappedData.LoggedInUserName = _loggedInUser.FindFirstValue(ClaimTypes.Name);
            var response = await _conversionRateRepo.RemoveConversionRateAsync(mappedData);
            return response;
        }

        /// <summary>
        /// Updates the conversion rate async.
        /// </summary>
        /// <param name="updateConversionRate">The update conversion rate.</param>
        /// <returns>A Task.</returns>
        public async Task<(SprocMessage, IEnumerable<ExchangeRateChangedListPartner>)> UpdateConversionRateAsync(UpdateConversionRateVm updateConversionRate)
        {
            var mappedData = _mapper.Map<IUDConversionRate>(updateConversionRate);
            mappedData.UserType = _loggedInUser.FindFirstValue("UserType");
            mappedData.LoggedInUserName = _loggedInUser.FindFirstValue(ClaimTypes.Name);
            var (response, data) = await _conversionRateRepo.UpdateConversionRateAsync(mappedData);
            return (response, data);
        }
    }
}
