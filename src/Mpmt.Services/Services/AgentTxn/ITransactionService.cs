using Mpmt.Core.Dtos.Agent;
using Mpmts.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Services.Services.AgentTxn
{
    public interface ITransactionService
    {
        Task<(AgentTxnModel, SprocMessage)> checkControlNumberAsynce(string controlNumber);
        Task<string> GetProcessIdAsync(string agentCode, string referenceId);
        Task<(AgentPayOutReceipt, SprocMessage)> payoutTransactionByAgentAysnc(AgentTxnModel model);
    }
}
