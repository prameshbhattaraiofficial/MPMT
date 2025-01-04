using Mpmt.Core.Domain;

namespace Mpmt.Core.Dtos.AgentApi
{
    public class GetProcessIdResponse : ApiResponse
    {
        private string _processId;

        public string ProcessId { get => _processId ?? string.Empty; set => _processId = value; }
    }
}
