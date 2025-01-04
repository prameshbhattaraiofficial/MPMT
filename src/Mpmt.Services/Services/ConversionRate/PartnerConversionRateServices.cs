using AutoMapper;
using Mpmt.Core.Dtos.ConversionRate;
using Mpmt.Core.ViewModel.ConversionRate;
using Mpmt.Data.Repositories.ConversionRate;
using Mpmt.Services.Services.Common;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.Services.ConversionRate
{
    public class PartnerConversionRateServices : BaseService, IPartnerConversionRateServices
    {
        private readonly IPartnerConversionRateRepo _partnerConversionRateRepo;
        private readonly IMapper _mapper;

        public PartnerConversionRateServices(IPartnerConversionRateRepo partnerConversionRateRepo, IMapper mapper)
        {
            _partnerConversionRateRepo = partnerConversionRateRepo;
            _mapper = mapper;
        }

        public async Task<SprocMessage> AddConversionRateAsync(List<AddPartnerConversionRateVm> addConversionRate, PartnerConversionRate partnerConversionRate)
        {
            var addConversionRates = new List<AddPartnerConversionRate>();
            foreach (var i in addConversionRate)
            {
                var mappedData = _mapper.Map<AddPartnerConversionRate>(i);
                addConversionRates.Add(mappedData);
            }
            var response = await _partnerConversionRateRepo.AddConversionRateAsync(addConversionRates, partnerConversionRate);
            return response;
        }

        public async Task<IEnumerable<PartnerConversionRate>> GetConversionRateAsync(PartnerConversionRateFilter conversionRateFilter)
        {
            var response = await _partnerConversionRateRepo.GetConversionRateAsync(conversionRateFilter);
            return response;
        }

        public async Task<(List<PartnerConversionRateDetails>, PartnerConversionRate)> GetConversionRateDetailAsync(PartnerConversionRateFilter conversionRateFilter)
        {
            var (response, data) = await _partnerConversionRateRepo.GetConversionRateDetailAsync(conversionRateFilter);
            return (response, data);
        }

        public Task<SprocMessage> RemoveConversionRateAsync(AddPartnerConversionRateVm removeConversionRate)
        {
            throw new NotImplementedException();
        }

        public async Task<(List<PartnerConversionRateDetails>, PartnerConversionRate)> ViewConversionRateDetailAsync(PartnerConversionRateFilter conversionRateFilter)
        {
            var (response, data) = await _partnerConversionRateRepo.ViewConversionRateDetailAsync(conversionRateFilter);
            return (response, data);
        }
    }
}
