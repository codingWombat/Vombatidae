using System;
using System.IO;
using System.Text;
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
        private readonly ICacheRepository _repository;

        public FeedController(ILogger<FeedController> logger, ICacheRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpPut("{Guid}/{Method}")]
        public async Task<IActionResult> Put([FromRoute] Guid guid, [FromRoute] string method)
        {
            using (var stream = new StreamReader(HttpContext.Request.Body))
            {
                var preparedResponse = await stream.ReadToEndAsync();
                await _repository.WriteResponseBodyAsync(method.ToUpper(), guid, preparedResponse);
            }
            
            return Ok();
        }
    }
}