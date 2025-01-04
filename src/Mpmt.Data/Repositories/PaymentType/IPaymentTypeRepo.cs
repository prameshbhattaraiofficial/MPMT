using Mpmt.Core.Dtos.PaymentType;
using Mpmts.Core.Dtos;

namespace Mpmt.Data.Repositories.PaymentType
{
    /// <summary>
    /// The payment type repo.
    /// </summary>
    public interface IPaymentTypeRepo
    {
        /// <summary>
        /// Gets the payment type async.
        /// </summary>
        /// <param name="paymentTypeFilter">The payment type filter.</param>
        /// <returns>A Task.</returns>
        Task<IEnumerable<PaymentTypeDetails>> GetPaymentTypeAsync(PaymentTypeFilter paymentTypeFilter);
        /// <summary>
        /// Gets the payment type by id async.
        /// </summary>
        /// <param name="paymentTypeId">The payment type id.</param>
        /// <returns>A Task.</returns>
        Task<PaymentTypeDetails> GetPaymentTypeByIdAsync(int paymentTypeId);
        /// <summary>
        /// Adds the payment type async.
        /// </summary>
        /// <param name="addPaymentType">The add payment type.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> AddPaymentTypeAsync(IUDPaymentType addPaymentType);
        /// <summary>
        /// Updates the payment type async.
        /// </summary>
        /// <param name="updatePaymentType">The update payment type.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> UpdatePaymentTypeAsync(IUDPaymentType updatePaymentType);
        /// <summary>
        /// Removes the payment type async.
        /// </summary>
        /// <param name="removePaymentType">The remove payment type.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> RemovePaymentTypeAsync(IUDPaymentType removePaymentType);
    }
}
