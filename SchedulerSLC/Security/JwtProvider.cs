using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using StudentSLC.Models;

namespace StudentSLC.Security
{
    
    public class JwtProvider(IOptions<JwtOptions> options)
    {
        private readonly JwtOptions _options = options.Value;

        public string GenerateToken(User user)
        {
            Console.WriteLine(_options.ExpiryHours);
            var claims = new List<Claim>
            {
                new ("UserCode", user.UserCode.ToString()),
                new (ClaimTypes.Role, user.Role)
            };
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                signingCredentials: signingCredentials,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(_options.ExpiryHours)
            );

            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenValue;
        }
    }
}