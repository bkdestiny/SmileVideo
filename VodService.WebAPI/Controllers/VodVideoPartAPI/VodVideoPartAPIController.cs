using Common.Attributes;
using Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VodService.Domain.DomainServices;
using VodService.Domain.Entities;
using VodService.Infrastructure;
using VodService.WebAPI.Controllers.VodVideoPartAPI.Dtos;

namespace VodService.WebAPI.Controllers.VodVideoPartAPI
{
    [Route("VodVideoPart")]
    [ApiController]
    [UnitOfWork([typeof(VodDbContext)])]
    [Authorize]
    public class VodVideoPartAPIController : ControllerBase
    {
        private readonly VodDomainService vodDomainService;

        public VodVideoPartAPIController(VodDomainService vodDomainService)
        {
            this.vodDomainService = vodDomainService;
        }
        /// <summary>
        /// 新增视频片段
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("AddVodVideoPart")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Result>> AddVodVideoPart(VodVideoPartDto dto)
        {
            var vr=new AddVodVideoPartDtoValidator().Validate(dto);
            if (!vr.IsValid) {
                return Result.Error(vr.Errors[0].ErrorMessage);
            }
            VodVideoPart vodVideoPart = new VodVideoPart(Guid.Empty,dto.PartName,dto.PartFile,dto.ReleaseTime,dto.PartStatus,dto.SortIndex);
            (bool addSuccess,string addMessage)=await vodDomainService.AddVodVideoPartAsync(dto.VideoId, vodVideoPart);
            if (!addSuccess) { 
                return Result.Error(addMessage);
            }
            return Result.Ok();
        }
        /// <summary>
        /// 更新视频片段
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("UpdateVodVideoPart")]
        [Authorize(Roles = "Admin")]
        public ActionResult<Result> UpdateVodVideoPart(VodVideoPartDto dto)
        {
            var vr=new UpdateVodVideoPartDtoValidator().Validate(dto);
            if (!vr.IsValid) {
                return Result.Error(vr.Errors[0].ErrorMessage);
            } 
            VodVideoPart vodVideoPart = new VodVideoPart(dto.Id, dto.PartName, dto.PartFile, dto.ReleaseTime, dto.PartStatus, dto.SortIndex);
            (bool updateSuccess,string updateMessage)=vodDomainService.UpdateVodVideoPart(vodVideoPart);
            if (!updateSuccess) { 
                return Result.Error(updateMessage);
            }
            return Result.Ok();
        }
        /// <summary>
        /// 根据Id获取视频片段信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetVodVideoPart")]
        public async Task<ActionResult<Result>> GetVodVideoPart([FromQuery]Guid id)
        {
            VodVideoPart? vodVideoPart=await vodDomainService.GetVodVideoPartAsync(id);
            if (vodVideoPart == null) {
                return Result.DataQueryFailed;
            }
            VodVideoPartDto dto=new VodVideoPartDto(vodVideoPart);
            return Result.Ok(dto);
        }
        /// <summary>
        /// 获取视频片段分页数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet("GetVodVideoPartPagingData")]
        public async Task<ActionResult<Result>> GetVodVideoPartPagingData([FromQuery]GetVodVideoPartPagingDataRequest req)
        {
            var vr=new GetVodVideoPartPagingDataRequestValidator().Validate(req);
            if (!vr.IsValid) {
                return Result.Error(vr.Errors[0].ErrorMessage);
            }
            IEnumerable<VodVideoPartDto> enumerable=(await vodDomainService.QueryVodVideoPartAsync(req.VideoId,req.SearchText)).Select(p=>new VodVideoPartDto(p)).ToList();
            PagingData pagingData=PagingData.Create(enumerable, req.PageSize, req.PageIndex);
            return Result.Ok(pagingData);   
        }

        [HttpPost("RemoveVodVideoPart")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Result>> RemoveVodVideoPart(List<Guid> ids)
        {
            await vodDomainService.RemoveVodVideoPartAsync(ids.ToArray());
            return Result.Ok();
        }
    }
}
