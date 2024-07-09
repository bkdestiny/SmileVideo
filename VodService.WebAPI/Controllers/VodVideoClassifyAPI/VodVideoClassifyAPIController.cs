using Common.Attributes;
using Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VodService.Domain.DomainServices;
using VodService.Domain.Entities;
using VodService.Infrastructure;
using VodService.WebAPI.Controllers.VodVideoClassifyAPI.Dtos;

namespace VodService.WebAPI.Controllers.VodVideoClassifyAPI
{
    [Route("VodVideoClassify")]
    [ApiController]
    [UnitOfWork(typeof(VodDbContext))]
    public class VodVideoClassifyAPIController : ControllerBase
    {
        private readonly VodDomainService vodDomainService;

        public VodVideoClassifyAPIController(VodDomainService vodDomainService)
        {
            this.vodDomainService = vodDomainService;
        }
        /// <summary>
        /// 新增视频分类
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("AddVodVideoClassify")]
        [Idempotent]
        public async Task<ActionResult<Result>> AddVodVideoClassify(VodVideoClassifyDto dto)
        {
            var vr=new VodVideoClassifyDtoValidator().Validate(dto);
            if (!vr.IsValid)
            {
                return Result.Error(vr.Errors[0].ErrorMessage);
            }
            VodVideoClassify vodVideoClassify = new VodVideoClassify(Guid.Empty, dto.ClassifyName, dto.ClassifyType, dto.SortIndex);
            (bool addSuccess,string addMessage)=await vodDomainService.AddVodVideoClassifyAsync(vodVideoClassify);
            if (!addSuccess)
            {
                return Result.Error(addMessage);
            }
            return Result.Ok();
        }
        /// <summary>
        /// 更新视频分类信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("UpdateVodVideoClassify")]
        public async Task<ActionResult<Result>> UpdateVodVideoClassify(VodVideoClassifyDto dto)
        {
            var vr = new VodVideoClassifyDtoValidator().Validate(dto);
            if (!vr.IsValid)
            {
                return Result.Error(vr.Errors[0].ErrorMessage);
            }
            VodVideoClassify vodVideoClassify = new VodVideoClassify(dto.Id, dto.ClassifyName, dto.ClassifyType, dto.SortIndex);
            (bool updateSuccess,string updateMessage)=await vodDomainService.UpdateVodVideoClassifyAsync(vodVideoClassify);
            if (!updateSuccess) { 
                return Result.Error(updateMessage);
            }
            return Result.Ok();
        }
        /// <summary>
        /// 获取视频分类信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetVodVideoClassify")]
        public async Task<ActionResult<Result>> GetVodVideoClassify(Guid id)
        {
            VodVideoClassify? vodVideoClassify=await vodDomainService.GetVodVideoClassifyByIdAsync(id);
            if(vodVideoClassify == null)
            {
                return Result.DataQueryFailed;
            }
            return Result.Ok(vodVideoClassify); 
        }
        /// <summary>
        /// 获取视频分类分页数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet("GetVodVideoClassifyPagingData")]
        public async Task<ActionResult<Result>> GetVodVideoClassifyPagingData([FromQuery]GetVodVideoClassifyPagingDataRequest req)
        {
            var vr=new GetVodVideoClassifyPagingDataRequestValidator().Validate(req);
            if (!vr.IsValid) { 
                return Result.Error(vr.Errors[0].ErrorMessage);
            }
            IEnumerable<VodVideoClassifyDto> vodVideoClassifiesEnumerable=(await vodDomainService.QueryVodVideoClassifyAsync(req.classifyType)).Select(e=>new VodVideoClassifyDto(e));
            PagingData pagingData=PagingData.Create(vodVideoClassifiesEnumerable, req.PageSize, req.PageIndex);
            return Result.Ok(pagingData);
        }
    }
}
