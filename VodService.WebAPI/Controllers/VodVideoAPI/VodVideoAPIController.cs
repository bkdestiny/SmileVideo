using Common.Attributes;
using Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using VodService.Domain.DomainServices;
using VodService.Domain.Entities;
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

        public VodVideoAPIController(VodDomainService vodDomainService)
        {
            this.vodDomainService = vodDomainService;
        }
        /// <summary>
        /// 新增视频
        /// </summary>
        /// <param name="vodVideoDto"></param>
        /// <returns></returns>
        [HttpPost("AddVodVideo")]
        public async Task<ActionResult<Result>> AddVodVideo(VodVideoDto vodVideoDto)
        {
            var vr = new VodVideoDtoValidator().Validate(vodVideoDto);
            if (!vr.IsValid)
            {
                return Result.Error(vr.Errors[0].ErrorMessage);
            }
            VodVideo vodVideo = new VodVideo(Guid.Empty, vodVideoDto.VideoName, vodVideoDto.CoverFile, vodVideoDto.Performers, vodVideoDto.Director, vodVideoDto.Scriptwriter, vodVideoDto.Description, vodVideoDto.Profile);
            (bool addSuccess, string addMessage) = await vodDomainService.AddVodVideoAsync(vodVideo);
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
            VodVideo vodVideo = new VodVideo(vodVideoDto.Id, vodVideoDto.VideoName, vodVideoDto.CoverFile, vodVideoDto.Performers, vodVideoDto.Director, vodVideoDto.Scriptwriter, vodVideoDto.Description, vodVideoDto.Profile);
            (bool updateSuccess, string updateMessage) =await vodDomainService.UpdateVodVideoAsync(vodVideo);
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
        public async Task<ActionResult<Result>> GetVodVideo([FromQuery]Guid id)
        {
            VodVideo? video=await vodDomainService.GetVodVideoByIdAsync(id);
            if (video == null)
            {
                return Result.DataQueryFailed;
            }
            VodVideoDto videoDto=new VodVideoDto(video);
            return Result.Ok(videoDto);
        }
        /// <summary>
        /// 获取视频分页数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet("GetVodVideoPagingData")]
        public async Task<ActionResult<Result>> GetVodVideoPagingData([FromQuery]VodVideoPagingDataRequest req)
        {
            var vr=new VodVideoPagingDataRequestValidator().Validate(req);
            if (!vr.IsValid)
            {
                return Result.Error(vr.Errors[0].ErrorMessage);
            }
            var pagingData=await vodDomainService.GetVodVideoPagingDataAsync(typeof(VodVideoDto),req.PageSize, req.PageIndex,req.ClassifyId);
            return Result.Ok(pagingData);
        }
    }
}
