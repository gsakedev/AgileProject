using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OrderManager.Domain.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OrderManager.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly string _jwtSecret;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly string _expiration;

        public TokenService(IConfiguration configuration)
        {
            _jwtSecret = configuration["Jwt:Secret"] ?? throw new ArgumentNullException("Jwt:Secret");
            _issuer = configuration["Jwt:Issuer"] ?? "https://localhost";
            _audience = configuration["Jwt:Audience"] ?? "api";
            _expiration = configuration["Jwt:Expiration"];
        }

        public string GenerateToken(string userId, IEnumerable<string> roles)
        {
            var tokenExpiration = DateTime.UtcNow.AddHours(int.Parse(_expiration));
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: tokenExpiration,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
