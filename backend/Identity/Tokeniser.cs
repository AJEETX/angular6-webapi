using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using WebApi.Helpers;
using System.Collections.Generic;

namespace WebApi.Identity
{
    public interface ITokeniser
    {
        string CreateToken(string userid,string username);
    }

    public class Tokeniser : ITokeniser
    {
        private readonly AppSettings _appSettings;

        public Tokeniser(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public string CreateToken(string userid,string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var claims=new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name,userid));
            if(username.ToLowerInvariant()=="admin")
            claims.Add(new Claim(ClaimTypes.Role,"Admin"));
            else
            claims.Add(new Claim(ClaimTypes.Role,"User"));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }
    }
}