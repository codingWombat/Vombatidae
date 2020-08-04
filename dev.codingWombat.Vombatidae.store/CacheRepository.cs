using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using dev.codingWombat.Vombatidae.business;
using dev.codingWombat.Vombatidae.config;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace dev.codingWombat.Vombatidae.store
{
    public interface ICacheRepository
    {
        Task<Burrow> ReadBurrowAsync(Guid guid);
        Task WriteBurrowAsync(Burrow burrow);
        Task<JsonElement> ReadResponseBodyAsync(string httpMethod, Guid guid);
        Task WriteResponseBodyAsync(string httpMethod, Guid guid, string responseBody);
        Dictionary<string, RequestResponseHistory> ReadHistory();
        Task WriteHistoryAsync(Dictionary<string, RequestResponseHistory> history);
    }

    internal class CacheRepository : ICacheRepository
    {
        private class TmpHistory
        {
            public Guid Id { get; set; }
            public Queue<RequestResponse> History { get; set; }
        }

        private readonly IDistributedCache _cache;
        private readonly ILogger<CacheRepository> _logger;
        private readonly CacheConfiguration _configuration;

        public CacheRepository(IDistributedCache cache, ILogger<CacheRepository> logger,
            IOptions<CacheConfiguration> configuration)
        {
            _cache = cache;
            _logger = logger;
            _configuration = configuration.Value;
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
                new DistributedCacheEntryOptions().SetSlidingExpiration(
                    TimeSpan.FromSeconds(_configuration.SlidingExpiration)));

            _logger.LogDebug("Finished writing burrow with id {} to cache.", burrow.Id.ToString());
        }

        public async Task<JsonElement> ReadResponseBodyAsync(string httpMethod, Guid guid)
        {
            _logger.LogDebug("Start reading response for guid {} and method {} from cache.", guid.ToString(),
                httpMethod);
            await ReadBurrowAsync(guid);

            var response = await _cache.GetStringAsync(guid + "_" + httpMethod);

            if (string.IsNullOrWhiteSpace(response))
            {
                throw new Exception("Response not existing");
            }

            _logger.LogDebug("Finished loading response for guid {} and method {} from cache.", guid.ToString(),
                httpMethod);

            using var doc = JsonDocument.Parse(response);
            return doc.RootElement.Clone();
        }

        public async Task WriteResponseBodyAsync(string httpMethod, Guid guid, string responseBody)
        {
            _logger.LogDebug("Start writing response for guid: {} and method: {} to cache.", guid.ToString(),
                httpMethod);

            await _cache.SetStringAsync(guid + "_" + httpMethod, responseBody,
                new DistributedCacheEntryOptions().SetSlidingExpiration(
                    TimeSpan.FromSeconds(_configuration.SlidingExpiration)));

            _logger.LogDebug("Finished writing response for guid: {} and method: {} to cache.", guid.ToString(),
                httpMethod);
        }

        public Dictionary<string, RequestResponseHistory> ReadHistory()
        {
            _logger.LogDebug("Start loading request history");

            var historyString = _cache.GetString("history");

            if (string.IsNullOrWhiteSpace(historyString))
            {
                _logger.LogDebug("Finished loading empty request history");
                return new Dictionary<string, RequestResponseHistory>();
            }

            var history = JsonSerializer.Deserialize<Dictionary<string, TmpHistory>>(historyString);

            _logger.LogDebug("Finished loading request history");

            return history.ToDictionary(pair => pair.Key,
                pair => new RequestResponseHistory
                    {Id = Guid.Parse(pair.Key), History = new ConcurrentQueue<RequestResponse>(pair.Value.History)});
        }

        public async Task WriteHistoryAsync(Dictionary<string, RequestResponseHistory> history)
        {
            _logger.LogDebug("Start writing history.");
            var jsonHistory = JsonSerializer.Serialize(history);
            await _cache.SetStringAsync("history", jsonHistory,
                new DistributedCacheEntryOptions().SetSlidingExpiration(
                    TimeSpan.FromSeconds(_configuration.SlidingExpiration)));

            _logger.LogDebug("Finished writing history.");
        }
    }
}