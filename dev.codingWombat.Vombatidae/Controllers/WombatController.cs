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
        private readonly IBurrowUpdater _updater;
        private readonly IResponseReader _reader;
        private readonly IResponseHelper _helper;

        public WombatController(ILogger<WombatController> logger, IBurrowUpdater updater, IResponseReader reader, IResponseHelper helper)
        {
            _logger = logger;
            _updater = updater;
            _reader = reader;
            _helper = helper;
        }

        [HttpPut("{Guid}/config")]
        public async Task<IActionResult> PutConfig([FromRoute] Guid guid, [FromBody] Burrow burrowDto)
        {
            var burrow = await _updater.Update(guid, burrowDto);
            return Ok(burrow);
        }

        [HttpGet("{Guid}")]
        public async Task<IActionResult> Get([FromRoute] Guid guid)
        {
            return await ActionResult(guid, "GET");
        }

        [HttpPost("{Guid}")]
        public async Task<IActionResult> Post([FromRoute] Guid guid)
        {
            return await ActionResult(guid, "POST");
        }

        [HttpPut("{Guid}")]
        public async Task<IActionResult> Put([FromRoute] Guid guid)
        {
            return await ActionResult(guid, "PUT");
        }

        [HttpDelete("{Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid guid)
        {
            return await ActionResult(guid, "DELETE");
        }

        private async Task<IActionResult> ActionResult(Guid guid, string httpMethod)
        {
            var response = await _reader.ReadResponse(httpMethod, guid);
            return _helper.CreateHttpResponse(response);
        }
    }
}