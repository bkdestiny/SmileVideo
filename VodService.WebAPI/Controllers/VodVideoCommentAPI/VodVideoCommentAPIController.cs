using Common;
using Common.Attributes;
using Common.EFcore.Models;
using Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VodService.Domain.DomainServices;
using VodService.Domain.Entities;
using VodService.Infrastructure;
using VodService.WebAPI.Controllers.VodVideoCommentAPI.Dtos;

namespace VodService.WebAPI.Controllers.VodVideoCommentAPI
{
    [Route("VodVideoComment")]
    [ApiController]
    [Authorize]
    [UnitOfWork([typeof(VodDbContext)])]
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
            var vr = new AddRootVodVideoCommentDtoValidator().Validate(dto);
            if (!vr.IsValid)
            {
                return Result.Error(vr.Errors[0].ErrorMessage);
            }
            VodVideoComment vodVideoComment = VodVideoComment.CreateRootVideoComment(UserContext.UserInfo.Id, dto.Content);
            (bool addSuccess, string addMessage) = await vodDomainService.AddVodVideoCommentAsync(dto.VideoId, vodVideoComment);
            if (!addSuccess)
            {
                return Result.Error(addMessage);
            }
            return Result.Ok();
        }

        /// <summary>
        /// 新增子评论
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("AddSubVodVideoComment")]
        [Idempotent]
        public async Task<ActionResult<Result>> AddSubVodVideoComment(VodVideoCommentDto dto)
        {
            var vr = new AddSubVodVideoCommentDtoValidator().Validate(dto);
            if (!vr.IsValid)
            {
                return Result.Error(vr.Errors[0].ErrorMessage);
            }
            VodVideoComment vodVideoComment = VodVideoComment.CreateSubVideoComment(UserContext.UserInfo.Id, dto.Content);
            (bool addSuccess, string addMessage) = await vodDomainService.AddVodVideoCommentAsync(dto.VideoId, vodVideoComment, dto.RootVideoCommentId);
            if (!addSuccess)
            {
                return Result.Error(addMessage);
            }
            return Result.Ok();
        }
        /// <summary>
        /// 新增回复子评论
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("AddAnswerSubVodVideoComment")]
        [Idempotent]
        public async Task<ActionResult<Result>> AddAnswerSubVodVideoComment(VodVideoCommentDto dto)
        {
            var vr = new AddAnswerSubVodVideoCommentDtoValidator().Validate(dto);
            if (!vr.IsValid)
            {
                return Result.Error(vr.Errors[0].ErrorMessage);
            }
            VodVideoComment vodVideoComment = VodVideoComment.CreateAnswerSubVideoComment(UserContext.UserInfo.Id, dto.RespondentUserId, dto.Content);
            (bool success, string message) = await vodDomainService.AddVodVideoCommentAsync(dto.VideoId, vodVideoComment, dto.RootVideoCommentId);
            if (!success)
            {
                return Result.Error(message);
            }
            return Result.Ok();
        }

        [HttpGet("GetVodVideoCommentPagingData")]
        public async Task<ActionResult<Result>> GetVodVideoCommentPagingData([FromQuery] GetVodVideoCommentPagingDataRequest req)
        {
            var vr = new GetVodVideoCommentPagingDataRequestValidator().Validate(req);
            if (!vr.IsValid)
            {
                return Result.Error(vr.Errors[0].ErrorMessage);
            }
            IList<VodVideoCommentDto> dtos = (await vodDomainService.QueryVodVideoCommentAsync(req.VideoId, req.RootVideoCommentId)).Select(e => new VodVideoCommentDto(e)).ToList();
            if (req.SortOrderOfCreateTime != null)
            {
                if (req.SortOrderOfCreateTime == SortOrders.Asc)
                {
                    dtos = dtos.OrderBy(e => e.CreateTime).ToList();
                }
                else if (req.SortOrderOfCreateTime == SortOrders.Desc)
                {
                    dtos = dtos.OrderByDescending(e => e.CreateTime).ToList();
                }
            }
            PagingData pagingData = PagingData.Create(dtos, req.PageSize, req.PageIndex);
            return Result.Ok(pagingData);
        }
    }
}
