using Microsoft.IdentityModel.Tokens;
using MSLServer.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MSLServer.SecureServices
{

    public static class JWTToken
    {
        public static string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Resource.Criptkey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
