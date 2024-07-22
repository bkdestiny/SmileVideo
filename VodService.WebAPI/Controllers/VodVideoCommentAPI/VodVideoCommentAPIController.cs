using Common;
using Common.Attributes;
using Common.Models;
using Microsoft.AspNetCore.Mvc;
using VodService.Domain.DomainServices;
using VodService.Domain.Entities;
using VodService.WebAPI.Controllers.VodVideoCommentAPI.Dtos;

namespace VodService.WebAPI.Controllers.VodVideoCommentAPI
{
    [Route("VodVideoComment")]
    [ApiController]
    public class VodVideoCommentAPIController : ControllerBase
    {
        private readonly VodDomainService vodDomainService;

        public VodVideoCommentAPIController(VodDomainService vodDomainService)
        {
            this.vodDomainService = vodDomainService;
        }

        /// <summary>
        /// 新增根评论
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("AddRootVodVideoComment")]
        [Idempotent]
        public async Task<ActionResult<Result>> AddRootVodVideoComment(VodVideoCommentDto dto)
        {
           var vr=new AddRootVodVideoCommentDtoValidator().Validate(dto);
            if (!vr.IsValid) {
                return Result.Error(vr.Errors[0].ErrorMessage);
            }
            VodVideoComment vodVideoComment=VodVideoComment.CreateRootVideoComment(UserContext.UserInfo.Id, dto.Content);
            (bool addSuccess,string addMessage)=await vodDomainService.AddVodVideoCommentAsync(dto.VideoId, vodVideoComment);
            if (!addSuccess)
            {
                return Result.Error(addMessage);
            }
            return Result.Ok();
        }
        [HttpPost("AddSubVodVideoComment")]
        [Idempotent]
        public async Task<ActionResult<Result>> AddSubVodVideoComment(VodVideoCommentDto dto)
        {
            var vr=new AddSubVodVideoCommentDtoValidator().Validate(dto);
            if (!vr.IsValid) {
                return Result.Error(vr.Errors[0].ErrorMessage);
            }
            VodVideoComment vodVideoComment = VodVideoComment.CreateSubVideoComment(UserContext.UserInfo.Id, dto.Content);
            (bool addSuccess, string addMessage) = await vodDomainService.AddVodVideoCommentAsync(dto.VideoId, vodVideoComment,dto.RootVideoCommentId);
            if (!addSuccess)
            {
                return Result.Error(addMessage);
            }
            return Result.Ok();
        }
    }
}
