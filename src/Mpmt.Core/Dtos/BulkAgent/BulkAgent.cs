using Microsoft.AspNetCore.Http;

namespace Mpmt.Core.Dtos.BulkAgent;

public class BulkAgent
{
    public IFormFile UploadBulkAgentFile { get; set; }
}

public class BulkAgentDetailModel
{
    public string SN { get; set; }
    public string District { get; set; }
    public string AgentName { get; set; }
    public string Address { get; set; }
}