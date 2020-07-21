using dev.codingWombat.Vombatidae.business;
using Microsoft.AspNetCore.Mvc;

namespace dev.codingWombat.Vombatidae
{
    public interface IResponseHelper
    {
        public IActionResult CreateHttpResponse(Response response);
    }
    
    public class ResponseHelper : IResponseHelper
    {
        public IActionResult CreateHttpResponse(Response response)
        {
            var result = new ObjectResult(response.ResponseMessage);
            result.StatusCode = response.StatusCode;

            return result;
        }
    }
}