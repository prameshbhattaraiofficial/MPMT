using AspNetCoreRateLimit;
using LazyCache;
using Microsoft.Extensions.Caching.Memory;

namespace Mpmt.Web.Features.RateLimiting
{
    public class LazyCacheCachePolicyStore : IIpPolicyStore
    {
        IAppCache _appCache;
        public LazyCacheCachePolicyStore()
        {
            _appCache = new CachingService();
        }

        public Task<bool> ExistsAsync(string id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_appCache.TryGetValue<object>(id, out var _));
        }

        public Task<IpRateLimitPolicies> GetAsync(string id, CancellationToken cancellationToken = default)
        
        {
            _appCache.TryGetValue<IpRateLimitPolicies>(id, out var policy);

            return Task.FromResult(policy);
        }

        public Task RemoveAsync(string id, CancellationToken cancellationToken = default)
        {
            _appCache.Remove(id);

            return Task.CompletedTask;
        }

        public Task SeedAsync()
        {
            return Task.CompletedTask;
        }

        public Task SetAsync(string id, IpRateLimitPolicies entry, TimeSpan? expirationTime = null, CancellationToken cancellationToken = default)
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
