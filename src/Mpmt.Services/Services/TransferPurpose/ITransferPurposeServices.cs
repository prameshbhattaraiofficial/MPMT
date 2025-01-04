using Mpmt.Core.Dtos.TransferPurpose;
using Mpmt.Core.ViewModel.TransferPurpose;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.Services.TransferPurpose
{
    /// <summary>
    /// The transfer purpose services.
    /// </summary>
    public interface ITransferPurposeServices
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
        Task<SprocMessage> AddTransferPurposeAsync(AddTransferVm addTransferPurpose);
        /// <summary>
        /// Updates the transfer purpose async.
        /// </summary>
        /// <param name="updateTransferPurpose">The update transfer purpose.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> UpdateTransferPurposeAsync(UpdateTransferVm updateTransferPurpose);
        /// <summary>
        /// Removes the transfer purpose async.
        /// </summary>
        /// <param name="removeTransferPurpose">The remove transfer purpose.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> RemoveTransferPurposeAsync(UpdateTransferVm removeTransferPurpose);
    }
}
