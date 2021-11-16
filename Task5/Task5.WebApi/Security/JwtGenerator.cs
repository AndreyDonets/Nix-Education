﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Task5.DAL.Entities;
using Task5.WebApi.Interfaces;

namespace Task5.WebApi.Security
{
    public class JwtGenerator : IJwtGenerator
    {
        private readonly UserManager<User> userManager;
        public JwtGenerator(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<string> CreateToken(User user)
        {
            if (user != null)
            {
                var claims = await GetIdentity(user);

                var now = DateTime.UtcNow;

                var jwt = new JwtSecurityToken(
                        issuer: AuthOptions.ISSUER,
                        audience: AuthOptions.AUDIENCE,
                        notBefore: now,
                        claims: claims,
                        expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                        signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

                return encodedJwt;
            }
            return "";
        }

        private async Task<List<Claim>> GetIdentity(User user)
        {
            var claims = new List<Claim>
            {
                    new Claim("username", user.Email),
                    new Claim("email", user.Email)
            };

            var roles = await userManager.GetRolesAsync(user);

            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            return claims;
        }
    }
}