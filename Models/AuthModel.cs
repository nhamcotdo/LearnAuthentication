using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using workspace.LearnAuthentication.Common;
using workspace.LearnAuthentication.Common.Schemas;

namespace LearnAuthentication.Models
{
    public interface IAuthModel
    {
        Task<bool> CheckUser(string username, string password);
        Task<string> GenerateToken(string username, string[] roles);
        Task<string[]> GetRoles(string username);
        Task<ResponseInfo> Login(string username, string password);
    }

    public class AuthModel : IAuthModel
    {
        public async Task<bool> CheckUser(string username, string password)
        {
            if (username == "Admin" && password == "Admin@123")
            {
                return await Task.FromResult(true);
            }
            else if (username == "Nham" && password == "Admin@123")
            {
                return await Task.FromResult(true);
            }

            return await Task.FromResult(false);
        }

        public Task<string> GenerateToken(string username, string[] roles)
        {

            var now = DateTime.Now;
            var claims = new Claim[] {
                new ("username", username),
                new (ClaimTypes.Role, string.Join(',', roles)),
                new (JwtRegisteredClaimNames.Sub, username),
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid ().ToString ()),
                new (JwtRegisteredClaimNames.Iat, now.ToUniversalTime ().ToString (), ClaimValueTypes.Integer64)
            };
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Common.C_JWT_SECRET_KEY));
            var signingCredentials = new SigningCredentials(
                signingKey,
                SecurityAlgorithms.HmacSha256Signature);
            var handler = new JwtSecurityTokenHandler();

            var jwt = new JwtSecurityToken(
                issuer: Common.C_JWT_ISSUER,
                audience: Common.C_JWT_AUDIENCE,
                claims: claims,
                notBefore: now,
                expires: now.AddMinutes(Common.API_EXPIRES_MINUTE),
                signingCredentials: signingCredentials
            );

            var encodedJwt = handler.WriteToken(jwt);

            return Task.FromResult(encodedJwt);
        }

        public Task<string[]> GetRoles(string username)
        {
            if (username == "Admin")
            {
                return Task.FromResult(new string[] { "Admin" });
            }
            else if (username == "Nham")
            {
                return Task.FromResult(new string[] { "Nham" });
            }

            return Task.FromResult(Array.Empty<string>());
        }

        public async Task<ResponseInfo> Login(string username, string password)
        {
            try
            {
                if (await CheckUser(username, password))
                {
                    var token = await GenerateToken(username, await GetRoles(username));

                    return new ResponseInfo()
                    {
                        Data = token
                    };
                }
                else
                {
                    return new ResponseInfo()
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Data =  "Invalid Username or Password"
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }
    }
}