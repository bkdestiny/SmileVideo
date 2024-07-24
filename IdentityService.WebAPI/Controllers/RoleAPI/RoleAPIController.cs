using Common.Attributes;
using IdentityService.Domain.DomainServices;
using IdentityService.Domain.Entites;
using IdentityService.Infrastructure;
using IdentityService.WebAPI.Controllers.RoleAPI.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.WebAPI.Controllers.RoleAPI
{
    [Route("Role")]
    [ApiController]
    [Authorize]
    [UnitOfWork([typeof(IdDbContext)])]
    public class RoleAPIController : ControllerBase
    {
        private readonly IdentityDomainService identityDomainService;

        private readonly UserManager<User> userManager;
        public RoleAPIController(IdentityDomainService identityDomainService, UserManager<User> userManager)
        {
            this.identityDomainService = identityDomainService;
            this.userManager = userManager;
        }
        /// <summary>
        /// 获取角色分页数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet("GetRolePagingData")]
        public async Task<ActionResult<Result>> GetRolePagingData([FromQuery]GetRolePagingDataRequest req)
        {
            var vr = new GetRolePagingDataRequestValidator().Validate(req);
            if (!vr.IsValid)
            {
                return Result.Error(vr.Errors[0].ErrorMessage);
            }
            IList<RoleDto> dtos=(await identityDomainService.QueryRoleAsync(req.SearchText)).Select(e=>new RoleDto(e)).ToList();
            if (req.UserId != Guid.Empty)
            {
                User? user=await userManager.FindByIdAsync(req.UserId.ToString());
                if (user == null)
                {
                    return Result.Error("不存在该用户");
                }
                IList<string> userRoleNames= await userManager.GetRolesAsync(user);
                dtos=dtos.Where(e=>userRoleNames.Contains(e.Name)).ToList();
            }
            PagingData pagingData=PagingData.Create(dtos,req.PageSize,req.PageIndex);
            return Result.Ok(pagingData);   
        }
        /// <summary>
        /// 获取单个角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetRole")]
        public async Task<ActionResult<Result>> GetRolePagingData([FromQuery]Guid id)
        {
            Role? role=await identityDomainService.GetRoleAsync(id);
            if(role == null)
            {
                return Result.DataQueryFailed;
            }
            return Result.Ok(role);
        }
        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("AddRole")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Result>> AddRole(RoleDto dto)
        {
            var vr = new AddRoleDto().Validate(dto);
            if (!vr.IsValid)
            {
                return Result.Error(vr.Errors[0].ErrorMessage);
            }
            Role role=new Role(dto.Name,dto.Description);
            (bool success,string message)=await identityDomainService.AddRoleAsync(role);
            if (!success)
            {
                return Result.Error(message);
            }
            return Result.Ok();
        }
        /// <summary>
        /// 更新角色信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("UpdateRole")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Result>> UpdateRole(RoleDto dto)
        {
            var vr = new UpdateRoleDto().Validate(dto);
            if (!vr.IsValid)
            {
                return Result.Error(vr.Errors[0].ErrorMessage);
            }
            (bool success, string message) = await identityDomainService.UpdateRoleAsync(dto.Id,dto.Name,dto.Description);
            if (!success)
            {
                return Result.Error(message);
            }
            return Result.Ok();
        }
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost("RemoveRole")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Result>> RemoveRole(Guid[] ids)
        {
            await identityDomainService.RemoveRoleAsync(ids);
            return Result.Ok();
        }
    }
}
