using Mpmt.Core.Domain;

namespace Mpmt.Core.Dtos.PartnerApi
{
    public class TxnProcessIdResponse : ApiResponse
    {
        private string _processId;

        public string ProcessId { get => _processId ?? string.Empty; set => _processId = value; }
    }
}
