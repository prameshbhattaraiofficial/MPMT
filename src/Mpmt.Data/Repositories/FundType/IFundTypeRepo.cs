using Mpmt.Core.Dtos.FundType;
using Mpmts.Core.Dtos;

namespace Mpmt.Data.Repositories.FundType
{
    /// <summary>
    /// The fund type repo.
    /// </summary>
    public interface IFundTypeRepo
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
        Task<SprocMessage> AddFundTypeAsync(IUDFundType addFundType);
        /// <summary>
        /// Updates the fund type async.
        /// </summary>
        /// <param name="updateFundType">The update fund type.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> UpdateFundTypeAsync(IUDFundType updateFundType);
        /// <summary>
        /// Removes the fund type async.
        /// </summary>
        /// <param name="removeFundType">The remove fund type.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> RemoveFundTypeAsync(IUDFundType removeFundType);
    }
}
