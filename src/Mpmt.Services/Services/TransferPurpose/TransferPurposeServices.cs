using AutoMapper;
using Mpmt.Core.Dtos.TransferPurpose;
using Mpmt.Core.ViewModel.TransferPurpose;
using Mpmt.Data.Repositories.TransferPurpose;
using Mpmt.Services.Services.Common;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.Services.TransferPurpose
{
    /// <summary>
    /// The transfer purpose services.
    /// </summary>
    public class TransferPurposeServices : BaseService, ITransferPurposeServices
    {
        private readonly ITransferPurposeRepo _transferPurposeRepo;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransferPurposeServices"/> class.
        /// </summary>
        /// <param name="transferPurposeRepo">The transfer purpose repo.</param>
        /// <param name="mapper">The mapper.</param>
        public TransferPurposeServices(ITransferPurposeRepo transferPurposeRepo, IMapper mapper)
        {
            _transferPurposeRepo = transferPurposeRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Adds the transfer purpose async.
        /// </summary>
        /// <param name="addTransferPurpose">The add transfer purpose.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> AddTransferPurposeAsync(AddTransferVm addTransferPurpose)
        {
            var mappedData = _mapper.Map<IUDTransferPurpose>(addTransferPurpose);
            var response = await _transferPurposeRepo.AddTransferPurposeAsync(mappedData);
            return response;
        }

        /// <summary>
        /// Gets the transfer purpose async.
        /// </summary>
        /// <param name="transferPurposeFilter">The transfer purpose filter.</param>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<TransferPurposeDetails>> GetTransferPurposeAsync(TransferPurposeFilter transferPurposeFilter)
        {
            var response = await _transferPurposeRepo.GetTransferPurposeAsync(transferPurposeFilter);
            return response;
        }

        /// <summary>
        /// Gets the transfer purpose by id async.
        /// </summary>
        /// <param name="transferPurposeId">The transfer purpose id.</param>
        /// <returns>A Task.</returns>
        public async Task<TransferPurposeDetails> GetTransferPurposeByIdAsync(int transferPurposeId)
        {
            var response = await _transferPurposeRepo.GetTransferPurposeByIdAsync(transferPurposeId);
            return response;
        }

        /// <summary>
        /// Removes the transfer purpose async.
        /// </summary>
        /// <param name="removeTransferPurpose">The remove transfer purpose.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> RemoveTransferPurposeAsync(UpdateTransferVm removeTransferPurpose)
        {
            var mappedData = _mapper.Map<IUDTransferPurpose>(removeTransferPurpose);
            var response = await _transferPurposeRepo.RemoveTransferPurposeAsync(mappedData);
            return response;
        }

        /// <summary>
        /// Updates the transfer purpose async.
        /// </summary>
        /// <param name="updateTransferPurpose">The update transfer purpose.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> UpdateTransferPurposeAsync(UpdateTransferVm updateTransferPurpose)
        {
            var mappedData = _mapper.Map<IUDTransferPurpose>(updateTransferPurpose);
            var response = await _transferPurposeRepo.UpdateTransferPurposeAsync(mappedData);
            return response;
        }
    }
}
