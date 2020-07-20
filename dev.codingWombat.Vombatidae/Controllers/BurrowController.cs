using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace dev.codingWombat.Vombatidae.Controllers
{
    [Route("Vombatidae/burrow")]
    public class BurrowController : Controller
    {
        private readonly ILogger<BurrowController> _logger;

        public BurrowController(ILogger<BurrowController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            var guid = Guid.NewGuid();
            _logger.LogInformation("New burrow created with Guid: {}", guid.ToString());
            return Ok(guid.ToString());
        }
    }
}