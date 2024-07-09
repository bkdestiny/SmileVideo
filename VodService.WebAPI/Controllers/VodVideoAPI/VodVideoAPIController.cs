using Common.Attributes;
using Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using VodService.Domain.DomainServices;
using VodService.Domain.Entities;
using VodService.Domain.IRepositories;
using VodService.Infrastructure;
using VodService.WebAPI.Controllers.VodVideoAPI.Dtos;

namespace VodService.WebAPI.Controllers.VodVideoAPI
{
    [Route("VodVideo")]
    [ApiController]
    [UnitOfWork(typeof(VodDbContext))]
    public class VodVideoAPIController : ControllerBase
    {
        private readonly VodDomainService vodDomainService;

        private readonly IVodVideoRepository vodVideoRepository;

        public VodVideoAPIController(VodDomainService vodDomainService, IVodVideoRepository vodVideoRepository)
        {
            this.vodDomainService = vodDomainService;
            this.vodVideoRepository = vodVideoRepository;
        }
        /// <summary>
        /// 新增视频
        /// </summary>
        /// <param name="vodVideoDto"></param>
        /// <returns></returns>
        [HttpPost("AddVodVideo")]
        [Idempotent]
        public async Task<ActionResult<Result>> AddVodVideo(VodVideoDto vodVideoDto)
        {
            var vr = new VodVideoDtoValidator().Validate(vodVideoDto);
            if (!vr.IsValid)
            {
                return Result.Error(vr.Errors[0].ErrorMessage);
            }
            VodVideo vodVideo = new VodVideo(Guid.Empty, vodVideoDto.VideoName, vodVideoDto.CoverFile, vodVideoDto.Performers, vodVideoDto.Director, vodVideoDto.Scriptwriter, vodVideoDto.Description, vodVideoDto.Profile, vodVideoDto.VideoStatus);
            (bool addSuccess, string addMessage) = await vodDomainService.AddVodVideoAsync(vodVideo, vodVideoDto.VideoClassifies.ToArray());
            if (!addSuccess)
            {
                return Result.Error(addMessage);
            }
            return Result.Ok();
        }
        /// <summary>
        /// 更新视频
        /// </summary>
        /// <param name="vodVideoDto"></param>
        /// <returns></returns>
        [HttpPost("UpdateVodVideo")]
        public async Task<ActionResult<Result>> UpdateVodVideo(VodVideoDto vodVideoDto)
        {
            var vr = new VodVideoDtoValidator().Validate(vodVideoDto);
            if (!vr.IsValid)
            {
                return Result.Error(vr.Errors[0].ErrorMessage);
            }
            VodVideo vodVideo = new VodVideo(vodVideoDto.Id, vodVideoDto.VideoName, vodVideoDto.CoverFile, vodVideoDto.Performers, vodVideoDto.Director, vodVideoDto.Scriptwriter, vodVideoDto.Description, vodVideoDto.Profile, vodVideoDto.VideoStatus);
            (bool updateSuccess, string updateMessage) = await vodDomainService.UpdateVodVideoAsync(vodVideo, vodVideoDto.VideoClassifies.ToArray());
            if (!updateSuccess)
            {
                return Result.Error(updateMessage);
            }
            return Result.Ok();
        }
        /// <summary>
        /// 获取单个视频信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetVodVideo")]
        public async Task<ActionResult<Result>> GetVodVideo([FromQuery] Guid id)
        {
            VodVideo? video = await vodDomainService.GetVodVideoByIdAsync(id);
            if (video == null)
            {
                return Result.DataQueryFailed;
            }
            VodVideoDto videoDto = new VodVideoDto(video);
            return Result.Ok(videoDto);
        }
        /// <summary>
        /// 获取视频分页数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet("GetVodVideoPagingData")]
        public async Task<ActionResult<Result>> GetVodVideoPagingData([FromQuery] GetVodVideoPagingDataRequest req)
        {
            var vr = new GetVodVideoPagingDataRequestValidator().Validate(req);
            if (!vr.IsValid)
            {
                return Result.Error(vr.Errors[0].ErrorMessage);
            }

            IEnumerable<VodVideoDto> vodVideoEnumerable = (await vodDomainService.QueryVodVideoAsync(req.ClassifyId)).Select(e => new VodVideoDto(e));
            PagingData pagingData = PagingData.Create(vodVideoEnumerable, req.PageSize, req.PageIndex);
            return Result.Ok(pagingData);
        }

        [HttpGet("TestVodVideo")]
        public async Task<ActionResult<Result>> TestVodVideo([FromQuery] Guid id)
        {
           List<VodVideo>? vodVideo = await vodVideoRepository.Test(id);
            return Result.Ok(vodVideo);
        }
    }
}
