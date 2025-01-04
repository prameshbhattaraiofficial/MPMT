using AutoMapper;
using Mpmt.Core.Dtos.Occupation;
using Mpmt.Core.ViewModel.Occupation;
using Mpmt.Data.Repositories.Occupation;
using Mpmt.Services.Services.Common;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.Services.Occupation
{
    /// <summary>
    /// The occupation services.
    /// </summary>
    public class OccupationServices : BaseService, IOccupationServices
    {
        private readonly IOccupationRepo _occupationRepo;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="OccupationServices"/> class.
        /// </summary>
        /// <param name="occupationRepo">The occupation repo.</param>
        /// <param name="mapper">The mapper.</param>
        public OccupationServices(IOccupationRepo occupationRepo, IMapper mapper)
        {
            _occupationRepo = occupationRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Adds the occupation async.
        /// </summary>
        /// <param name="addOccupation">The add occupation.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> AddOccupationAsync(AddOccupationVm addOccupation)
        {
            var mappedData = _mapper.Map<IUDOccupation>(addOccupation);
            var response = await _occupationRepo.AddOccupationAsync(mappedData);
            return response;
        }

        /// <summary>
        /// Gets the occupation async.
        /// </summary>
        /// <param name="occupationFilter">The occupation filter.</param>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<OccupationDetails>> GetOccupationAsync(OccupationFilter occupationFilter)
        {
            var response = await _occupationRepo.GetOccupationAsync(occupationFilter);
            return response;
        }

        /// <summary>
        /// Gets the occupation by id async.
        /// </summary>
        /// <param name="occupationId">The occupation id.</param>
        /// <returns>A Task.</returns>
        public async Task<OccupationDetails> GetOccupationByIdAsync(int occupationId)
        {
            var response = await _occupationRepo.GetOccupationByIdAsync(occupationId);
            return response;
        }

        /// <summary>
        /// Removes the occupation async.
        /// </summary>
        /// <param name="removeOccupation">The remove occupation.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> RemoveOccupationAsync(UpdateOccupationVm removeOccupation)
        {
            var mappedData = _mapper.Map<IUDOccupation>(removeOccupation);
            var response = await _occupationRepo.RemoveOccupationAsync(mappedData);
            return response;
        }

        /// <summary>
        /// Updates the occupation async.
        /// </summary>
        /// <param name="updateOccupation">The update occupation.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> UpdateOccupationAsync(UpdateOccupationVm updateOccupation)
        {
            var mappedData = _mapper.Map<IUDOccupation>(updateOccupation);
            var response = await _occupationRepo.UpdateOccupationAsync(mappedData);
            return response;
        }
    }
}
