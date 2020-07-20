using System;
using System.Text.Json;
using System.Threading.Tasks;
using dev.codingWombat.Vombatidae.config;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace dev.codingWombat.Vombatidae.core
{
    public interface ICacheRepository
    {
        Task<Burrow> ReadAsync(Guid guid);
        Task WriteAsync(Burrow burrow);
    } 
    
    internal class CacheRepository : ICacheRepository
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger<CacheRepository> _logger;

        public CacheRepository(IDistributedCache cache, ILogger<CacheRepository> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public async Task<Burrow> ReadAsync(Guid guid)
        {
            _logger.LogDebug("Start reading burrow {} from cache.", guid.ToString());

            var burrowString = await _cache.GetStringAsync(guid.ToString());
            
            if (string.IsNullOrWhiteSpace(burrowString))
            {
                throw new Exception("Burrow not existing");
            }
            
            var burrow = JsonSerializer.Deserialize<Burrow>(burrowString);
            
            _logger.LogDebug("Finished loading burro: {}", burrow.ToString());

            return burrow;
        }

        public async Task WriteAsync(Burrow burrow)
        {
            _logger.LogDebug("Start writing burrow: {} to cache.", burrow.ToString());

            var jsonBurrow = JsonSerializer.Serialize(burrow);
            await _cache.SetStringAsync(burrow.Id.ToString(), jsonBurrow,
                new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(1)));
            
            _logger.LogDebug("Finished writing burrow with id {} to cache.", burrow.Id.ToString());
        }
    }
}