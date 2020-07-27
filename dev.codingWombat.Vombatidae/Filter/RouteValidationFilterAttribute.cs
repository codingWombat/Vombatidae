using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace dev.codingWombat.Vombatidae.Filter
{
    public class RouteValidationFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.RouteData.Values.ContainsKey("Guid"))
            {
                return;
            }

            var guid = Guid.Empty;

            if (Guid.TryParse(context.RouteData.Values["Guid"] as string, out guid))
            {
                if (guid != Guid.Empty)
                {
                    return;
                }
            }

            context.Result = new BadRequestObjectResult("No valid burrow id in url");
        }
    }
}