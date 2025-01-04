using AutoMapper;
using Dapper;
using Mpmt.Core.Domain.Agents;
using Mpmt.Core.Dtos.AgentApplications;
using Mpmt.Core.Dtos.Paging;
using Mpmt.Data.Common;
using Mpmts.Core.Dtos;
using System.Data;

namespace Mpmt.Data.Repositories.Agents;

public class AgentRegistrationRepository : IAgentRegistrationRepository
{
    private readonly IMapper _mapper;

    public AgentRegistrationRepository(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<PagedList<AgentApplicationsModel>> GetAgentApplicationsAsync(AgentApplicationsFilter requestFilter)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();

        var param = new DynamicParameters();
        param.Add("@FullName", requestFilter.FullName);
        param.Add("@StartDate", requestFilter.StartDate);
        param.Add("@EndDate", requestFilter.EndDate);

        param.Add("@PageNumber", requestFilter.PageNumber);
        param.Add("@PageSize", requestFilter.PageSize);
        param.Add("@SortingCol", requestFilter.SortBy);
        param.Add("@SortType", requestFilter.SortOrder);
        param.Add("@SearchVal", requestFilter.SearchVal);
        param.Add("@Export", requestFilter.Export);

        var data = await connection
            .QueryMultipleAsync("[dbo].[usp_get_agent_applications]", param: param, commandType: CommandType.StoredProcedure);

        var agentApplications = await data.ReadAsync<AgentApplicationsModel>();
        var pagedInfo = await data.ReadFirstAsync<PageInfo>();
        var mappeddata = _mapper.Map<PagedList<AgentApplicationsModel>>(pagedInfo);
        mappeddata.Items = agentApplications;
        return mappeddata;
    }

    public async Task<SprocMessage> InsertAsync(AgentRegister request)
    {
        const string operationMode = "I";
        using var connection = DbConnectionManager.GetDefaultConnection();

        var param = new DynamicParameters();
        param.Add("@Operation", operationMode);
        param.Add("@FirstName", request.FirstName);
        param.Add("@LastName", request.LastName);
        param.Add("@ContactNumber", request.ContactNumber);
        param.Add("@Address", request.Address);
        param.Add("@DistrictCode", request.DristrictCode);
        param.Add("@District", request.Dristrict);
        param.Add("@TypeOfBusiness", request.TypeOfBusiness);
        param.Add("@Message", request.Message);
        param.Add("@IpAddress", request.IpAddress);

        param.Add("@ReturnPrimaryId", dbType: DbType.Int32, direction: ParameterDirection.Output);
        param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
        param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
        param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

        _ = await connection.ExecuteAsync("[dbo].[usp_agent_registration]", param, commandType: CommandType.StoredProcedure);

        var identityVal = param.Get<int>("@ReturnPrimaryId");
        var statusCode = param.Get<int>("@StatusCode");
        var msgType = param.Get<string>("@MsgType");
        var msgText = param.Get<string>("@MsgText");

        return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
    }
}
