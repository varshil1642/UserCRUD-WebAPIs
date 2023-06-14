using Microsoft.IdentityModel.Tokens;
using Models.DataModels;
using Models.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BooksWebAPI
{
    public class JwtTokenHelper
    {
        public static string GenerateToken(JwtSettings jwtSetting, Users user)
        {
            if (jwtSetting == null)
                return string.Empty;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Role, user.UserType.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Email)
            };

            var token = new JwtSecurityToken(
                    jwtSetting.Issuer,
                    jwtSetting.Audience,
                    claims,
                    signingCredentials: credentials,
                    expires: DateTime.UtcNow.AddMinutes(50)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}