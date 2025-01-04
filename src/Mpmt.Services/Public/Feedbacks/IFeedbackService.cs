using Mpmt.Core.Dtos;
using Mpmt.Core.Models.Public.Feedbacks;

namespace Mpmt.Services.Public.Feedbacks
{
    public interface IFeedbackService
    {
        Task<MpmtResult> InsertPublicFeedBackAsync(PublicFeedbackRequest request);
    }
}
