using Microsoft.AspNetCore.Mvc.Filters;

namespace LearnAuthentication.Configuarations.Filters
{
    public class MyFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine("This filter executed on: OnActionExecuting");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine("This filter executed on: OnActionExecuted");
        }
    }
}