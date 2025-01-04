using Microsoft.AspNetCore.Mvc;
using Mpmt.Core.Domain;
using Mpmt.Core.Dtos;
using System.Net;

namespace Mpmt.PublicApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        protected virtual IActionResult HandleResponseFromStatusCode<TResponse>(HttpStatusCode statusCode, TResponse response)
        {
            return statusCode switch
            {
                HttpStatusCode.OK => StatusCode(StatusCodes.Status200OK, response),
                HttpStatusCode.Created => StatusCode(StatusCodes.Status201Created, response),
                HttpStatusCode.NonAuthoritativeInformation => StatusCode(StatusCodes.Status203NonAuthoritative, response),
                HttpStatusCode.NoContent => StatusCode(StatusCodes.Status204NoContent, response),

                HttpStatusCode.BadRequest => StatusCode(StatusCodes.Status400BadRequest, response),
                HttpStatusCode.Unauthorized => StatusCode(StatusCodes.Status401Unauthorized, response),
                HttpStatusCode.PaymentRequired => StatusCode(StatusCodes.Status402PaymentRequired, response),
                HttpStatusCode.Forbidden => StatusCode(StatusCodes.Status403Forbidden, response),
                HttpStatusCode.NotFound => StatusCode(StatusCodes.Status404NotFound, response),
                HttpStatusCode.MethodNotAllowed => StatusCode(StatusCodes.Status405MethodNotAllowed, response),
                HttpStatusCode.NotAcceptable => StatusCode(StatusCodes.Status406NotAcceptable, response),
                HttpStatusCode.Conflict => StatusCode(StatusCodes.Status409Conflict, response),
                HttpStatusCode.TooManyRequests => StatusCode(StatusCodes.Status429TooManyRequests, response),

                HttpStatusCode.InternalServerError => StatusCode(StatusCodes.Status500InternalServerError, response),
                HttpStatusCode.NotImplemented => StatusCode(StatusCodes.Status501NotImplemented, response),
                HttpStatusCode.BadGateway => StatusCode(StatusCodes.Status502BadGateway, response),
                HttpStatusCode.ServiceUnavailable => StatusCode(StatusCodes.Status503ServiceUnavailable, response),
                HttpStatusCode.GatewayTimeout => StatusCode(StatusCodes.Status504GatewayTimeout, response),

                _ => StatusCode(StatusCodes.Status400BadRequest, response),
            };
        }

        protected virtual ActionResult<ApiResponse> HandleResponseFromMpmtResult(MpmtResult result)
        {
            var response = new ApiResponse
            {
                ResponseStatus = ResponseStatuses.Success,
                ResponseCode = ResponseCodes.Code200_Success,
                ResponseMessage = result.Message ?? "Success!"
            };

            if (result.Success)
                return StatusCode(StatusCodes.Status200OK, response);

            response.ResponseStatus = ResponseStatuses.Error;
            response.ResponseCode = ResponseCodes.Code400_BadRequest;
            response.ResponseMessage = result.Errors.First() ?? ResponseMessages.Msg400_BadRequest;

            if (result.ResultCode is >= 400 and < 600)
            {
                response.ResponseCode = result.ResultCode.ToString();

                return result.ResultCode switch
                {
                    StatusCodes.Status200OK => StatusCode(StatusCodes.Status200OK, response),
                    StatusCodes.Status201Created => StatusCode(StatusCodes.Status201Created, response),
                    StatusCodes.Status203NonAuthoritative => StatusCode(StatusCodes.Status203NonAuthoritative, response),
                    StatusCodes.Status204NoContent => StatusCode(StatusCodes.Status204NoContent, response),

                    StatusCodes.Status400BadRequest => StatusCode(StatusCodes.Status400BadRequest, response),
                    StatusCodes.Status401Unauthorized => StatusCode(StatusCodes.Status401Unauthorized, response),
                    StatusCodes.Status402PaymentRequired => StatusCode(StatusCodes.Status402PaymentRequired, response),
                    StatusCodes.Status403Forbidden => StatusCode(StatusCodes.Status403Forbidden, response),
                    StatusCodes.Status404NotFound => StatusCode(StatusCodes.Status404NotFound, response),
                    StatusCodes.Status405MethodNotAllowed => StatusCode(StatusCodes.Status405MethodNotAllowed, response),
                    StatusCodes.Status406NotAcceptable => StatusCode(StatusCodes.Status406NotAcceptable, response),
                    StatusCodes.Status409Conflict => StatusCode(StatusCodes.Status409Conflict, response),
                    StatusCodes.Status429TooManyRequests => StatusCode(StatusCodes.Status429TooManyRequests, response),

                    StatusCodes.Status500InternalServerError => StatusCode(StatusCodes.Status500InternalServerError, response),
                    StatusCodes.Status501NotImplemented => StatusCode(StatusCodes.Status501NotImplemented, response),
                    StatusCodes.Status502BadGateway => StatusCode(StatusCodes.Status502BadGateway, response),
                    StatusCodes.Status503ServiceUnavailable => StatusCode(StatusCodes.Status503ServiceUnavailable, response),
                    StatusCodes.Status504GatewayTimeout => StatusCode(StatusCodes.Status504GatewayTimeout, response),

                    _ => StatusCode(StatusCodes.Status400BadRequest, response),
                };
            }

            return StatusCode(StatusCodes.Status400BadRequest, response);
        }
    }
}
