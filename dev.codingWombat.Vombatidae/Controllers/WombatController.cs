using System;
using System.Text.Json;
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

        public WombatController(ILogger<WombatController> logger, IBurrowUpdater updater, IResponseReader reader)
        {
            _logger = logger;
            _updater = updater;
            _reader = reader;
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
            var response = await _reader.ReadResponse("GET", guid);

            return Ok(response);
        }

        [HttpPost("{Guid}")]
        public async Task<IActionResult> Post([FromRoute] Guid guid)
        {
            var response = await _reader.ReadResponse("POST", guid);
            return Ok(response);
        }

        [HttpPut("{Guid}")]
        public async Task<IActionResult> Put([FromRoute] Guid guid)
        {
            var response = await _reader.ReadResponse("PUT", guid);
            return Ok(response);
        }

        [HttpDelete("{Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid guid)
        {
            var response = await _reader.ReadResponse("DELETE", guid);
            return Ok(response);
        }
    }
}