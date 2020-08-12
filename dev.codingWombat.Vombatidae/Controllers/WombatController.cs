using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using dev.codingWombat.Vombatidae.business;
using dev.codingWombat.Vombatidae.core;
using dev.codingWombat.Vombatidae.Filter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace dev.codingWombat.Vombatidae.Controllers
{
    [Route("Vombatidae")]
    public class WombatController : Controller
    {
        private readonly ILogger<WombatController> _logger;
        private readonly IResponseReader _reader;
        private readonly IControllerHelper _helper;
        private readonly IHistoryHandler _historyHandler;

        private const string basePath = "/Vombatidae/";

        public WombatController(ILogger<WombatController> logger, IResponseReader reader,
            IControllerHelper helper, IHistoryHandler historyHandler)
        {
            _logger = logger;
            _reader = reader;
            _helper = helper;
            _historyHandler = historyHandler;
        }

        [HttpGet("{Guid}/{**Wildcard}")]
        [RouteValidationFilter]
        public async Task<IActionResult> Get([FromRoute] Guid guid)
        {
            var dynamicRoute = _helper.GetDynamicPartOfRoute(guid, HttpContext.Request, basePath) ?? "";
            return await ActionResult(guid, "GET", HttpContext.Request.Body, HttpContext.Request.Query.ToList(),
                dynamicRoute);
        }

        [HttpPost("{Guid}/{**Wildcard}")]
        [RouteValidationFilter]
        public async Task<IActionResult> Post([FromRoute] Guid guid)
        {
            var dynamicRoute = _helper.GetDynamicPartOfRoute(guid, HttpContext.Request, basePath) ?? "";
            return await ActionResult(guid, "POST", HttpContext.Request.Body, HttpContext.Request.Query.ToList(),
                dynamicRoute);
        }

        [HttpPut("{Guid}/{**Wildcard}")]
        [RouteValidationFilter]
        public async Task<IActionResult> Put([FromRoute] Guid guid)
        {
            var dynamicRoute = _helper.GetDynamicPartOfRoute(guid, HttpContext.Request, basePath) ?? "";
            return await ActionResult(guid, "PUT", HttpContext.Request.Body, HttpContext.Request.Query.ToList(),
                dynamicRoute);
        }

        [HttpDelete("{Guid}/{**Wildcard}")]
        [RouteValidationFilter]
        public async Task<IActionResult> Delete([FromRoute] Guid guid)
        {
            var dynamicRoute = _helper.GetDynamicPartOfRoute(guid, HttpContext.Request, basePath) ?? "";
            return await ActionResult(guid, "DELETE", HttpContext.Request.Body, HttpContext.Request.Query.ToList(),
                dynamicRoute);
        }

        private async Task<IActionResult> ActionResult(Guid id, string httpMethod, Stream requestBody,
            List<KeyValuePair<string, StringValues>> queryParams, string dynamicRoute)
        {
            var response = await _reader.ReadResponse(dynamicRoute + "_" + httpMethod.ToUpper(), id);
            using (var streamReader = new StreamReader(requestBody))
            {
                var body = await streamReader.ReadToEndAsync();
                _historyHandler.AppendRequest(id,
                    new RequestResponse
                    {
                        Id = Guid.NewGuid(),
                        Timestamp = DateTime.UtcNow, HttpMethod = httpMethod, ResponseBody = response,
                        RequestBody = string.IsNullOrWhiteSpace(body)
                            ? JsonDocument.Parse("{}").RootElement
                            : JsonDocument.Parse(body).RootElement,
                        QueryParams = queryParams,
                        Route = dynamicRoute
                    }
                );
            }

            _logger.LogDebug("Endpoint with Guid {} and dynamic route {} and method {} called.", id.ToString(),
                dynamicRoute, httpMethod);
            return _helper.CreateHttpResponse(response);
        }
    }
}