using Mpmt.Core.Dtos.PartnerBank;
using Mpmts.Core.Dtos;

namespace Mpmt.Data.Repositories.PartnerBank
{
    /// <summary>
    /// The partner bank repo.
    /// </summary>
    public interface IPartnerBankRepo
    {
        /// <summary>
        /// Gets the partner bank async.
        /// </summary>
        /// <param name="partnerBankFilter">The partner bank filter.</param>
        /// <returns>A Task.</returns>
        Task<IEnumerable<PartnerBankDetails>> GetPartnerBankAsync(PartnerBankFilter partnerBankFilter);
        /// <summary>
        /// Gets the partner bank by partner id async.
        /// </summary>
        /// <param name="partnerId">The partner id.</param>
        /// <returns>A Task.</returns>
        Task<PartnerBankDetails> GetPartnerBankByPartnerIdAsync(int partnerId);
        /// <summary>
        /// Adds the partner bank async.
        /// </summary>
        /// <param name="addPartnerBank">The add partner bank.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> AddPartnerBankAsync(IUDPartnerBank addPartnerBank);
        /// <summary>
        /// Updates the partner bank async.
        /// </summary>
        /// <param name="updatePartnerBank">The update partner bank.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> UpdatePartnerBankAsync(IUDPartnerBank updatePartnerBank);
        /// <summary>
        /// Removes the partner bank async.
        /// </summary>
        /// <param name="removePartnerBank">The remove partner bank.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> RemovePartnerBankAsync(IUDPartnerBank removePartnerBank);
    }
}
