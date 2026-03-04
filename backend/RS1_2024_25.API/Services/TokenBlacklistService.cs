using Microsoft.Extensions.Caching.Memory;

namespace RS1_2024_25.API.Services
{
    public class TokenBlacklistService
    {
        private readonly IMemoryCache _cache;

        public TokenBlacklistService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void BlacklistToken(string token, DateTime expiry)
        {
            var timeUntilExpiry = expiry - DateTime.UtcNow;
            _cache.Set($"blacklist_{token}", true, timeUntilExpiry);
        }

        public bool IsBlacklisted(string token)
        {
            return _cache.TryGetValue($"blacklist_{token}", out _);
        }
    }
}
