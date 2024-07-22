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
        public static string BuildToken(JWTModel model, JWTOptions options)
        {
            List<Claim> claims = new List<Claim>();
            DateTime now = DateTime.Now;
            claims.Add(new Claim("Id", model.Id.ToString()));
            claims.Add(new Claim("IpAddress", model.IpAddress));
            claims.Add(new Claim("Name", model.Name));
            claims.Add(new Claim("Role", model.Role));
            claims.Add(new Claim("Avatar", model.Avatar.ToString()));
            claims.Add(new Claim("AuthorizationTime", now.Ticks.ToString()));
            long expirationTime = now.AddSeconds(options.ExpireSeconds).Ticks;
            claims.Add(new Claim("ExpirationTime", expirationTime.ToString()));
            return BuildToken(claims, options);
        }
        public static JWTModel BuildJWTModel(IEnumerable<Claim> claims)
        {
            Guid id = new Guid(claims.Single(c => c.Type.Equals("Id")).Value);
            string ipAddress = claims.Single(c => c.Type.Equals("IpAddress")).Value;
            string name = claims.Single(c => c.Type.Equals("Name")).Value;
            string role = claims.Single(c => c.Type.Equals("Role")).Value;
            Guid avatar = new Guid(claims.Single(c => c.Type.Equals("Avatar")).Value);
            return new JWTModel(id, ipAddress, name, role, avatar);
        }
    }
}
