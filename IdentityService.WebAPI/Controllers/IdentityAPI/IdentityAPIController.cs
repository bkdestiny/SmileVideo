using Common.JWT;
using IdentityService.Domain.Entites;
using IdentityService.Domain.Services;
using IdentityService.WebAPI.Controllers.IdentityAPI.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;
namespace IdentityService.WebAPI.Controllers.IdentityAPI
{
    [Route("Identity")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IdentityDomainService identityDomainService;
        private readonly IOptions<JWTOptions> jwtOptions;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;
        public UserController(IdentityDomainService identityDomainService, IOptions<JWTOptions> jwtOptions, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            this.identityDomainService = identityDomainService;
            this.jwtOptions = jwtOptions;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        [HttpPost("LoginByUserNameAndPassword")]
        [AllowAnonymous]
        public async Task<ActionResult<Result>> LoginByUserNameAndPassword(LoginByUserNameAndPasswordRequest req)
        {
            User? user = await identityDomainService.LoginByUserNameAndPasswordAsync(req.UserName, req.Password);
            if (user == null)
            {
                return Result.Response(ResponseTypes.UserNameOrPasswordError);
            }
            string ipAddress = HttpHelper.GetRemoteIpAddress(HttpContext);
            IEnumerable<string> roles = await userManager.GetRolesAsync(user);
            string rolesStr = string.Join(",", roles);
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim("IpAddress", ipAddress));
            claims.Add(new Claim("Name", user.UserName));
            claims.Add(new Claim("Role", rolesStr));
            claims.Add(new Claim("AuthorizationTimeStamp", DateTime.Now.Ticks.ToString()));
            string token = JWTHelper.BuildToken(claims, jwtOptions.Value);
            return Result.Ok(token);
        }

        [HttpPost("Authentication")]
        public ActionResult<Result> Authentication()
        {
            return Result.Ok();
        }
    }
}
