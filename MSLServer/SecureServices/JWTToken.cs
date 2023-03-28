using Microsoft.IdentityModel.Tokens;
using MSLServer.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MSLServer.SecureServices
{

    public static class JWTToken
    {

        //public static string CreateToken(User user)
        //{
        //    List<Claim> lista = new List<Claim>();
        //    lista.Add(new Claim("_id", user.Id.ToString()));
        //    lista.Add(new Claim("_email", user.Email));
        //    lista.Add(new Claim("_password", user.Password));
        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Resource.Criptkey));
        //    ;
        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        //    var token = new JwtSecurityToken(
        //        claims: lista,
        //        expires: DateTime.UtcNow.AddDays(30),
        //        signingCredentials: creds);
        //    var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        //    return jwt;
        //}
        //public static JwtSecurityToken DecodeToken(string stream)
        //{
        //    var handler = new JwtSecurityTokenHandler();
        //    return handler.ReadJwtToken(stream);

        //}
        ////Már nem jó mert át lettek nevezve a jwt token adatok
        //public static string GetDataFromToken(HttpContext context, string type)
        //{
        //    ClaimsIdentity identity = context.User.Identity as ClaimsIdentity;
        //    IEnumerable<Claim> claim = identity.Claims;
        //    var data = claim.Where(x => x.Type == type).FirstOrDefault().ToString().Split(':')[1].Trim();
        //    return data;

        //}
        public static string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
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
