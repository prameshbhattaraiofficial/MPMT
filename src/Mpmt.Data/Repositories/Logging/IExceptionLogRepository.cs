using Mpmt.Core.Dtos.Logging;

namespace Mpmt.Data.Repositories.Logging
{
    public interface IExceptionLogRepository
    {
        Task AddAsync(ExceptionLogParam logParam);
    }
}
