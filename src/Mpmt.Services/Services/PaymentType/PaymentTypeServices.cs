using AutoMapper;
using Mpmt.Core.Dtos.PaymentType;
using Mpmt.Core.ViewModel.PaymentType;
using Mpmt.Data.Repositories.PaymentType;
using Mpmt.Services.Services.Common;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.Services.PaymentType
{
    /// <summary>
    /// The payment type services.
    /// </summary>
    public class PaymentTypeServices : BaseService, IPaymentTypeServices
    {
        private readonly IPaymentTypeRepo _paymentTypeRepo;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypeServices"/> class.
        /// </summary>
        /// <param name="paymentTypeRepo">The payment type repo.</param>
        /// <param name="mapper">The mapper.</param>
        public PaymentTypeServices(IPaymentTypeRepo paymentTypeRepo, IMapper mapper)
        {
            _paymentTypeRepo = paymentTypeRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Adds the payment type async.
        /// </summary>
        /// <param name="addPaymentType">The add payment type.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> AddPaymentTypeAsync(AddPaymentTypeVm addPaymentType)
        {
            var mappedData = _mapper.Map<IUDPaymentType>(addPaymentType);
            var response = await _paymentTypeRepo.AddPaymentTypeAsync(mappedData);
            return response;
        }

        /// <summary>
        /// Gets the payment type async.
        /// </summary>
        /// <param name="paymentTypeFilter">The payment type filter.</param>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<PaymentTypeDetails>> GetPaymentTypeAsync(PaymentTypeFilter paymentTypeFilter)
        {
            var response = await _paymentTypeRepo.GetPaymentTypeAsync(paymentTypeFilter);
            return response;
        }

        /// <summary>
        /// Gets the payment type by id async.
        /// </summary>
        /// <param name="PaymentTypeId">The payment type id.</param>
        /// <returns>A Task.</returns>
        public async Task<PaymentTypeDetails> GetPaymentTypeByIdAsync(int PaymentTypeId)
        {
            var response = await _paymentTypeRepo.GetPaymentTypeByIdAsync(PaymentTypeId);
            return response;
        }

        /// <summary>
        /// Removes the payment type async.
        /// </summary>
        /// <param name="removePaymentType">The remove payment type.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> RemovePaymentTypeAsync(UpdatePaymentTypeVm removePaymentType)
        {
            var mappedData = _mapper.Map<IUDPaymentType>(removePaymentType);
            var response = await _paymentTypeRepo.RemovePaymentTypeAsync(mappedData);
            return response;
        }

        /// <summary>
        /// Updates the payment type async.
        /// </summary>
        /// <param name="updatePaymentType">The update payment type.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> UpdatePaymentTypeAsync(UpdatePaymentTypeVm updatePaymentType)
        {
            var mappedData = _mapper.Map<IUDPaymentType>(updatePaymentType);
            var response = await _paymentTypeRepo.UpdatePaymentTypeAsync(mappedData);
            return response;
        }
    }
}
