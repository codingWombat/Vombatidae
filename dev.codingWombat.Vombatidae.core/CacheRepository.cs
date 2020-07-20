using System;
using System.Text.Json;
using System.Threading.Tasks;
using dev.codingWombat.Vombatidae.business;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace dev.codingWombat.Vombatidae.core
{
    public interface ICacheRepository
    {
        Task<Burrow> ReadBurrowAsync(Guid guid);
        Task WriteBurrowAsync(Burrow burrow);
        Task WriteResponseBodyAsync(string httpMethod, Guid guid, string responseBody);
        Task<JsonElement> ReadResponseBodyAsync(string httpMethod, Guid guid);
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

        public async Task<Burrow> ReadBurrowAsync(Guid guid)
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

        public async Task WriteBurrowAsync(Burrow burrow)
        {
            _logger.LogDebug("Start writing burrow: {} to cache.", burrow.ToString());

            var jsonBurrow = JsonSerializer.Serialize(burrow);
            await _cache.SetStringAsync(burrow.Id.ToString(), jsonBurrow,
                new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(1)));

            _logger.LogDebug("Finished writing burrow with id {} to cache.", burrow.Id.ToString());
        }

        public async Task WriteResponseBodyAsync(string httpMethod, Guid guid, string responseBody)
        {
            _logger.LogDebug("Start writing response for guid: {} and method: {} to cache.", guid.ToString(),
                httpMethod);

            await _cache.SetStringAsync(guid + "_" + httpMethod, responseBody,
                new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(1)));

            _logger.LogDebug("Finished writing response for guid: {} and method: {} to cache.", guid.ToString(),
                httpMethod);
        }

        public async Task<JsonElement> ReadResponseBodyAsync(string httpMethod, Guid guid)
        {
            _logger.LogDebug("Start reading response for guid {} and method {} from cache.", guid.ToString(),
                httpMethod);

            var response = await _cache.GetStringAsync(guid + "_" + httpMethod);

            if (string.IsNullOrWhiteSpace(response))
            {
                throw new Exception("Respone not existing");
            }

            _logger.LogDebug("Finished loading response for guid {} and method {} from cache.", guid.ToString(),
                httpMethod);

            using var doc = JsonDocument.Parse(response);
            return doc.RootElement.Clone();
        }
    }
}