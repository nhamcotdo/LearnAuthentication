using System.Security.Claims;

namespace workspace.LearnAuthentication.Services
{
    public interface IIdentityService
    {
        public string[] GetRoles();

        public string GetUser();
    }

    public class IdentityService : IIdentityService
    {
        private readonly IHttpContextAccessor _httpContext;

        public IdentityService(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
        }

        public string[] GetRoles()
        {
            var user = _httpContext.HttpContext.User;
            if (user != null && user.Identity != null && user.Identity.IsAuthenticated)
            {
                var identity = user.Identity as ClaimsIdentity;
                var roles = identity?.Claims.Where(p => p.Type == ClaimTypes.Role).FirstOrDefault()?.Value;

                return roles?.Split(',') ?? Array.Empty<string>();
            }

            return Array.Empty<string>();
        }

        public string GetUser()
        {
            var user = _httpContext.HttpContext.User;
            if (user != null && user.Identity != null && user.Identity.IsAuthenticated)
            {
                var identity = user.Identity as ClaimsIdentity;
                var username = identity?.Claims.Where(p => p.Type == "username").FirstOrDefault()?.Value;

                return username ?? string.Empty;
            }

            return string.Empty;
        }
    }
}