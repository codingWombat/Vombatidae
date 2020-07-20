using System;
using System.Threading.Tasks;
using dev.codingWombat.Vombatidae.business;
using Microsoft.Extensions.Logging;

namespace dev.codingWombat.Vombatidae.core
{
    public interface IBurrowCreator
    {
        public Task<Burrow> Create();
    }

    public class BurrowCreator : IBurrowCreator
    {
        private readonly ICacheRepository _cache;
        private readonly ILogger<BurrowCreator> _logger;

        public BurrowCreator(ICacheRepository cache, ILogger<BurrowCreator> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public async Task<Burrow> Create()
        {
            var now = DateTime.UtcNow;
            var burrow = new Burrow {Id = Guid.NewGuid(), Create = now, Modified = now};
            await _cache.WriteBurrowAsync(burrow);
            return burrow;
        }
    }
}