using LazyCache;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Mpmt.Core.Configuration;

namespace Mpmt.Services.Authentication
{
    public class UserAuthSessionService : IUserAuthSessionService
    {
        private const string _prefixSessionExpire = "SessionExpire-";
        private readonly IAppCache _appCache;
        private readonly CookieAuthOptions _cookieAuthOptions;

        public UserAuthSessionService(IAppCache appCache, IConfiguration config)
        {
            _appCache = appCache;
            _cookieAuthOptions = config.GetSection(CookieAuthOptions.SectionName).Get<CookieAuthOptions>();
        }

        public Task<bool> AddToExpirationAsync(SessionAuthExpiration sessionAuthExpiration)
        {
            if (sessionAuthExpiration is null)
                return Task.FromResult(false);

            if (string.IsNullOrWhiteSpace(sessionAuthExpiration.UserUniqueId) || !sessionAuthExpiration.ExpireBefore.HasValue)
                return Task.FromResult(false);

            _appCache.Add($"{_prefixSessionExpire}{sessionAuthExpiration.UserUniqueId}",
                sessionAuthExpiration,
                new MemoryCacheEntryOptions
                {
                    Priority = CacheItemPriority.NeverRemove,
                    AbsoluteExpirationRelativeToNow = DateTime.Now
                        .AddDays(_cookieAuthOptions.ExpirationDays)
                        .AddHours(_cookieAuthOptions.ExpirationHours)
                        .AddMinutes(_cookieAuthOptions.ExpirationMinutes + 1) // +1 => add an extra min
                        .AddSeconds(_cookieAuthOptions.ExpirationSeconds) - DateTime.Now
                });

            return Task.FromResult(true);
        }

        public Task<bool> ValidateAsync(string userUniqueId, string issueDate)
        {
            if (string.IsNullOrWhiteSpace(issueDate))
                return Task.FromResult(false);

            if (!DateTime.TryParse(issueDate, out var issuedDateParsed))
                return Task.FromResult(false);

            _appCache.TryGetValue<SessionAuthExpiration>($"{_prefixSessionExpire}{userUniqueId}", out var authSessionExp);

            if (authSessionExp is null)
                return Task.FromResult(true);

            if (!authSessionExp.ExpireBefore.HasValue)
                return Task.FromResult(false);

            // if issuedDateParsed is <= ExpireBefore, return false to indicate invalid
            if (issuedDateParsed <= authSessionExp.ExpireBefore)
                return Task.FromResult(false);
            
            return Task.FromResult(true);
        }
    }
}
