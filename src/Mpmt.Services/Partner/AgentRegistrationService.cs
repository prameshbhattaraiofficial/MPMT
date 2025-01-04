using AutoMapper;
using Microsoft.AspNetCore.Http;
using Mpmt.Core.Domain.Agents;
using Mpmt.Core.Dtos;
using Mpmt.Core.Models.Agents;
using Mpmt.Data.Repositories.Agents;
using Mpmt.Services.Services.Common;

namespace Mpmt.Services.Partner;

public class AgentRegistrationService : BaseService, IAgentRegistrationService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly IAgentRegistrationRepository _agentRegistrationRepository;

    public AgentRegistrationService(IMapper mapper, IHttpContextAccessor httpContextAccessor, IAgentRegistrationRepository agentRegistrationRepository)
    {
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _agentRegistrationRepository = agentRegistrationRepository;
    }

    public async Task<MpmtResult> InsertAsync(AgentRegistration request)
    {
        var application = _mapper.Map<AgentRegister>(request);
        application.IpAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress.ToString();

        var submitResult = await _agentRegistrationRepository.InsertAsync(application);
        return MapSprocMessageToMpmtResult(submitResult);
    }
}
