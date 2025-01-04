using AutoMapper;
using Mpmt.Core.Dtos.ServiceCharge;
using Mpmt.Core.ViewModel.ServiceCharge;
using Mpmt.Data.Repositories.ServiceCharge;
using Mpmt.Services.Services.Common;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.Services.ServiceCharge
{
    public class ServiceChargeServices : BaseService, IServiceChargeServices
    {
        private readonly IServiceChargeRepo _serviceChargeRepo;
        private readonly IMapper _mapper;

        public ServiceChargeServices(IServiceChargeRepo serviceChargeRepo, IMapper mapper)
        {
            _serviceChargeRepo = serviceChargeRepo;
            _mapper = mapper;
        }

        public async Task<SprocMessage> AddServiceChargeAsync(List<AddServiceChargeVm> addServiceCharges, int chargeategoryid, int paymenttypeid, string sourcecurrency, string destinationcurrency)
        {
            var serviceCharges = new List<AddServiceCharges>();
            foreach (var i in addServiceCharges)
            {
                var mappedData = _mapper.Map<AddServiceCharges>(i);
                serviceCharges.Add(mappedData);
            }
            var response = await _serviceChargeRepo.AddServiceChargeAsync(serviceCharges, chargeategoryid, paymenttypeid, sourcecurrency, destinationcurrency);
            return response;
        }

        public async Task<IEnumerable<ServiceChargeDetails>> GetServiceChargeAsync(ServiceChargeFilter serviceChargeFilter)
        {
            var response = await _serviceChargeRepo.GetServiceChargeAsync(serviceChargeFilter);
            return response;
        }

        public async Task<(List<ServiceChargeList>, ServiceChargeSelect)> GetServiceChargeByIdAsync(int CategoryId, string SourceCurrency, string DestinationCurrency, int PaymentTypeId)
        {
            var (response, data) = await _serviceChargeRepo.GetServiceChargeByIdAsync(CategoryId, SourceCurrency, DestinationCurrency, PaymentTypeId);
            return (response, data);
        }

        public async Task<SprocMessage> RemoveServiceChargeAsync(ServiceChargeSelect serviceChargeSelect)
        {
            var response = await _serviceChargeRepo.RemoveServiceChargeAsync(serviceChargeSelect);
            return response;
        }

        public async Task<SprocMessage> UpdateServiceChargeAsync(List<AddServiceChargeVm> updateServiceCharge, int chargeategoryid, int paymenttypeid, string sourcecurrency, string destinationcurrency)
        {
            var mappedData = _mapper.Map<List<AddServiceCharges>>(updateServiceCharge);
            var response = await _serviceChargeRepo.UpdateServiceChargeAsync(mappedData, chargeategoryid, paymenttypeid, sourcecurrency, destinationcurrency);
            return response;
        }
    }
}
