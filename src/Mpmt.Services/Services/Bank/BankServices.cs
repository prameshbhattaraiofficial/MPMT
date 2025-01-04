using AutoMapper;
using Mpmt.Core.Dtos.Banks;
using Mpmt.Core.ViewModel.Bank;
using Mpmt.Data.Repositories.Bank;
using Mpmt.Services.Services.Common;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.Services.Bank
{
    /// <summary>
    /// The bank services.
    /// </summary>
    public class BankServices : BaseService, IBankServices
    {
        private readonly IBankRepo _bankRepo;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="BankServices"/> class.
        /// </summary>
        /// <param name="bankRepo">The bank repo.</param>
        /// <param name="mapper">The mapper.</param>
        public BankServices(IBankRepo bankRepo, IMapper mapper)
        {
            _bankRepo = bankRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Adds the bank async.
        /// </summary>
        /// <param name="addbank">The addbank.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> AddBankAsync(AddBankVm addbank)
        {
            var mappeddata = _mapper.Map<IUDBank>(addbank);
            var response = await _bankRepo.AddBankAsync(mappeddata);
            return response;
        }

        /// <summary>
        /// Gets the bank async.
        /// </summary>
        /// <param name="bankFilter">The bank filter.</param>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<BankDetails>> GetBankAsync(BankFilter bankFilter)
        {
            var response = await _bankRepo.GetBankAsync(bankFilter);

            return response;
        }

        /// <summary>
        /// Gets the bank by id async.
        /// </summary>
        /// <param name="BankId">The bank id.</param>
        /// <returns>A Task.</returns>
        public async Task<BankDetails> GetBankByIdAsync(int BankId)
        {
            var response = await _bankRepo.GetBankByIdAsync(BankId);
            return response;
        }

        /// <summary>
        /// Removes the bank async.
        /// </summary>
        /// <param name="Removebank">The removebank.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> RemoveBankAsync(UpdateBankVm Removebank)
        {
            var mappeddata = _mapper.Map<IUDBank>(Removebank);
            var response = await _bankRepo.RemoveBankAsync(mappeddata);
            return response;
        }

        /// <summary>
        /// Updates the bank async.
        /// </summary>
        /// <param name="Updatebank">The updatebank.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> UpdateBankAsync(UpdateBankVm Updatebank)
        {
            var mappeddata = _mapper.Map<IUDBank>(Updatebank);
            var response = await _bankRepo.UpdateBankAsync(mappeddata);
            return response;
        }
    }
}
