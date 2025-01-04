using Dapper;
using Mpmt.Core.Dtos.AgentList;
using System.Data;
using Mpmt.Data.Common;
using Mpmt.Core.Dtos.Paging;
using Mpmt.Core.Dtos.PartnerReport;
using AutoMapper;

namespace Mpmt.Data.Repositories.AgentList;

public class AgentListRepository : IAgentListRepository
{
    private readonly IMapper _mapper;

    public AgentListRepository(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<PagedList<AgentListDetail>> GetAgentListAsync(GetAgentListRequest request)
    {
        try
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();

            param.Add("@AgentName", request.AgentName);
            param.Add("@DistrictCode", request.DistrictCode);
            param.Add("@Export", request.Export);

            param.Add("@PageNumber", request.PageNumber);
            param.Add("@PageSize", request.PageSize);
            param.Add("@SortingCol", request.SortOrder);
            param.Add("@SortType", request.SortBy);
            param.Add("@SearchVal", request.SearchVal);

            var data = await connection
                .QueryMultipleAsync("[dbo].[sp_get_remit_CashAgent_for_website]", param: param, commandType: CommandType.StoredProcedure);

            var agentList = await data.ReadAsync<AgentListDetail>();
            var pagedInfo = await data.ReadFirstAsync<PageInfo>();
            var mappeddata = _mapper.Map<PagedList<AgentListDetail>>(pagedInfo);
            mappeddata.Items = agentList;
            return mappeddata;
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}