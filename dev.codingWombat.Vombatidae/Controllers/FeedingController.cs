using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace dev.codingWombat.Vombatidae.Controllers
{
    [Route("Vombatidae")]
    public class FeedingController : Controller
    {
        private readonly ILogger<FeedingController> _logger;

        public FeedingController(ILogger<FeedingController> logger)
        {
            _logger = logger;
        }

        [HttpPut("{Guid}/config")]
        public IActionResult PutConfig([FromRoute] Guid guid)
        {
            _logger.LogInformation("guid: {}",guid.ToString());
            return Ok();
        }

        [HttpGet("{Guid}")]
        public IActionResult Get([FromRoute] Guid guid)
        {
            _logger.LogInformation("guid: {}", guid.ToString());
            return Ok();
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