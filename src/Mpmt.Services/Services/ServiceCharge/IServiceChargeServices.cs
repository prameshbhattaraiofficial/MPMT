using Mpmt.Core.Dtos.ServiceCharge;
using Mpmt.Core.ViewModel.ServiceCharge;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.Services.ServiceCharge
{
    public interface IServiceChargeServices
    {
        Task<IEnumerable<ServiceChargeDetails>> GetServiceChargeAsync(ServiceChargeFilter serviceChargeFilter);
        Task<SprocMessage> AddServiceChargeAsync(List<AddServiceChargeVm> addServiceCharges, int chargeategoryid, int paymenttypeid, string sourcecurrency, string destinationcurrency);
        //Task<ServiceChargeDetails> GetServiceChargeByIdAsync(int serviceChargeId);
        Task<(List<ServiceChargeList>, ServiceChargeSelect)> GetServiceChargeByIdAsync(int CategoryId, string SourceCurrency, string DestinationCurrency, int PaymentTypeId);
        Task<SprocMessage> UpdateServiceChargeAsync(List<AddServiceChargeVm> updateServiceCharge, int chargeategoryid, int paymenttypeid, string sourcecurrency, string destinationcurrency);
        Task<SprocMessage> RemoveServiceChargeAsync(ServiceChargeSelect serviceChargeSelect);
    }
}
