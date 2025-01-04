using Mpmt.Core.Dtos.FeeFundRequest;
using Mpmt.Core.Dtos.Paging;
using Mpmt.Data.Repositories.FeeFundRequest;
using Mpmt.Services.Services.Common;

namespace Mpmt.Services.Services.FeeFundRequest;

public class FeeFundRequestService : BaseService, IFeeFundRequestService
{
    private readonly IFeeFundRequestRepo _feeFund;

    public FeeFundRequestService(IFeeFundRequestRepo feeFund)
    {
        _feeFund = feeFund;
    }

    public async Task<PagedList<FeeFundRequestList>> GetFeeFundRequestAsync(FeeFundRequestFilter requestFilter)
    {
        var data = await _feeFund.GetFeeFundRequestAsync(requestFilter);
        return data;
    }
}
