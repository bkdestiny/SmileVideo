using Common.Models;
using COSXML.Model.Object;
using FileService.Domain.DomainServices;
using FileService.WebAPI.Controllers.UploadAPI.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FileService.WebAPI.Controllers.UploadAPI
{
    [Route("Upload")]
    [ApiController]
    public class UploadAPIController : ControllerBase
    {
        private readonly SysFileDomainService sysFileDomainService;

        public UploadAPIController(SysFileDomainService sysFileDomainService)
        {
            this.sysFileDomainService = sysFileDomainService;
        }

        [HttpPost("UploadFile")]
        public async Task<ActionResult<Result>> UploadFromAdmin(UploadFromAdminRequest req)
        {
            var valid=new UploadFromAdminRequestValidaror().Validate(req);
            if (!valid.IsValid) {
                return Result.Error(valid.Errors[0].ErrorMessage);
            }
            Guid? Id=await sysFileDomainService.SaveFileAsync(req.File);
            if (Id == null)
            {
                return Result.Error("文件上传失败");
            }
            return Result.Ok(Id);
        }
    }
}
