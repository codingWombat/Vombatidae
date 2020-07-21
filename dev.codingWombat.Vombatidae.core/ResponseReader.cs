using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace dev.codingWombat.Vombatidae.core
{
    public interface IResponseReader
    {
        public Task<JsonElement> ReadResponse(string httpMethod, Guid guid);
    }
    
    public class ResponseReader : IResponseReader
    {
        private readonly ICacheRepository _cache;
        private readonly IBurrowReader _reader;
        private readonly ILogger<ResponseReader> _logger;

        public ResponseReader(ICacheRepository cache, IBurrowReader reader, ILogger<ResponseReader> logger)
        {
            _cache = cache;
            _reader = reader;
            _logger = logger;
        }

        public async Task<JsonElement> ReadResponse(string httpMethod, Guid guid)
        {
            var burrow = await _reader.Read(guid);

            var response = await _cache.ReadResponseBodyAsync(httpMethod, guid);

            _logger.LogDebug("read response for guid: {}, httpMethod {}", burrow.Id.ToString(), httpMethod);

            return response;
        }
    }
}