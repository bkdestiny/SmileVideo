using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Common.JWT
{
    public class JWTHelper
    {
        public static string BuildToken(IEnumerable<Claim> claims, JWTOptions options)
        {
            TimeSpan ExpiryDuration = TimeSpan.FromSeconds(options.ExpireSeconds);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(options.Issuer, options.Audience, claims,
                expires: DateTime.Now.Add(ExpiryDuration), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
        public static string BuildToken(JWTModel model,JWTOptions options)
        {
            List<Claim> claims = new List<Claim>();
            DateTime now = DateTime.Now;
            claims.Add(new Claim("IpAddress", model.ipAddress));
            claims.Add(new Claim("Name", model.name));
            claims.Add(new Claim("Role", model.rule));
            claims.Add(new Claim("Avatar", model.avatar.ToString()));
            claims.Add(new Claim("AuthorizationTime", now.Ticks.ToString()));
            long expirationTime = now.AddSeconds(options.ExpireSeconds).Ticks;
            claims.Add(new Claim("ExpirationTime", expirationTime.ToString()));
            return BuildToken(claims, options);
        }
    }
}
