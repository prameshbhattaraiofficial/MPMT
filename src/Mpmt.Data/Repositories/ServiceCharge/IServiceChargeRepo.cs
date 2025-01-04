using Mpmt.Core.Dtos.ServiceCharge;
using Mpmts.Core.Dtos;

namespace Mpmt.Data.Repositories.ServiceCharge
{
    public interface IServiceChargeRepo
    {
        Task<IEnumerable<ServiceChargeDetails>> GetServiceChargeAsync(ServiceChargeFilter serviceChargeFilter);
        Task<SprocMessage> AddServiceChargeAsync(List<AddServiceCharges> addServiceCharge, int chargeategoryid, int paymenttypeid, string sourcecurrency, string destinationcurrency);
        //Task<ServiceChargeDetails> GetServiceChargeByIdAsync(int serviceChargeId);
        Task<SprocMessage> UpdateServiceChargeAsync(List<AddServiceCharges> updateServiceCharge, int chargeategoryid, int paymenttypeid, string sourcecurrency, string destinationcurrency);
        Task<SprocMessage> RemoveServiceChargeAsync(ServiceChargeSelect serviceChargeSelect);
        Task<(List<ServiceChargeList>, ServiceChargeSelect)> GetServiceChargeByIdAsync(int CategoryId, string SourceCurrency, string DestinationCurrency, int PaymentTypeId);
    }
}
