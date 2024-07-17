using Common.Attributes;
using Common.EFcore.Models;
using Common.Models;
using Microsoft.AspNetCore.Authorization;
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
    [UnitOfWork([typeof(VodDbContext)])]
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
        [Authorize(Roles ="Admin")]
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
        [Authorize(Roles = "Admin")]
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
            IEnumerable<VodVideoClassifyDto> vodVideoClassifiesEnumerable=(await vodDomainService.QueryVodVideoClassifyAsync(req.classifyType,req.SearchText)).Select(e=>new VodVideoClassifyDto(e));
            if (req.SortOrderOfSortIndex != null&&req.SortOrderOfSortIndex==SortOrders.Asc)
            {
                vodVideoClassifiesEnumerable = vodVideoClassifiesEnumerable.OrderBy(e => e.SortIndex).ToList();
            }
            else if(req.SortOrderOfSortIndex != null && req.SortOrderOfSortIndex == SortOrders.Desc)
            {
                vodVideoClassifiesEnumerable=vodVideoClassifiesEnumerable.OrderByDescending(e => e.SortIndex).ToList();
            }
            PagingData pagingData=PagingData.Create(vodVideoClassifiesEnumerable, req.PageSize, req.PageIndex);
            return Result.Ok(pagingData);
        }

        [HttpPost("RemoveVodVideoClassify")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Result>> RemoveVodVideoClassify(List<Guid> ids)
        {
            await vodDomainService.RemoveVodVideoClassifyAsync(ids.ToArray());
            return Result.Ok();
        }
    }
}
