using Common;
using Common.Models;
using IdentityService.Domain.Entites;
using IdentityService.Domain.IRepositories;
using IdentityService.Domain.DomainServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.WebAPI.Controllers.UserManageAPI
{
    [ApiController]
    [Route("UserManage")]
    public class UserManageAPIController : ControllerBase
    {
        private readonly UserManager<User> userManager;

        private readonly RoleManager<Role> roleManager;

        private readonly IUserRepository userRepository;

        private readonly IdentityDomainService identityDomainService;

        private readonly IConfiguration configuration;
        public UserManageAPIController(UserManager<User> userManager, IUserRepository userRepository, RoleManager<Role> roleManager, IdentityDomainService identityDomainService, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.userRepository = userRepository;
            this.roleManager = roleManager;
            this.identityDomainService = identityDomainService;
            this.configuration = configuration;
        }
        /// <summary>
        /// 初始化管理员
        /// </summary>
        /// <returns></returns>
        [HttpPost("InitAdmin")]
        public async Task<ActionResult<Result>> InitAdmin()
        {
            string? userName = configuration.GetValue<string>("IdentityService:Admin:UserName");
            string? password = configuration.GetValue<string>("IdentityService:Admin:Password");
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                return Result.Response(ResponseTypes.MissingRelevantConfiguration);
            }
            return await identityDomainService.InitAdmin(userName, password);
        }


    }
}
