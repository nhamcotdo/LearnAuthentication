using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace LearnAuthentication.Services
{
    public interface IAuthService
    {
        Task<bool> CheckUser(string username, string password);
        Task<string> GenerateToken(string username, string[] roles);
        Task<string[]> GetRoles(string username);
    }

    public class AuthService : IAuthService
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
            var claims = new Claim[] {
                new ("username", username),
                new ("roles", string.Join(',', roles)),
            };
            var now = DateTime.Now;
            var jwt = new JwtSecurityToken(
                issuer: "nhamcotdo",
                audience: "aud",
                claims: claims,
                notBefore: now,
                expires: now.AddMinutes(5),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes("secretKey")), SecurityAlgorithms.Aes128CbcHmacSha256)
            );

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

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
    }
}