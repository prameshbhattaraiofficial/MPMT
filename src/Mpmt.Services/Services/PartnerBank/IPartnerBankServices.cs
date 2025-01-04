using Mpmt.Core.Dtos.PartnerBank;
using Mpmt.Core.ViewModel.PartnerBank;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.Services.PartnerBank
{
    /// <summary>
    /// The partner bank services.
    /// </summary>
    public interface IPartnerBankServices
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
        Task<SprocMessage> AddPartnerBankAsync(AddPartnerBankVm addPartnerBank);
        /// <summary>
        /// Updates the partner bank async.
        /// </summary>
        /// <param name="updatePartnerBank">The update partner bank.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> UpdatePartnerBankAsync(UpdatePartnerBankVm updatePartnerBank);
        /// <summary>
        /// Removes the partner bank async.
        /// </summary>
        /// <param name="removePartnerBank">The remove partner bank.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> RemovePartnerBankAsync(UpdatePartnerBankVm removePartnerBank);
    }
}
