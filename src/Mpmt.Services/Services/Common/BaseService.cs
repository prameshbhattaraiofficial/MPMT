using Mpmt.Core.Dtos;
using Mpmts.Core.Dtos;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;
using System.Text;

namespace Mpmt.Services.Services.Common
{
    /// <summary>
    /// The base service.
    /// </summary>
    public abstract class BaseService
    {
        /// <summary>
        /// Gets the error response from sproc message.
        /// </summary>
        /// <param name="spMsgResponse">The sp msg response.</param>
        /// <returns>A (HttpStatusCode, ResponseDto) .</returns>
        protected (HttpStatusCode, ResponseDto) GetErrorResponseFromSprocMessage(SprocMessage spMsgResponse)
        {
            return spMsgResponse.StatusCode switch
            {
                400 => (HttpStatusCode.BadRequest, new ResponseDto { Success = false, Message = "Bad request!", Errors = new List<string> { spMsgResponse.MsgText } }),
                401 => (HttpStatusCode.Unauthorized, new ResponseDto { Success = false, Message = "Unauthorized!", Errors = new List<string> { spMsgResponse.MsgText } }),
                403 => (HttpStatusCode.Forbidden, new ResponseDto { Success = false, Message = "Forbidden!", Errors = new List<string> { spMsgResponse.MsgText } }),
                404 => (HttpStatusCode.NotFound, new ResponseDto { Success = false, Message = "Not found!", Errors = new List<string> { spMsgResponse.MsgText } }),
                409 => (HttpStatusCode.Conflict, new ResponseDto { Success = false, Message = "Conflict!", Errors = new List<string> { spMsgResponse.MsgText } }),
                _ => (HttpStatusCode.BadRequest, new ResponseDto { Success = false, Message = "Bad request!", Errors = new List<string> { spMsgResponse.MsgText } })
            };
        }

        protected MpmtResult MapSprocMessageToMpmtResult(SprocMessage sprocMessage)
        {
            ArgumentNullException.ThrowIfNull(nameof(sprocMessage));

            var result = new MpmtResult();
            if (sprocMessage.StatusCode != 200)
            {
                result.AddError(sprocMessage.StatusCode, sprocMessage.MsgText);
                return result;
            }

            result.AddSuccess(sprocMessage.StatusCode, sprocMessage.MsgText);
            return result;
        }

        protected virtual StringContent GetJsonStringContent(object inputDataObj)
        {
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() },
                Formatting = Formatting.Indented
            };

            return new StringContent(JsonConvert.SerializeObject(inputDataObj, jsonSerializerSettings), Encoding.UTF8, "application/json");
        }
    }
}
