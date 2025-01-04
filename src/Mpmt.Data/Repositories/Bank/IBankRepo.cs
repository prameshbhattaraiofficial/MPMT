using Mpmt.Core.Dtos.Banks;
using Mpmts.Core.Dtos;

namespace Mpmt.Data.Repositories.Bank
{
    /// <summary>
    /// The bank repo.
    /// </summary>
    public interface IBankRepo
    {
        /// <summary>
        /// Adds the bank async.
        /// </summary>
        /// <param name="addbank">The addbank.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> AddBankAsync(IUDBank addbank);
        /// <summary>
        /// Gets the bank by id async.
        /// </summary>
        /// <param name="BankId">The bank id.</param>
        /// <returns>A Task.</returns>
        Task<BankDetails> GetBankByIdAsync(int BankId);
        /// <summary>
        /// Updates the bank async.
        /// </summary>
        /// <param name="Updatebank">The updatebank.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> UpdateBankAsync(IUDBank Updatebank);
        /// <summary>
        /// Removes the bank async.
        /// </summary>
        /// <param name="Removebank">The removebank.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> RemoveBankAsync(IUDBank Removebank);
        /// <summary>
        /// Gets the bank async.
        /// </summary>
        /// <param name="bankFilter">The bank filter.</param>
        /// <returns>A Task.</returns>
        Task<IEnumerable<BankDetails>> GetBankAsync(BankFilter bankFilter);
    }
}
