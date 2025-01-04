using AspNetCoreRateLimit;
using LazyCache;
using Microsoft.Extensions.Caching.Memory;

namespace Mpmt.Web.Features.RateLimiting
{
    public class LazyCacheRateLimitCounterStore : IRateLimitCounterStore
    {
        IAppCache _appCache;

        public LazyCacheRateLimitCounterStore()
        {
            _appCache = new CachingService();
        }

        public Task<bool> ExistsAsync(string id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_appCache.TryGetValue<object>(id, out var _));
        }

        public Task<RateLimitCounter?> GetAsync(string id, CancellationToken cancellationToken = default)
        {
            _appCache.TryGetValue<RateLimitCounter>(id, out var counter);
            RateLimitCounter? rateLimitCounter = counter;

            return Task.FromResult(rateLimitCounter);
        }

        public Task RemoveAsync(string id, CancellationToken cancellationToken = default)
        {
            _appCache.Remove(id);

            return Task.CompletedTask;
        }

        public Task SetAsync(string id, RateLimitCounter? entry, TimeSpan? expirationTime = null, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(id) || entry is null)
                return Task.CompletedTask;

            _appCache.Add(id, entry, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expirationTime
            });

            return Task.CompletedTask;
        }
    }
}
