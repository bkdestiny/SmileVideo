using System.Collections.Generic;
using System.Security.Claims;

namespace Common.JWT
{
    public interface ITokenService
    {
        string BuildToken(IEnumerable<Claim> claims, JWTOptions options);
    }
}
