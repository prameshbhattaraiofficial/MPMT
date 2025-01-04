using Mpmt.Core.Dtos.FeeFundRequest;
using Mpmt.Core.Dtos.Paging;

namespace Mpmt.Services.Services.FeeFundRequest;

public interface IFeeFundRequestService
{
    Task<PagedList<FeeFundRequestList>> GetFeeFundRequestAsync(FeeFundRequestFilter requestFilter);
}
