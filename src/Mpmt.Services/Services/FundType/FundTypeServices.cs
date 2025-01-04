using AutoMapper;
using Mpmt.Core.Dtos.FundType;
using Mpmt.Core.ViewModel.FundType;
using Mpmt.Data.Repositories.FundType;
using Mpmt.Services.Services.Common;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.Services.FundType
{
    /// <summary>
    /// The fund type services.
    /// </summary>
    public class FundTypeServices : BaseService, IFundTypeServices
    {
        private readonly IFundTypeRepo _fundTypeRepo;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="FundTypeServices"/> class.
        /// </summary>
        /// <param name="fundTypeRepo">The fund type repo.</param>
        /// <param name="mapper">The mapper.</param>
        public FundTypeServices(IFundTypeRepo fundTypeRepo, IMapper mapper)
        {
            _fundTypeRepo = fundTypeRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Adds the fund type async.
        /// </summary>
        /// <param name="addFundType">The add fund type.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> AddFundTypeAsync(AddFundTypeVm addFundType)
        {
            var mappedData = _mapper.Map<IUDFundType>(addFundType);
            var response = await _fundTypeRepo.AddFundTypeAsync(mappedData);
            return response;
        }

        /// <summary>
        /// Gets the fund type async.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<FundTypeDetails>> GetFundTypeAsync()
        {
            var response = await _fundTypeRepo.GetFundTypeAsync();
            return response;
        }

        /// <summary>
        /// Gets the fund type by id async.
        /// </summary>
        /// <param name="fundTypeId">The fund type id.</param>
        /// <returns>A Task.</returns>
        public async Task<FundTypeDetails> GetFundTypeByIdAsync(int fundTypeId)
        {
            var response = await _fundTypeRepo.GetFundTypeByIdAsync(fundTypeId);
            return response;
        }

        /// <summary>
        /// Removes the fund type async.
        /// </summary>
        /// <param name="removeFundType">The remove fund type.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> RemoveFundTypeAsync(UpdateFundTypeVm removeFundType)
        {
            var mappedData = _mapper.Map<IUDFundType>(removeFundType);
            var response = await _fundTypeRepo.RemoveFundTypeAsync(mappedData);
            return response;
        }

        /// <summary>
        /// Updates the fund type async.
        /// </summary>
        /// <param name="updateFundType">The update fund type.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> UpdateFundTypeAsync(UpdateFundTypeVm updateFundType)
        {
            var mappedData = _mapper.Map<IUDFundType>(updateFundType);
            var response = await _fundTypeRepo.UpdateFundTypeAsync(mappedData);
            return response;
        }
    }
}
