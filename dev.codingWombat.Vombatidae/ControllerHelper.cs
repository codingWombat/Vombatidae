using System;
using System.Linq;
using dev.codingWombat.Vombatidae.business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace dev.codingWombat.Vombatidae
{
    public interface IControllerHelper
    {
        public IActionResult CreateHttpResponse(Response response);
        public string GetDynamicPartOfRoute(Guid guid, HttpRequest request, string basePath);
    }
    
    public class ControllerHelper : IControllerHelper
    {
        public string GetDynamicPartOfRoute(Guid guid, HttpRequest request, string basePath)
        {
            return request.Path.Value.Split(basePath + guid)
                .FirstOrDefault(s => !string.IsNullOrWhiteSpace(s));
        }
        
        public IActionResult CreateHttpResponse(Response response)
        {
            var result = new ObjectResult(response.ResponseMessage);
            result.StatusCode = response.StatusCode;

            return result;
        }
    }
}