using AutoMapper;
using Mpmt.Core.Dtos.ServiceChargeCategory;
using Mpmt.Core.ViewModel.ServiceChargeCategory;
using Mpmt.Data.Repositories.ServiceChargeCategory;
using Mpmt.Services.Services.Common;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.Services.ServiceChargeCategory
{
    /// <summary>
    /// The service category services.
    /// </summary>
    public class ServiceCategoryServices : BaseService, IServiceCategoryServices
    {
        private readonly IServiceCategoryRepo _serviceCategoryRepo;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceCategoryServices"/> class.
        /// </summary>
        /// <param name="serviceCategoryRepo">The service category repo.</param>
        /// <param name="mapper">The mapper.</param>
        public ServiceCategoryServices(IServiceCategoryRepo serviceCategoryRepo, IMapper mapper)
        {
            _serviceCategoryRepo = serviceCategoryRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Adds the service category async.
        /// </summary>
        /// <param name="addServiceCategory">The add service category.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> AddServiceCategoryAsync(AddServiceCategoryVm addServiceCategory)
        {
            var mappedData = _mapper.Map<IUDServiceCategory>(addServiceCategory);
            var response = await _serviceCategoryRepo.AddServiceCategoryAsync(mappedData);
            return response;
        }

        /// <summary>
        /// Gets the service category async.
        /// </summary>
        /// <param name="serviceCategoryFilter">The service category filter.</param>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<ServiceCategoryDetails>> GetServiceCategoryAsync(ServiceCategoryFilter serviceCategoryFilter)
        {
            var response = await _serviceCategoryRepo.GetServiceCategoryAsync(serviceCategoryFilter);
            return response;
        }

        /// <summary>
        /// Gets the service category by id async.
        /// </summary>
        /// <param name="serviceCategoryId">The service category id.</param>
        /// <returns>A Task.</returns>
        public async Task<ServiceCategoryDetails> GetServiceCategoryByIdAsync(int serviceCategoryId)
        {
            var response = await _serviceCategoryRepo.GetServiceCategoryByIdAsync(serviceCategoryId);
            return response;
        }

        /// <summary>
        /// Removes the service category async.
        /// </summary>
        /// <param name="removeServiceCategory">The remove service category.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> RemoveServiceCategoryAsync(UpdateServiceCategoryVm removeServiceCategory)
        {
            var mappedData = _mapper.Map<IUDServiceCategory>(removeServiceCategory);
            var response = await _serviceCategoryRepo.RemoveServiceCategoryAsync(mappedData);
            return response;
        }

        /// <summary>
        /// Updates the service category async.
        /// </summary>
        /// <param name="updateServiceCategory">The update service category.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> UpdateServiceCategoryAsync(UpdateServiceCategoryVm updateServiceCategory)
        {
            var mappedData = _mapper.Map<IUDServiceCategory>(updateServiceCategory);
            var response = await _serviceCategoryRepo.UpdateServiceCategoryAsync(mappedData);
            return response;
        }
    }
}
