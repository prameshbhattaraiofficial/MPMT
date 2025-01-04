using Mpmt.Core.Domain.Public.Feedbacks;
using Mpmts.Core.Dtos;

namespace Mpmt.Data.Repositories.Public
{
    public interface IFeedbackRepository
    {
        Task<SprocMessage> InsertPublicFeedbackAsync(PublicFeedback publicFeedback);
    }
}
