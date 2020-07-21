using System;
using System.Threading.Tasks;
using dev.codingWombat.Vombatidae.business;
using Microsoft.Extensions.Logging;

namespace dev.codingWombat.Vombatidae.core
{
    public interface IBurrowUpdater
    {
        Task<Burrow> Update(Guid guid, Burrow burrow);
    }
    
    public class BurrowUpdater : IBurrowUpdater
    {
        private readonly ILogger<BurrowUpdater> _logger;
        private readonly ICacheRepository _repository;

        public BurrowUpdater(ILogger<BurrowUpdater> logger, ICacheRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<Burrow> Update(Guid guid, Burrow burrow)
        {
            var existing = await _repository.ReadBurrowAsync(guid);

            if (existing.Modified != burrow.Modified)
            {
                throw new Exception("burrow was already modified");
            }

            burrow.Modified = DateTime.UtcNow;
            
            await _repository.WriteBurrowAsync(burrow);

            _logger.LogDebug("Burrow {} updated with new config.", guid.ToString());
            return burrow;
        }
    }
}