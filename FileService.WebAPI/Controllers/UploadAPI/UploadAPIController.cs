using Common.Attributes;
using Common.Models;
using Common.Utils;
using COSXML.Model.Object;
using FileService.Domain.DomainServices;
using FileService.Infrastructure;
using FileService.WebAPI.Controllers.UploadAPI.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FileService.WebAPI.Controllers.UploadAPI
{
    [Route("Upload")]
    [ApiController]
    [UnitOfWork([typeof(SysFileDbContext)],EnableTransaction =false)]
    public class UploadAPIController : ControllerBase
    {
        private readonly SysFileDomainService sysFileDomainService;

        public UploadAPIController(SysFileDomainService sysFileDomainService)
        {
            this.sysFileDomainService = sysFileDomainService;
        }
        /// <summary>
        /// 角色Admin的上传文件接口
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("UploadFile")]
        [Idempotent]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Result>> UploadFromAdmin(UploadFromAdminRequest req)
        {
            var valid = new UploadFromAdminRequestValidaror().Validate(req);
            if (!valid.IsValid)
            {
                return Result.Error(valid.Errors[0].ErrorMessage);
            }
            Guid id = await sysFileDomainService.SaveFileAsync(req.File);
            if (id == Guid.Empty)
            {
                return Result.Error("文件上传失败");
            }
            return Result.Ok(id);
        }
        /// <summary>
        /// 上传视频文件并转换为M3u8文件
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("UploadVideoAndConvertM3u8")]
        [RequestSizeLimit(3_000_000_000)]
        [Authorize(Roles = "Admin")]
        [Idempotent]
        public async Task<ActionResult<Result>> UploadVideoAndConvertM3u8(UploadVideoAndConvertM3u8Request req)
        {
            var vr = new UploadVideoAndConvertM3u8RequestValidator().Validate(req);
            if (!vr.IsValid)
            {
                return Result.Error(vr.Errors[0].ErrorMessage);
            }
            Guid id=await sysFileDomainService.ConvertHlsAndSaveFile(req.File);
            if (id == Guid.Empty) {
                return Result.Error("文件上传失败");
            }
            return Result.Ok(id);
        }
    }
}
