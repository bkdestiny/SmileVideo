using Common.Attributes;
using Common.JWT;
using Common.Sms;
using IdentityService.Domain.Entites;
using IdentityService.Domain.DomainServices;
using IdentityService.WebAPI.Controllers.IdentityAPI.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Net;
namespace IdentityService.WebAPI.Controllers.IdentityAPI
{
    [Route("Identity")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IdentityDomainService identityDomainService;
        private readonly JWTOptions jwtOptions;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly ILogger<UserController> logger;
        private readonly ISms sms;
        
        public UserController(IdentityDomainService identityDomainService, IOptions<JWTOptions> jwtOptions, UserManager<User> userManager, RoleManager<Role> roleManager, ILogger<UserController> logger, ISms sms)
        {
            this.identityDomainService = identityDomainService;
            this.jwtOptions = jwtOptions.Value;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.logger = logger;
            this.sms = sms;
        }

        /// <summary>
        /// 登录 用户名和密码
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("LoginByUserNameAndPassword")]
        [AllowAnonymous]
        public async Task<ActionResult<Result>> LoginByUserNameAndPassword(LoginByUserNameAndPasswordRequest req)
        {
            string ipAddress = HttpHelper.GetRemoteIpAddress(HttpContext);
            User? user = await identityDomainService.LoginByUserNameAndPasswordAsync(req.UserName, req.Password);
            if (user == null)
            {
                return Result.Error("用户名或密码不存在");
            }
            IEnumerable<string> roles = await userManager.GetRolesAsync(user);
/*            string rolesStr = string.Join(",", roles);
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim("IpAddress", ipAddress));
            claims.Add(new Claim("Name", user.UserName!));
            claims.Add(new Claim("Role", rolesStr));
            claims.Add(new Claim("AuthorizationTime", DateTime.Now.Ticks.ToString()));
            long expirationTime = DateTime.Now.AddSeconds(jwtOptions.ExpireSeconds).Ticks;
            claims.Add(new Claim("ExpirationTime", expirationTime.ToString()));
            string token = JWTHelper.BuildToken(claims, jwtOptions);*/
            string token = JWTHelper.BuildToken(new JWTModel(user.UserName!, string.Join(",", roles), ipAddress, user.Avatar), jwtOptions);
            Result result=Result.Ok("登录成功", token);
            return result;
        }

        /// <summary>
        /// 校验Token是否有效
        /// </summary>
        /// <returns></returns>
        [HttpPost("Authentication")]
        public ActionResult<Result> Authentication()
        {
            return Result.Ok();
        }
        /// <summary>
        /// 发送登录或注册短信验证码
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("SendLoginOrRegisterVerifyCode")]
        [AllowAnonymous]
        public async Task<ActionResult<Result>> SendLoginOrRegisterVerifyCode(SendLoginOrRegisterVerifyCodeRequest req)
        {
            var valid=new SendLoginOrRegisterVerifyCodeRequestValidator().Validate(req);
            if (!valid.IsValid) {
                return Result.Error(valid.Errors[0].ErrorMessage);
            }
            (bool success,string? message)=await identityDomainService.SendLoginOrRegisterVerifyCode(req.PhoneNumber);
            if (!success)
            {
                return Result.Error(message!);
            }
            return Result.Ok();
        }

        /// <summary>
        /// 登录或注册 
        /// </summary>
        /// <param name="req">手机号码,验证码</param>
        /// <returns></returns>
        [HttpPost("LoginOrRegitserByPhoneNumberAndVerifyCode")]
        [AllowAnonymous]
        [Idempotent]
        public async Task<ActionResult<Result>> LoginOrRegitserByPhoneNumberAndVerifyCode(LoginOrRegisterByPhoneNumberAndVerifyCodeRequest req)
        {
            var valid = new LoginOrRegisterByPhoneNumberAndVerifyCodeRequestValidator().Validate(req);
            if (!valid.IsValid)
            {
                return Result.Error(valid.Errors[0].ErrorMessage);
            }
           User user=await identityDomainService.LoginOrRegitserByPhoneNumberAndVerifyCodeAsync(req.PhoneNumber, req.VerifyCode);
            if (user == null)
            {
                return Result.Error();
            }
            IEnumerable<string> roles=await userManager.GetRolesAsync(user);
            string ipAddress = HttpHelper.GetRemoteIpAddress(this.HttpContext);
            string token=JWTHelper.BuildToken(new JWTModel(user.UserName!, string.Join(",", roles), ipAddress,user.Avatar),jwtOptions);
            return Result.Ok(token);

        }


        [HttpPost("Test")]
        [AllowAnonymous]
        public async Task<ActionResult<Result>> Test(string phoneNumber,string verifyCode)
        {
            (bool b,string message)=await sms.SendAsync(phoneNumber,sms.GetSmsTemplate().VerifyCode!,sms.GetSmsTemplate().VerifyCodeParam!,verifyCode); 
            return Result.Ok(message);
        }

        
    }
}
