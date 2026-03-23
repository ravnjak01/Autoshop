using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace RS1_2024_25.API.Services
{
    public class TokenBlacklistService
    {
        private readonly IDistributedCache _cache;

        public TokenBlacklistService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async void BlacklistToken(string token, DateTime expiry)
        {
            var timeUntilExpiry = expiry - DateTime.UtcNow;
            if (timeUntilExpiry.TotalSeconds <= 0) return;

            var options=new DistributedCacheEntryOptions
            {
                
                AbsoluteExpirationRelativeToNow= timeUntilExpiry

            };

            await _cache.SetStringAsync($"blacklist_{token}", "true", options);


        }

        public async Task<bool> IsBlacklistedAsync(string token)
        {
            var result = await _cache.GetStringAsync($"blacklist_{token}");
            return result != null;
        }
    }
}
