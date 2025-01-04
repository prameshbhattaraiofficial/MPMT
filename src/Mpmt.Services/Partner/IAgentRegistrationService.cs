using Mpmt.Core.Dtos;
using Mpmt.Core.Models.Agents;

namespace Mpmt.Services.Partner;

public interface IAgentRegistrationService
{
    Task<MpmtResult> InsertAsync(AgentRegistration request);
}
