using System;
using System.Threading.Tasks;
using dev.codingWombat.Vombatidae.config;
using dev.codingWombat.Vombatidae.core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace dev.codingWombat.Vombatidae.Controllers
{
    [Route("Vombatidae")]
    public class FeedingController : Controller
    {
        private readonly ILogger<FeedingController> _logger;
        private readonly IBurrowReader _reader;
        private readonly IBurrowUpdater _updater;
        
        public FeedingController(ILogger<FeedingController> logger, IBurrowReader reader, IBurrowUpdater updater)
        {
            _logger = logger;
            _reader = reader;
            _updater = updater;
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
            
            _logger.LogInformation("guid: {}", burrow.Id.ToString());
            return Ok(burrow);
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