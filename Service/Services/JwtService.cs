﻿using Common;
using Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class JwtService : IJwtService
    {
        private readonly SiteSettings _siteSetting;
        private readonly SignInManager<User> _signInManager;

        public JwtService(IOptionsSnapshot<SiteSettings> settings, SignInManager<User> signInManager)
        {
            _siteSetting = settings.Value;
            this._signInManager = signInManager;
        }

        public async Task<string> GenerateAsync(User user)
        {
            var secretKey = Encoding.UTF8.GetBytes(_siteSetting.JwtSettings.SecretKey); // longer that 16 character
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature);

            var encryptionkey = Encoding.UTF8.GetBytes(_siteSetting.JwtSettings.EncryptKey); //must be 16 character
            var encryptingCredentials = new EncryptingCredentials(new SymmetricSecurityKey(encryptionkey), SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);

            var claims = await _getClaimsAsync(user);

            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = _siteSetting.JwtSettings.Issuer,
                Audience = _siteSetting.JwtSettings.Audience,
                IssuedAt = DateTime.Now,
                NotBefore = DateTime.Now.AddMinutes(_siteSetting.JwtSettings.NotBeforeMinutes),
                Expires = DateTime.Now.AddMinutes(_siteSetting.JwtSettings.ExpirationMinutes),
                SigningCredentials = signingCredentials,
                EncryptingCredentials = encryptingCredentials,
                Subject = new ClaimsIdentity(claims)
            };

            //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            //JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
            //JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

            var tokenHandler = new JwtSecurityTokenHandler();

            var securityToken = tokenHandler.CreateToken(descriptor);

            var jwt = tokenHandler.WriteToken(securityToken);

            return jwt;
        }

        private async Task<IEnumerable<Claim>> _getClaimsAsync(User user)
        {
            var result = await _signInManager.ClaimsFactory.CreateAsync(user);
            //add custom claims
            var list = new List<Claim>(result.Claims);
            list.Add(new Claim(ClaimTypes.MobilePhone, "09123456987"));
            return list;

            //JwtRegisteredClaimNames.Sub
            //var securityStampClaimType = new ClaimsIdentityOptions().SecurityStampClaimType;

            //var list = new List<Claim>
            //{
            //    new Claim(ClaimTypes.Name, user.UserName),
            //    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            //    new Claim(ClaimTypes.MobilePhone, "09123456987"),
            //    new Claim(securityStampClaimType, user.SecurityStamp.ToString())
            //};

            //var roles = new Role[] { new Role { Name = "Admin" } };
            //foreach (var role in roles)
            //    list.Add(new Claim(ClaimTypes.Role, role.Name));

            //return list;
        }
    }
}
