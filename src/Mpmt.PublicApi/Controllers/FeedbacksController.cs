using Microsoft.AspNetCore.Mvc;
using Mpmt.Core.Domain;
using Mpmt.Core.Models.Public.Feedbacks;
using Mpmt.Services.Public.Feedbacks;

namespace Mpmt.PublicApi.Controllers
{
    public class FeedbacksController : BaseApiController
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbacksController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [HttpPost("public")]
        public async Task<ActionResult<ApiResponse>> PublicFeedback(PublicFeedbackRequest request)
        {
            var result = await _feedbackService.InsertPublicFeedBackAsync(request);

            return HandleResponseFromMpmtResult(result);
        }
    }
}
