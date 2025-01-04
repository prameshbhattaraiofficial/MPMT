using Mpmt.Core.Dtos.PaymentType;
using Mpmt.Core.ViewModel.PaymentType;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.Services.PaymentType
{
    /// <summary>
    /// The payment type services.
    /// </summary>
    public interface IPaymentTypeServices
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
        /// <param name="PaymentTypeId">The payment type id.</param>
        /// <returns>A Task.</returns>
        Task<PaymentTypeDetails> GetPaymentTypeByIdAsync(int PaymentTypeId);
        /// <summary>
        /// Adds the payment type async.
        /// </summary>
        /// <param name="addPaymentType">The add payment type.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> AddPaymentTypeAsync(AddPaymentTypeVm addPaymentType);
        /// <summary>
        /// Updates the payment type async.
        /// </summary>
        /// <param name="updatePaymentType">The update payment type.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> UpdatePaymentTypeAsync(UpdatePaymentTypeVm updatePaymentType);
        /// <summary>
        /// Removes the payment type async.
        /// </summary>
        /// <param name="removePaymentType">The remove payment type.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> RemovePaymentTypeAsync(UpdatePaymentTypeVm removePaymentType);
    }
}
