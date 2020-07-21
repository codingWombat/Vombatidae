using System;
using System.Threading.Tasks;
using dev.codingWombat.Vombatidae.business;
using Microsoft.Extensions.Logging;

namespace dev.codingWombat.Vombatidae.core
{
    public interface IResponseReader
    {
        public Task<Response> ReadResponse(string httpMethod, Guid guid);
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

        public async Task<Response> ReadResponse(string httpMethod, Guid guid)
        {
            var burrow = await _reader.Read(guid);
            var response = await _cache.ReadResponseBodyAsync(httpMethod, guid);

            var statusCodeElement = response.GetProperty("StatusCode");
            var responseMessageElement = response.GetProperty("ResponseMessage");

            _logger.LogDebug("read response for guid: {}, httpMethod {}", burrow.Id.ToString(), httpMethod);

            return new Response(){ResponseMessage = responseMessageElement, StatusCode = statusCodeElement.GetInt32()};
        }
    }
}