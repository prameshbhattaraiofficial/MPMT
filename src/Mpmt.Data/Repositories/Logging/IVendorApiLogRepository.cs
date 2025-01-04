using Mpmt.Core.Dtos.Logging;

namespace Mpmt.Data.Repositories.Logging
{
    public interface IVendorApiLogRepository
    {
        Task LogInsertAsync(VendorApiLogParam log);
        Task LogUpdateAsync(VendorApiLogParam log);
        Task LogUpdateResponseAsync(VendorApiLogParam log);
        Task LogUpdateVendorApiResponseAsync(VendorApiLogParam log);
        Task LogUpdateVendorApiExceptionAsync(VendorApiLogParam log);
        Task LogUpdateVendorApiException2Async(VendorApiLogParam log);
        Task LogUpdateVendorApiException3Async(VendorApiLogParam log);
        Task LogUpdateVendorApiResponse2Async(VendorApiLogParam log);
        Task LogUpdateVendorApiResponse3Async(VendorApiLogParam log);
    }
}
