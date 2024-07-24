using Common.Attributes;
using Common.Models;
using Common.Utils;
using FileService.Domain.DomainServices;
using FileService.Domain.Entites;
using FileService.Infrastructure;
using FileService.WebAPI.Controllers.SysFileAPI.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace FileService.WebAPI.Controllers.SysFileAPI
{
    [Route("SysFile")]
    [ApiController]
    [UnitOfWork([typeof(SysFileDbContext)],EnableTransaction =false)]
    [Authorize]
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
        [HttpGet("GetUrl")]
        public async Task<ActionResult<Result>> GetUrl([FromQuery]Guid id)
        {
            Uri? sysFileUrl=await sysFileDomainService.GetSysFileUrl(id);
            if (sysFileUrl == null) {
                return Result.Error("该文件不存在");
            }
            return Result.Ok(sysFileUrl);
        }
        /// <summary>
        /// 查询系统文件分页数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet("GetSysFilePagingData")]
        public async Task<ActionResult<Result>> GetSysFilePagingData([FromQuery]GetSysFilePagingDataRequest req)
        {
            var vr=new GetSysFilePagingDataRequestValidator().Validate(req);
            if (!vr.IsValid) {
                return Result.Error(vr.Errors[0].ErrorMessage);
            }
            IEnumerable<SysFileDto> enumerable=(await sysFileDomainService.QuerySysFileAsync(req.FileIds.Split(",").Select(id=>Guid.Parse(id)).ToArray())).Select(e=>new SysFileDto(e)).ToList();
            PagingData pagingData=PagingData.Create(enumerable,req.PageSize,req.PageIndex);
            return Result.Ok(pagingData);
        }
    }
}
