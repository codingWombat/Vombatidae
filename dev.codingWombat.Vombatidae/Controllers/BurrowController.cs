using System.Threading.Tasks;
using dev.codingWombat.Vombatidae.business;
using dev.codingWombat.Vombatidae.core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace dev.codingWombat.Vombatidae.Controllers
{
    [Route("Vombatidae/Burrow")]
    public class BurrowController : Controller
    {
        private readonly ILogger<BurrowController> _logger;
        private readonly IBurrowCreator _creator;

        public BurrowController(ILogger<BurrowController> logger, IBurrowCreator creator)
        {
            _logger = logger;
            _creator = creator;
        }

        [HttpGet]
        public async Task<ActionResult<Burrow>> Get()
        {
            var burrow = await _creator.Create();
            _logger.LogInformation("New burrow created with Guid: {}", burrow.Id.ToString());
            return Ok(burrow);
        }
    }
}