using Mpmt.Core.Dtos.ServiceChargeCategory;
using Mpmt.Core.ViewModel.ServiceChargeCategory;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.Services.ServiceChargeCategory
{
    /// <summary>
    /// The service category services.
    /// </summary>
    public interface IServiceCategoryServices
    {
        /// <summary>
        /// Gets the service category async.
        /// </summary>
        /// <param name="serviceCategoryFilter">The service category filter.</param>
        /// <returns>A Task.</returns>
        Task<IEnumerable<ServiceCategoryDetails>> GetServiceCategoryAsync(ServiceCategoryFilter serviceCategoryFilter);
        /// <summary>
        /// Gets the service category by id async.
        /// </summary>
        /// <param name="serviceCategoryId">The service category id.</param>
        /// <returns>A Task.</returns>
        Task<ServiceCategoryDetails> GetServiceCategoryByIdAsync(int serviceCategoryId);
        /// <summary>
        /// Adds the service category async.
        /// </summary>
        /// <param name="addServiceCategory">The add service category.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> AddServiceCategoryAsync(AddServiceCategoryVm addServiceCategory);
        /// <summary>
        /// Updates the service category async.
        /// </summary>
        /// <param name="updateServiceCategory">The update service category.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> UpdateServiceCategoryAsync(UpdateServiceCategoryVm updateServiceCategory);
        /// <summary>
        /// Removes the service category async.
        /// </summary>
        /// <param name="removeServiceCategory">The remove service category.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> RemoveServiceCategoryAsync(UpdateServiceCategoryVm removeServiceCategory);
    }
}
