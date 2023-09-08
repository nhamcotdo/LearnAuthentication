using Microsoft.AspNetCore.Mvc.Filters;

namespace LearnAuthentication.Configuarations.Filters
{
    [AttributeUsage(AttributeTargets.All)]
    public class MyFilterAttribute : Attribute, IActionFilter
    {
        private readonly string _callerName;


        public MyFilterAttribute(string callerName)
        {
            _callerName = callerName;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine($"OnActionExecuting: {_callerName}");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine($"OnActionExecuted: {_callerName}");
        }
    }
}