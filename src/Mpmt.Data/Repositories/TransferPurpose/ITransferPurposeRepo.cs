using Mpmt.Core.Dtos.TransferPurpose;
using Mpmts.Core.Dtos;

namespace Mpmt.Data.Repositories.TransferPurpose
{
    /// <summary>
    /// The transfer purpose repo.
    /// </summary>
    public interface ITransferPurposeRepo
    {
        /// <summary>
        /// Gets the transfer purpose async.
        /// </summary>
        /// <param name="transferPurposeFilter">The transfer purpose filter.</param>
        /// <returns>A Task.</returns>
        Task<IEnumerable<TransferPurposeDetails>> GetTransferPurposeAsync(TransferPurposeFilter transferPurposeFilter);
        /// <summary>
        /// Gets the transfer purpose by id async.
        /// </summary>
        /// <param name="transferPurposeId">The transfer purpose id.</param>
        /// <returns>A Task.</returns>
        Task<TransferPurposeDetails> GetTransferPurposeByIdAsync(int transferPurposeId);
        /// <summary>
        /// Adds the transfer purpose async.
        /// </summary>
        /// <param name="addTransferPurpose">The add transfer purpose.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> AddTransferPurposeAsync(IUDTransferPurpose addTransferPurpose);
        /// <summary>
        /// Updates the transfer purpose async.
        /// </summary>
        /// <param name="updateTransferPurpose">The update transfer purpose.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> UpdateTransferPurposeAsync(IUDTransferPurpose updateTransferPurpose);
        /// <summary>
        /// Removes the transfer purpose async.
        /// </summary>
        /// <param name="removeTransferPurpose">The remove transfer purpose.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> RemoveTransferPurposeAsync(IUDTransferPurpose removeTransferPurpose);
    }
}