using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Entities.User;
using Microsoft.IdentityModel.Tokens;

namespace Services.Services
{
    public class JwtService:IJwtService 
    {
        public string Generate(User user)
        {
            var secretKey = Encoding.UTF8.GetBytes("Mohammad Hossein Nejadhendi"); // must be 16 chars or longer
            var signingCredentials =
                new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature);

            var claims = this.GetClaims(user);
            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = "shg1998WebSite",
                Audience = "shg1998WebSite",
                IssuedAt = DateTime.Now,
                NotBefore = DateTime.Now.AddSeconds(15), // for example:)
                Expires = DateTime.Now.AddHours(4),
                SigningCredentials = signingCredentials,
                Subject = new ClaimsIdentity(claims)
            };

            //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            //JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();
            //JwtSecurityTokenHandler.DefaultMapInboundClaims = false;


            var token = new JwtSecurityTokenHandler();
            var secToken = token.CreateToken(descriptor);
            var finalSecToken = token.WriteToken(secToken);
            return finalSecToken;
        }

        private IEnumerable<Claim> GetClaims(User user)
        {
            var list = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString())
            };
            var roles = new[] { new Role() { Name = "Admin" } };

            list.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role.Name)));
            return null;
        }
    }
}
