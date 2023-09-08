using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LearnAuthentication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using workspace.LearnAuthentication.Common;
using workspace.LearnAuthentication.Common.Schemas;

namespace LearnAuthentication.Configuarations.Filters
{
    [AttributeUsage(AttributeTargets.All)]
    public class BasicAuthFilterAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly string _role;
        public BasicAuthFilterAttribute(string role = null)
        {
            _role = role;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            try
            {
                IAuthService? authService = context.HttpContext.RequestServices.GetService<IAuthService>();
                var accessToken = context.HttpContext.Request.Headers["Authorization"].ToString().Split(' ');
                var schemas = accessToken[0];

                if (schemas == "Basic")
                {
                    var token = accessToken[1];
                    var credentials = Helpers.Base64Decode(token).Split(':');
                    string username = credentials[0];
                    var password = credentials[1];
                    if (await authService?.CheckUser(username, password))
                    {
                        return;
                    }
                }
                else if (schemas == "Bearer")
                {
                    var token = accessToken[1];
                    var jwt = new JwtSecurityToken(token);
                    var tokenData = new JwtSecurityTokenHandler().ReadJwtToken(token);
                    var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Common.C_JWT_SECRET_KEY));
                    var tokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = signingKey,
                        ValidateAudience = true,
                        ValidAudiences = new string[]
                        {
                            Common.C_JWT_AUDIENCE
                        },
                        ValidateIssuer = true,
                        ValidIssuers = new string[]
                        {
                            Common.C_JWT_ISSUER
                        },
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                    };


                    SecurityToken validatedToken;
                    var handler = new JwtSecurityTokenHandler();

                    var claimsPrincipal = handler.ValidateToken(token, tokenValidationParameters, out validatedToken);
                    var roles = claimsPrincipal.Claims.Where(p => p.Type == ClaimTypes.Role).FirstOrDefault()?.Value.Split(',');
                    context.HttpContext.User = new System.Security.Principal.GenericPrincipal(claimsPrincipal.Identity, roles);

                    if (!string.IsNullOrEmpty(_role) && !roles.Contains(_role))
                    {
                        context.Result = new JsonResult(new ResponseInfo() { Code = StatusCodes.Status403Forbidden, Message = "You are not allowed access this page" });
                    }

                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            context.Result = new JsonResult(new ResponseInfo() { Code = StatusCodes.Status401Unauthorized, Message = "You must sign in to access this page" });
        }
    }
}