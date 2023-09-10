using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using LearnAuthentication.LearnAuthentication.Common.Schemas;

namespace LearnAuthentication.LearnAuthentication.Configuarations.Filters
{
    public class ApiFilter : IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            try
            {
                var apiKey = context.HttpContext.Request.Headers["X-API-Key"].ToString();
                if (string.IsNullOrEmpty(apiKey))
                {
                    apiKey = context.HttpContext.Request.Query["X-API-Key"].ToString();
                }
                if (string.IsNullOrEmpty(apiKey))
                {
                    apiKey = context.HttpContext.Request.Cookies["X-API-Key"].ToString();
                }
                
                if (apiKey == Common.Common.API_KEY)
                {
                    await Task.CompletedTask;
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("API key is missing or invalid");
            context.Result = new JsonResult(new ResponseInfo() { Code = StatusCodes.Status401Unauthorized, Message = "API key is missing or invalid" });
        }
    }
}