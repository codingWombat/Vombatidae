using System;
using System.Threading.Tasks;
using dev.codingWombat.Vombatidae.core;
using dev.codingWombat.Vombatidae.Filter;
using Microsoft.AspNetCore.Mvc;

namespace dev.codingWombat.Vombatidae.Controllers
{
    [Route("Vombatidae/history")]
    public class HistoryController : Controller
    {
        private readonly IHistoryHandler _historyHandler;

        public HistoryController(IHistoryHandler historyHandler)
        {
            _historyHandler = historyHandler;
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            await _historyHandler.SaveOptionsAsync();
            return Ok();
        }

        [HttpGet("{Guid}")]
        [RouteValidationFilter]
        public async Task<IActionResult> Get([FromRoute] Guid guid)
        {
            var requestResponseHistory = _historyHandler.Load(guid);

            return Ok(requestResponseHistory.History);
        }
    }
}