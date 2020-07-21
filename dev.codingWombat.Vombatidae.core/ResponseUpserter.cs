using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace dev.codingWombat.Vombatidae.core
{
    public interface IResponseUpserter
    {
        Task UpsertResponse(Guid guid, string method, Stream response);
    }

    public class ResponseUpserter : IResponseUpserter
    {
        private readonly ICacheRepository _cache;
        private readonly ILogger<ResponseUpserter> _logger;

        public ResponseUpserter(ICacheRepository cache, ILogger<ResponseUpserter> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public async Task UpsertResponse(Guid guid, string method, Stream response)
        {
            using (var streamReader = new StreamReader(response))
            {
                var preparedResponse = await streamReader.ReadToEndAsync();
                await _cache.WriteResponseBodyAsync(method.ToUpper(), guid, preparedResponse);
                _logger.LogDebug("Saved new repsonse for burrow {} and method {}", guid.ToString(), method);
            }
        }
    }
}