using Microsoft.AspNetCore.Mvc;
using Mpmt.Core.Domain;

namespace Mpmt.Api.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : BaseApiController
    {
        [Route("{statusCode:int}")]
        public IActionResult Error(int statusCode)
        {
            return new ObjectResult(new ApiResponse { ResponseCode = statusCode.ToString() });
        }
    }
}
