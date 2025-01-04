using Mpmt.Core.Dtos.ConversionRate;
using Mpmt.Core.ViewModel.ConversionRate;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.Services.ConversionRate
{
    public interface IPartnerConversionRateServices
    {
        Task<IEnumerable<PartnerConversionRate>> GetConversionRateAsync(PartnerConversionRateFilter conversionRateFilter);
        Task<SprocMessage> AddConversionRateAsync(List<AddPartnerConversionRateVm> addConversionRate, PartnerConversionRate partnerConversionRate);
        Task<SprocMessage> RemoveConversionRateAsync(AddPartnerConversionRateVm removeConversionRate);
        Task<(List<PartnerConversionRateDetails>, PartnerConversionRate)> GetConversionRateDetailAsync(PartnerConversionRateFilter conversionRateFilter);
        Task<(List<PartnerConversionRateDetails>, PartnerConversionRate)> ViewConversionRateDetailAsync(PartnerConversionRateFilter conversionRateFilter);
    }
}
