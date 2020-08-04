using System;
using System.Threading.Tasks;
using dev.codingWombat.Vombatidae.core;
using dev.codingWombat.Vombatidae.Filter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace dev.codingWombat.Vombatidae.Controllers
{
    [Route("Vombatidae/Feed")]
    public class FeedController : Controller
    {
        private readonly ILogger<FeedController> _logger;
        private readonly IResponseUpserter _upserter;
        private readonly IResponseReader _reader;
        private readonly IControllerHelper _helper;

        private const string basePath = "/Vombatidae/Feed/";
        
        public FeedController(ILogger<FeedController> logger, IResponseUpserter upserter, IResponseReader reader, IControllerHelper helper)
        {
            _logger = logger;
            _upserter = upserter;
            _reader = reader;
            _helper = helper;
        }

        [HttpPut("{Guid}/{**Wildcard}/")]
        [RouteValidationFilter]
        public async Task<IActionResult> Put([FromRoute] Guid guid)
        {
            var method = HttpContext.Request.Query["method"];
            var dynamicRoute = _helper.GetDynamicPartOfRoute(guid, Request, basePath);
            await _upserter.UpsertResponse(guid, dynamicRoute+"_"+method[0].ToUpper(), HttpContext.Request.Body);
            return Ok();
        }
        
        [HttpGet("{Guid}/{**Wildcard}/")]
        [RouteValidationFilter]
        public async Task<IActionResult> Get([FromRoute] Guid guid)
        {
            var method = HttpContext.Request.Query["method"];
            var dynamicRoute = _helper.GetDynamicPartOfRoute(guid, Request, basePath);
            var response = await _reader.ReadResponse(dynamicRoute+"_"+method[0].ToUpper(), guid);
            return Ok(response);
        }
    }
}