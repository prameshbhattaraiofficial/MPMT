using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mpmt.Agent.Controllers
{
    /// <summary>
    /// The error controller.
    /// </summary>
    [AllowAnonymous]
    public class ErrorController : AgentBaseController
    {
        /// <summary>
        /// Errors the.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <returns>An IActionResult.</returns>
        [Route("/Error/{statusCode}")]
        public IActionResult Error(int statusCode)
        {
            return statusCode switch
            {
                403 => View("Forbidden"),
                404 => View("NotFound"),
                500 => View("InternalServerError"),
                _ => View("Error")
            };
        }

        /// <summary>
        /// Internals the server error.
        /// </summary>
        /// <returns>An IActionResult.</returns>
        [Route("/InternalServerError")]
        public IActionResult InternalServerError()
        {
            return View("InternalServerError");
        }
    }
}
