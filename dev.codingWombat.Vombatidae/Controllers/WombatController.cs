using System;
using System.IO;
using System.Threading.Tasks;
using dev.codingWombat.Vombatidae.business;
using dev.codingWombat.Vombatidae.core;
using dev.codingWombat.Vombatidae.Filter;
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
        private readonly IHistoryHandler _historyHandler;

        public WombatController(ILogger<WombatController> logger, IBurrowUpdater updater, IResponseReader reader,
            IResponseHelper helper, IHistoryHandler historyHandler)
        {
            _logger = logger;
            _updater = updater;
            _reader = reader;
            _helper = helper;
            _historyHandler = historyHandler;
        }

        [HttpPut("{Guid}/config")]
        public async Task<IActionResult> PutConfig([FromRoute] Guid guid, [FromBody] Burrow burrowDto)
        {
            var burrow = await _updater.Update(guid, burrowDto);
            return Ok(burrow);
        }

        [HttpGet("{Guid}")]
        [RouteValidationFilter]
        public async Task<IActionResult> Get([FromRoute] Guid guid)
        {
            return await ActionResult(guid, "GET", HttpContext.Request.Body);
        }

        [HttpPost("{Guid}")]
        [RouteValidationFilter]
        public async Task<IActionResult> Post([FromRoute] Guid guid)
        {
            return await ActionResult(guid, "POST", HttpContext.Request.Body);
        }

        [HttpPut("{Guid}")]
        [RouteValidationFilter]
        public async Task<IActionResult> Put([FromRoute] Guid guid)
        {
            return await ActionResult(guid, "PUT", HttpContext.Request.Body);
        }

        [HttpDelete("{Guid}")]
        [RouteValidationFilter]
        public async Task<IActionResult> Delete([FromRoute] Guid guid)
        {
            return await ActionResult(guid, "DELETE", HttpContext.Request.Body);
        }

        private async Task<IActionResult> ActionResult(Guid id, string httpMethod, Stream requestBody)
        {
            var response = await _reader.ReadResponse(httpMethod, id);
            using (var streamReader = new StreamReader(requestBody))
            {
                var body = await streamReader.ReadToEndAsync();
                _historyHandler.AppendRequest(id,
                    new RequestResponse
                    {
                        Timestamp = DateTime.UtcNow, HttpMethod = httpMethod, ResponseBody = response,
                        RequestBody = body
                    }
                );
            }

            return _helper.CreateHttpResponse(response);
        }
    }
}