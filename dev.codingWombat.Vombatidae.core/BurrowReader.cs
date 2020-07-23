using System;
using System.Threading.Tasks;
using dev.codingWombat.Vombatidae.business;
using dev.codingWombat.Vombatidae.store;
using Microsoft.Extensions.Logging;

namespace dev.codingWombat.Vombatidae.core
{
    public interface IBurrowReader
    {
        Task<Burrow> Read(Guid guid);
    }
    
    public class BurrowReader : IBurrowReader
    {
        private readonly ICacheRepository _repository;
        private readonly ILogger<BurrowCreator> _logger;

        public BurrowReader(ICacheRepository repository, ILogger<BurrowCreator> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Burrow> Read(Guid guid)
        {
            return  await _repository.ReadBurrowAsync(guid);
        }
    }
}