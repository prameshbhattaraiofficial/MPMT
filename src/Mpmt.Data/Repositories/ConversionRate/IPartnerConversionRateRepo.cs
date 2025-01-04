using Mpmt.Core.Dtos.ConversionRate;
using Mpmts.Core.Dtos;

namespace Mpmt.Data.Repositories.ConversionRate
{
    public interface IPartnerConversionRateRepo
    {
        Task<IEnumerable<PartnerConversionRate>> GetConversionRateAsync(PartnerConversionRateFilter conversionRateFilter);
        Task<(List<PartnerConversionRateDetails>, PartnerConversionRate)> GetConversionRateDetailAsync(PartnerConversionRateFilter conversionRateFilter);
        Task<(List<PartnerConversionRateDetails>, PartnerConversionRate)> ViewConversionRateDetailAsync(PartnerConversionRateFilter conversionRateFilter);
        Task<SprocMessage> AddConversionRateAsync(List<AddPartnerConversionRate> addConversionRate, PartnerConversionRate partnerConversionRate);
        Task<SprocMessage> RemoveConversionRateAsync(AddPartnerConversionRate removeConversionRate);
    }
}
