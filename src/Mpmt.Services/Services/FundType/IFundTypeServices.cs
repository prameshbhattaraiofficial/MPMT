using Mpmt.Core.Dtos.FundType;
using Mpmt.Core.ViewModel.FundType;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.Services.FundType
{
    /// <summary>
    /// The fund type services.
    /// </summary>
    public interface IFundTypeServices
    {
        /// <summary>
        /// Gets the fund type async.
        /// </summary>
        /// <returns>A Task.</returns>
        Task<IEnumerable<FundTypeDetails>> GetFundTypeAsync();
        /// <summary>
        /// Gets the fund type by id async.
        /// </summary>
        /// <param name="fundTypeId">The fund type id.</param>
        /// <returns>A Task.</returns>
        Task<FundTypeDetails> GetFundTypeByIdAsync(int fundTypeId);
        /// <summary>
        /// Adds the fund type async.
        /// </summary>
        /// <param name="addFundType">The add fund type.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> AddFundTypeAsync(AddFundTypeVm addFundType);
        /// <summary>
        /// Updates the fund type async.
        /// </summary>
        /// <param name="updateFundType">The update fund type.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> UpdateFundTypeAsync(UpdateFundTypeVm updateFundType);
        /// <summary>
        /// Removes the fund type async.
        /// </summary>
        /// <param name="removeFundType">The remove fund type.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> RemoveFundTypeAsync(UpdateFundTypeVm removeFundType);
    }
}
