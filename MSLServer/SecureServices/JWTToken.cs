using Microsoft.IdentityModel.Tokens;
using MSLServer.Models;
using MSLServer.Models.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MSLServer.SecureServices
{

    public static class JWTToken
    {
        public static string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim("email", user.Email),
                new Claim("role", user.Role.ToString()),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Resource.Criptkey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static string GetData(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var validations = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Resource.Criptkey)),
                ValidateIssuer = false,
                ValidateAudience = false
            };
            var claims = handler.ValidateToken(token, validations, out var tokenSecure);
            return claims.Identity.Name;
        }

        public static string GetIdTokenExpiry(string idtoken, string claimname)
        {
            var token = new JwtSecurityToken(jwtEncodedString: idtoken);
            var asd = token.Claims;
            string expiry = token.Claims.First(c => c.Type == claimname).Value;
            return expiry;
        }
    }
}
