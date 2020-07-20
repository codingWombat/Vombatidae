using System;
using System.Threading.Tasks;
using dev.codingWombat.Vombatidae.business;
using dev.codingWombat.Vombatidae.core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace dev.codingWombat.Vombatidae.Controllers
{
    [Route("Vombatidae")]
    public class WombatController : Controller
    {
        private readonly ILogger<WombatController> _logger;
        private readonly IBurrowReader _reader;
        private readonly IBurrowUpdater _updater;
        private readonly ICacheRepository _cache;
        
        public WombatController(ILogger<WombatController> logger, IBurrowReader reader, IBurrowUpdater updater, ICacheRepository cache)
        {
            _logger = logger;
            _reader = reader;
            _updater = updater;
            _cache = cache;
        }

        [HttpPut("{Guid}/config")]
        public async Task<IActionResult> PutConfig([FromRoute] Guid guid, [FromBody] Burrow burrowDto)
        {
            var burrow = await _updater.Update(guid, burrowDto);
            
            _logger.LogInformation("guid: {}",burrow.Id.ToString());
            return Ok(burrow);
        }

        [HttpGet("{Guid}")]
        public async Task<IActionResult> Get([FromRoute] Guid guid)
        {
            var burrow = await _reader.Read(guid);

            var response = await _cache.ReadResponseBodyAsync("GET", guid);
            
            _logger.LogInformation("guid: {}", burrow.Id.ToString());

            return Ok(response);
        }
        
        [HttpPost("{Guid}")]
        public IActionResult Post([FromRoute] Guid guid)
        {
            _logger.LogInformation("guid: {}", guid.ToString());
            return Ok();
        }
        
        [HttpPut("{Guid}")]
        public IActionResult Put([FromRoute] Guid guid)
        {
            _logger.LogInformation("guid: {}", guid.ToString());
            return Ok();
        }
        
        [HttpDelete("{Guid}")]
        public IActionResult Delete([FromRoute] Guid guid)
        {
            _logger.LogInformation("guid: {}", guid.ToString());
            return Ok();
        }
    }
}