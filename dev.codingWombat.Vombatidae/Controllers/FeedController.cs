using System;
using System.Threading.Tasks;
using dev.codingWombat.Vombatidae.core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace dev.codingWombat.Vombatidae.Controllers
{
    [Route("Vombatidae/Feed")]
    public class FeedController : Controller
    {
        private readonly ILogger<FeedController> _logger;
        private readonly IResponseUpserter _upserter;

        public FeedController(ILogger<FeedController> logger, IResponseUpserter upserter)
        {
            _logger = logger;
            _upserter = upserter;
        }

        [HttpPut("{Guid}/{Method}")]
        public async Task<IActionResult> Put([FromRoute] Guid guid, [FromRoute] string method)
        {
            await _upserter.UpsertResponse(guid, method, HttpContext.Request.Body);
            return Ok();
        }
    }
}