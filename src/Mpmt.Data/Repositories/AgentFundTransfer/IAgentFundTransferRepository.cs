using Mpmt.Core.Dtos.AgentFundTransfer;
using Mpmts.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Data.Repositories.AgentFundTransfer
{
    public interface IAgentFundTransferRepository
    {
        Task<(GetAgentFundRequestList, SprocMessage)> AgentFundRequest(AgentFundTransferDto model);
        Task<SprocMessage> FundRequest(AgentFundTransferDto model);
        Task<AgentFundTransferDto> GetFundTransferDetailAsync(string agentCode);
    }
}
