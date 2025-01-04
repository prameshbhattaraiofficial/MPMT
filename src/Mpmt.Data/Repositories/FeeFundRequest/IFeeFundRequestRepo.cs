using Mpmt.Core.Dtos.FeeFundRequest;
using Mpmt.Core.Dtos.Paging;

namespace Mpmt.Data.Repositories.FeeFundRequest;

public interface IFeeFundRequestRepo
{
    Task<PagedList<FeeFundRequestList>> GetFeeFundRequestAsync(FeeFundRequestFilter requestFilter);
}
