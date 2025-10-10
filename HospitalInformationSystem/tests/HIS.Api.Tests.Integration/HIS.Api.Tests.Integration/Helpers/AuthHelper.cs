using HIS.Application.Models;
using HIS.Application.Repositories.Auth;
using HIS.Contracts.Requests.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HIS.Api.Tests.Integration.Helpers
{
    public static class AuthHelper
    {
       
        public static string GenerateToken()
        {
            var user = new User
            { 
                Id = Guid.NewGuid(),
                UserName = "Test",
                Email = "test@mail.by",
                Password = "testpass"
            };

            var userClaims = new Dictionary<string, object>()
            {
                //{ "admin", true },
                //{ "trusted_member", true },
            };
            
            var tokenSecret = "ForTheLoveOfGodStoreAndLoadThisSecurely";

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(tokenSecret);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new("UserId", user.Id.ToString()),
                new("UserName", user.UserName),
                new("Email", user.Email)
            };

            foreach (var userClaim in userClaims)
            {
                var claim = new Claim(userClaim.Key, userClaim.Value.ToString()!);
                claims.Add(claim);
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(TimeSpan.FromHours(8)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var jwt = tokenHandler.CreateToken(tokenDescriptor);

            var jwtString = tokenHandler.WriteToken(jwt);

            return jwtString;
        }
    }
}
