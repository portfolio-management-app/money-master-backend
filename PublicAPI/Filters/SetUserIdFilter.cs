using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PublicAPI.Filters
{
    public class SetUserIdFilter: IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                Console.WriteLine("Inside action filter");
                var userId = int
                    .Parse(context.HttpContext.User.Claims.First(c => c.Type == "ID").Value);
                context.HttpContext.Items.Add("userId", userId);
            }
            catch (InvalidOperationException ex)
            {
                context.HttpContext.Items.Add("userId", null);
            }

        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // do nothing
        }
    }
}