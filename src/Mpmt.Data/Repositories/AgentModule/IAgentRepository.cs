using Mpmt.Core.Dtos.Agent;
using Mpmt.Core.Dtos.Partner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Data.Repositories.AgentModule
{
    public interface IAgentRepository
    {
        Task<AgentWithCredentials> GetAgentWithCredentialsByApiUserNameAsync(string apiUserName);
    }
}
