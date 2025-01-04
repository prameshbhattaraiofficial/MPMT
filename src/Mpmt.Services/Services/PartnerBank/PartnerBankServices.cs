using AutoMapper;
using Mpmt.Core.Dtos.PartnerBank;
using Mpmt.Core.ViewModel.PartnerBank;
using Mpmt.Data.Repositories.PartnerBank;
using Mpmt.Services.Services.Common;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.Services.PartnerBank
{
    /// <summary>
    /// The partner bank services.
    /// </summary>
    public class PartnerBankServices : BaseService, IPartnerBankServices
    {
        private readonly IPartnerBankRepo _partnerBankRepo;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PartnerBankServices"/> class.
        /// </summary>
        /// <param name="partnerBankRepo">The partner bank repo.</param>
        /// <param name="mapper">The mapper.</param>
        public PartnerBankServices(IPartnerBankRepo partnerBankRepo, IMapper mapper)
        {
            _partnerBankRepo = partnerBankRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Adds the partner bank async.
        /// </summary>
        /// <param name="addPartnerBank">The add partner bank.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> AddPartnerBankAsync(AddPartnerBankVm addPartnerBank)
        {
            var mappedData = _mapper.Map<IUDPartnerBank>(addPartnerBank);
            var response = await _partnerBankRepo.AddPartnerBankAsync(mappedData);
            return response;
        }

        /// <summary>
        /// Gets the partner bank async.
        /// </summary>
        /// <param name="partnerBankFilter">The partner bank filter.</param>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<PartnerBankDetails>> GetPartnerBankAsync(PartnerBankFilter partnerBankFilter)
        {
            var response = await _partnerBankRepo.GetPartnerBankAsync(partnerBankFilter);
            return response;
        }

        /// <summary>
        /// Gets the partner bank by partner id async.
        /// </summary>
        /// <param name="partnerId">The partner id.</param>
        /// <returns>A Task.</returns>
        public async Task<PartnerBankDetails> GetPartnerBankByPartnerIdAsync(int partnerId)
        {
            var response = await _partnerBankRepo.GetPartnerBankByPartnerIdAsync(partnerId);
            return response;
        }

        /// <summary>
        /// Removes the partner bank async.
        /// </summary>
        /// <param name="removePartnerBank">The remove partner bank.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> RemovePartnerBankAsync(UpdatePartnerBankVm removePartnerBank)
        {
            var mappedData = _mapper.Map<IUDPartnerBank>(removePartnerBank);
            var response = await _partnerBankRepo.RemovePartnerBankAsync(mappedData);
            return response;
        }

        /// <summary>
        /// Updates the partner bank async.
        /// </summary>
        /// <param name="updatePartnerBank">The update partner bank.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> UpdatePartnerBankAsync(UpdatePartnerBankVm updatePartnerBank)
        {
            var mappedData = _mapper.Map<IUDPartnerBank>(updatePartnerBank);
            var response = await _partnerBankRepo.UpdatePartnerBankAsync(mappedData);
            return response;
        }
    }
}
