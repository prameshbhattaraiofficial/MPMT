using AutoMapper;
using Microsoft.AspNetCore.Http;
using Mpmt.Core.Domain.Public.Feedbacks;
using Mpmt.Core.Dtos;
using Mpmt.Core.Models.Public.Feedbacks;
using Mpmt.Data.Repositories.Public;
using Mpmt.Services.Services.Common;

namespace Mpmt.Services.Public.Feedbacks
{
    public class FeedbackService : BaseService, IFeedbackService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IFeedbackRepository _feedbackRepository;

        public FeedbackService(
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            IFeedbackRepository feedbackRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _feedbackRepository = feedbackRepository;
        }

        public async Task<MpmtResult> InsertPublicFeedBackAsync(PublicFeedbackRequest request)
        {
            var feedback = _mapper.Map<PublicFeedback>(request);
            feedback.IpAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress.ToString();

            var submitResult = await _feedbackRepository.InsertPublicFeedbackAsync(feedback);

            return MapSprocMessageToMpmtResult(submitResult);
        }
    }
}
