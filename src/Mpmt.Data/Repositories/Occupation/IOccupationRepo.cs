using Mpmt.Core.Dtos.Occupation;
using Mpmts.Core.Dtos;

namespace Mpmt.Data.Repositories.Occupation
{
    /// <summary>
    /// The occupation repo.
    /// </summary>
    public interface IOccupationRepo
    {
        /// <summary>
        /// Gets the occupation async.
        /// </summary>
        /// <param name="occupationFilter">The occupation filter.</param>
        /// <returns>A Task.</returns>
        Task<IEnumerable<OccupationDetails>> GetOccupationAsync(OccupationFilter occupationFilter);
        /// <summary>
        /// Gets the occupation by id async.
        /// </summary>
        /// <param name="occupationId">The occupation id.</param>
        /// <returns>A Task.</returns>
        Task<OccupationDetails> GetOccupationByIdAsync(int occupationId);
        /// <summary>
        /// Adds the occupation async.
        /// </summary>
        /// <param name="addOccupation">The add occupation.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> AddOccupationAsync(IUDOccupation addOccupation);
        /// <summary>
        /// Updates the occupation async.
        /// </summary>
        /// <param name="updateOccupation">The update occupation.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> UpdateOccupationAsync(IUDOccupation updateOccupation);
        /// <summary>
        /// Removes the occupation async.
        /// </summary>
        /// <param name="removeOccupation">The remove occupation.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> RemoveOccupationAsync(IUDOccupation removeOccupation);
    }
}