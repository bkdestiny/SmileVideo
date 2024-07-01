using Common.Models;
using FileService.Domain.DomainServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FileService.WebAPI.Controllers.SysFileAPI
{
    [Route("SysFile")]
    [ApiController]
    public class SysFileAPIControllerController : ControllerBase
    {
        private readonly SysFileDomainService sysFileDomainService;

        public SysFileAPIControllerController(SysFileDomainService sysFileDomainService)
        {
            this.sysFileDomainService = sysFileDomainService;
        }
        /// <summary>
        /// 根据Id获取文件存储地址
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetSysFileUrl")]
        public async Task<ActionResult<Result>> GetSysFileUrl(Guid id)
        {
            Uri? sysFileUrl=await sysFileDomainService.GetSysFileUrl(id);
            if (sysFileUrl == null) {
                return Result.Error("该文件不存在");
            }
            return Result.Ok(sysFileUrl);
        }   
    }
}
