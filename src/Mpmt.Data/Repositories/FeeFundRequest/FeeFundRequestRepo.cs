using AutoMapper;
using Dapper;
using Mpmt.Core.Dtos.FeeFundRequest;
using Mpmt.Core.Dtos.Paging;
using Mpmt.Data.Common;
using System.Data;

namespace Mpmt.Data.Repositories.FeeFundRequest;

public class FeeFundRequestRepo : IFeeFundRequestRepo
{
    private readonly IMapper _mapper;

    public FeeFundRequestRepo(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<PagedList<FeeFundRequestList>> GetFeeFundRequestAsync(FeeFundRequestFilter requestFilter)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();

        var param = new DynamicParameters();
        param.Add("@PartnerCode", requestFilter.PartnerCode);
        param.Add("@TxnId", requestFilter.TxnId);
        param.Add("@PageNumber", requestFilter.PageNumber);
        param.Add("@PageSize", requestFilter.PageSize);
        param.Add("@SortingCol", requestFilter.SortingCol);
        param.Add("@SortType", requestFilter.SortType);
        param.Add("@SearchVal", requestFilter.SearchVal);
        param.Add("@Export", requestFilter.Export);
        var data = await connection
            .QueryMultipleAsync("[dbo].[usp_get_feefundrequestapproved_list]", param: param, commandType: CommandType.StoredProcedure);

        var prefundList = await data.ReadAsync<FeeFundRequestList>();
        var pagedInfo = await data.ReadFirstAsync<PageInfo>();
        var mappeddata = _mapper.Map<PagedList<FeeFundRequestList>>(pagedInfo);
        mappeddata.Items = prefundList;
        return mappeddata;
    }
}
