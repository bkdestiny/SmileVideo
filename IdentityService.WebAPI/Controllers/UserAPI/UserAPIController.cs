using Common;
using Common.Models;
using IdentityService.Domain.Entites;
using IdentityService.Domain.IRepositories;
using IdentityService.Domain.DomainServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using IdentityService.WebAPI.Controllers.UserAPI.Dtos;
using Microsoft.AspNetCore.Authorization;
using Common.Attributes;
using IdentityService.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.WebAPI.Controllers.UserManageAPI
{
    [ApiController]
    [Route("User")]
    [UnitOfWork([typeof(IdDbContext)])]
    [Authorize]
    public class UserAPIController : ControllerBase
    {
        private readonly UserManager<User> userManager;

        private readonly RoleManager<Role> roleManager;

        private readonly IdentityDomainService identityDomainService;

        private readonly IConfiguration configuration;

        private string defaultPassword="123456";
        public UserAPIController(UserManager<User> userManager,RoleManager<Role> roleManager, IdentityDomainService identityDomainService, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.identityDomainService = identityDomainService;
            this.configuration = configuration;
            string? configDefaultPassword=configuration.GetValue<string>("IdentityService:DefaultPassword");
            if (configDefaultPassword != null)
            {
                this.defaultPassword = configDefaultPassword;
            }
        }
        /// <summary>
        /// 初始化管理员
        /// </summary>
        /// <returns></returns>
        [HttpPost("InitAdmin")]
        [AllowAnonymous]
        public async Task<ActionResult<Result>> InitAdmin()
        {
            string? userName = "admin";
            return await identityDomainService.InitAdmin(userName, defaultPassword);
        }
        /// <summary>
        /// 获取用户分页数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet("GetUserPagingData")]
        public async Task<ActionResult<Result>> GetUserPagingData([FromQuery] GetUserPagingDataRequest req)
        {
            var vr=new GetUserPagingDataRequestValidator().Validate(req);
            if (!vr.IsValid)
            {
                return Result.Error(vr.Errors[0].ErrorMessage);
            }
            IList<UserDto> users=(await identityDomainService.QueryUserAsync(req.SearchText)).Select(e=>new UserDto(e)).ToList();
            PagingData pagingData = PagingData.Create(users, req.PageSize, req.PageIndex);
            return Result.Ok(pagingData);
        }
        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("AddUser")]
        [Idempotent]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Result>> AddUser(UserDto dto)
        {
            var vr=new AddUserDtoValidator().Validate(dto);
            if (!vr.IsValid) {
                return Result.Error(vr.Errors[0].ErrorMessage);
            }
            User user=new User(dto.UserName,dto.PhoneNumber,dto.Email);
            (bool addSuccess,string addMessage)=await identityDomainService.AddUserAsync(user,defaultPassword);
            if (!addSuccess) { 
                return Result.Error(addMessage); 
            }
            return Result.Ok(); 
        }
        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("UpdateUser")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Result>> UpdateUser(UserDto dto)
        {
            var vr = new UpdateUserDtoValidator().Validate(dto);
            if (!vr.IsValid)
            {
                return Result.Error(vr.Errors[0].ErrorMessage);
            }
            (bool success,string message)=await identityDomainService.UpdateUserAsync(dto.Id,dto.UserName,dto.PhoneNumber,dto.Email);
            if (!success)
            {
                return Result.Error(message);
            }
            return Result.Ok();
        }
        /// <summary>
        /// 获取单个用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetUser")]
        public async Task<ActionResult<Result>> GetUser([FromQuery]Guid id)
        {
            User? user=await identityDomainService.GetUserAsync(id);
            if(user == null)
            {
                return Result.DataQueryFailed;
            }
            UserDto dto = new UserDto(user);
            return Result.Ok(dto);
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost("RemoveUser")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Result>> RemoveUser(Guid[] ids)
        {
            await identityDomainService.RemoveUserAsync(ids);
            return Result.Ok();
        }
        /// <summary>
        /// 重置密码(管理员)
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("ResetPasswordFromAdmin")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Result>> ResetPasswordFromAdmin(ResetPasswordRequest req)
        {
            var vr=new ResetPasswordFromAdminFromValidator().Validate(req);
            if (!vr.IsValid)
            {
                return Result.Error(vr.Errors[0].ErrorMessage);
            }
            (bool success,string message)=await identityDomainService.ResetPasswordFromAdminAsync(req.Id, req.NewPassword);
            if (!success) { 
                return Result.Error(message);
            }
            return Result.Ok();
        }
        /// <summary>
        /// 配置用户的角色
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("ResetRolesOfUser")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Result>> ResetRolesOfUser(ResetRolesOfUserReqeuest req)
        {
            var vr=new ResetRolesOfUserRequestValidator().Validate(req);
            if (!vr.IsValid) {
                return Result.Error(vr.Errors[0].ErrorMessage);
            }
            User? user = await userManager.FindByIdAsync(req.UserId.ToString());
            if (user == null) {
                return Result.Error("不存在该用户");
            }
            IList<string> removeRoleNames=await userManager.GetRolesAsync(user);
            var ir=await userManager.RemoveFromRolesAsync(user,removeRoleNames);
            if (!ir.Succeeded)
            {
                return Result.Error("配置角色失败");
            }
            IList<string> newRoleNames=await roleManager.Roles.Where(e => req.RoleIds.Contains(e.Id)).Select(e => e.Name!).ToListAsync();
            ir=await userManager.AddToRolesAsync(user,newRoleNames);
            if (!ir.Succeeded) {
                return Result.Error("配置角色失败");
            }
            return Result.Ok();
        }
    }
}
