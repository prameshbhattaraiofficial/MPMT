using AutoMapper;
using Mpmt.Core.Dtos.KYCRemark;
using Mpmt.Core.ViewModel.KYCRemark;
using Mpmt.Data.Repositories.KYCRemark;
using Mpmt.Services.Services.Common;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.Services.KYCRemark
{
    /// <summary>
    /// The kyc remark services.
    /// </summary>
    public class KycRemarkServices : BaseService, IKycRemarkServices
    {
        private readonly IKycRemarkRepo _kycRemarkRepo;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="KycRemarkServices"/> class.
        /// </summary>
        /// <param name="kycRemarkRepo">The kyc remark repo.</param>
        /// <param name="mapper">The mapper.</param>
        public KycRemarkServices(IKycRemarkRepo kycRemarkRepo, IMapper mapper)
        {
            _kycRemarkRepo = kycRemarkRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Adds the kyc remark async.
        /// </summary>
        /// <param name="addKycRemark">The add kyc remark.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> AddKycRemarkAsync(AddKycRemarkVm addKycRemark)
        {
            var mappedData = _mapper.Map<IUDKycRemark>(addKycRemark);
            var response = await _kycRemarkRepo.AddKycRemarkAsync(mappedData);
            return response;
        }

        /// <summary>
        /// Gets the kyc remark async.
        /// </summary>
        /// <param name="kycRemarkFilter">The kyc remark filter.</param>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<KycRemarkDetails>> GetKycRemarkAsync(KycRemarkFilter kycRemarkFilter)
        {
            var response = await _kycRemarkRepo.GetKycRemarkAsync(kycRemarkFilter);
            return response;
        }

        /// <summary>
        /// Gets the kyc remark by id async.
        /// </summary>
        /// <param name="kycRemarkId">The kyc remark id.</param>
        /// <returns>A Task.</returns>
        public async Task<KycRemarkDetails> GetKycRemarkByIdAsync(int kycRemarkId)
        {
            var response = await _kycRemarkRepo.GetKycRemarkByIdAsync(kycRemarkId);
            return response;
        }

        /// <summary>
        /// Removes the kyc remark async.
        /// </summary>
        /// <param name="removeKycRemark">The remove kyc remark.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> RemoveKycRemarkAsync(UpdateKycRemarkVm removeKycRemark)
        {
            var mappedData = _mapper.Map<IUDKycRemark>(removeKycRemark);
            var response = await _kycRemarkRepo.RemoveKycRemarkAsync(mappedData);
            return response;
        }

        /// <summary>
        /// Updates the kyc remark async.
        /// </summary>
        /// <param name="updateKycRemark">The update kyc remark.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> UpdateKycRemarkAsync(UpdateKycRemarkVm updateKycRemark)
        {
            var mappedData = _mapper.Map<IUDKycRemark>(updateKycRemark);
            var response = await _kycRemarkRepo.UpdateKycRemarkAsync(mappedData);
            return response;
        }
    }
}
