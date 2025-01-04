namespace Mpmt.Services.Authentication
{
    public interface IUserAuthSessionService
    {
        Task<bool> AddToExpirationAsync(SessionAuthExpiration sessionAuthExpiration);

        Task<bool> ValidateAsync(string userUniqueId, string issueDate);
    }
}
