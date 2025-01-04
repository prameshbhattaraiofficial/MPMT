using Mpmt.Core.Dtos.KYCRemark;
using Mpmt.Core.ViewModel.KYCRemark;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.Services.KYCRemark
{
    /// <summary>
    /// The kyc remark services.
    /// </summary>
    public interface IKycRemarkServices
    {
        /// <summary>
        /// Gets the kyc remark async.
        /// </summary>
        /// <param name="kycRemarkFilter">The kyc remark filter.</param>
        /// <returns>A Task.</returns>
        Task<IEnumerable<KycRemarkDetails>> GetKycRemarkAsync(KycRemarkFilter kycRemarkFilter);
        /// <summary>
        /// Gets the kyc remark by id async.
        /// </summary>
        /// <param name="kycRemarkId">The kyc remark id.</param>
        /// <returns>A Task.</returns>
        Task<KycRemarkDetails> GetKycRemarkByIdAsync(int kycRemarkId);
        /// <summary>
        /// Adds the kyc remark async.
        /// </summary>
        /// <param name="addKycRemark">The add kyc remark.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> AddKycRemarkAsync(AddKycRemarkVm addKycRemark);
        /// <summary>
        /// Updates the kyc remark async.
        /// </summary>
        /// <param name="updateKycRemark">The update kyc remark.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> UpdateKycRemarkAsync(UpdateKycRemarkVm updateKycRemark);
        /// <summary>
        /// Removes the kyc remark async.
        /// </summary>
        /// <param name="removeKycRemark">The remove kyc remark.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> RemoveKycRemarkAsync(UpdateKycRemarkVm removeKycRemark);
    }
}
