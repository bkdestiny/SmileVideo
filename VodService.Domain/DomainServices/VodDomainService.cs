using Common.Models;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VodService.Domain.Entities;
using VodService.Domain.IRepositories;

namespace VodService.Domain.DomainServices
{
    public class VodDomainService
    {
        private readonly IVodVideoRepository vodVideoRepository;

        private readonly IVodVideoClassifyRepository vodVideoClassifyRepository;

        public VodDomainService(IVodVideoRepository vodVideoRepository, IVodVideoClassifyRepository vodVideoClassifyRepository)
        {
            this.vodVideoRepository = vodVideoRepository;
            this.vodVideoClassifyRepository = vodVideoClassifyRepository;
        }
        /// <summary>
        /// 新增视频
        /// </summary>
        /// <param name="vodVideo"></param>
        /// <returns></returns>
        public async Task<(bool,string)> AddVodVideoAsync(VodVideo vodVideo)
        {
            List<VodVideo> videoList=await vodVideoRepository.FindVodVideoByVideoNameAsync(vodVideo.VideoName);
            if (videoList.Count>0)
            {
                //该视频名称已被使用
                return (false,"该视频名称已被使用"); 
            }
            await vodVideoRepository.AddVodVideoAsync(vodVideo);
            return (true, "新增成功");
        }
        /// <summary>
        /// 更新视频
        /// </summary>
        /// <param name="vodVideo"></param>
        /// <returns></returns>
        public async Task<(bool, string)> UpdateVodVideoAsync(VodVideo vodVideo) {
            List<VodVideo> videoList = await vodVideoRepository.FindVodVideoByVideoNameAsync(vodVideo.VideoName);
            if (videoList.Count > 0 && videoList[0].Id!=vodVideo.Id)
            {
                //该视频名称已被使用
                return (false, "该视频名称已被使用");
            }
            vodVideoRepository.UpdateVodVideo(vodVideo);
            return (true, "更新成功");
        }
        /// <summary>
        /// 获取视频分页数据
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public async Task<PagingData<D>> GetVodVideoPagingDataAsync<D>(D dtoType,int pageSize,int pageIndex,Guid? classifyId)
        {
            IQueryable<VodVideo> queryable=vodVideoRepository.VodVideoQueryable().Where(e=>!e.IsDeleted);
            if (classifyId != null)
            {
                queryable=queryable.Where(e=>e.VideoClassifies.Any(c=>c.Id==classifyId));
            }
            PagingData<D> pagingData=await PagingData<D>.CreateAsync(queryable,dtoType, pageSize, pageIndex);
            return pagingData; 
        }
        /// <summary>
        /// 根据Id获取视频
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<VodVideo?> GetVodVideoByIdAsync(Guid id)
        {
           return await vodVideoRepository.FindVodVideoById(id);
        }
        /// <summary>
        /// 根据Id获取视频分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<VodVideoClassify?> GetVodVideoClassifyByIdAsync(Guid id) {
            return await vodVideoClassifyRepository.FindVodVideoClassifyByIdAsync(id);
        }
        /// <summary>
        /// 新增视频分类
        /// </summary>
        /// <param name="vodVideoClassify"></param>
        /// <returns></returns>
        public async Task<(bool,string)> AddVodVideoClassifyAsync(VodVideoClassify vodVideoClassify)
        {
            List<VodVideoClassify> classifyList=await vodVideoClassifyRepository.FindVodVideoClassifyByClassifyNameAsync(vodVideoClassify.ClassifyName);
            if (classifyList.Count > 0) {
                return (false, "该视频分类名称已被使用");
            }
            await vodVideoClassifyRepository.AddVodVideoClassifyAsync(vodVideoClassify);
            return (true, "");
        }
        /// <summary>
        /// 更新视频分类
        /// </summary>
        /// <param name="vodVideoClassify"></param>
        /// <returns></returns>
        public async Task<(bool,string)> UpdateVodVideoClassifyAsync(VodVideoClassify vodVideoClassify) {
            List<VodVideoClassify> classifyList = await vodVideoClassifyRepository.FindVodVideoClassifyByClassifyNameAsync(vodVideoClassify.ClassifyName);
            if (classifyList.Count > 0 && classifyList[0].Id!=vodVideoClassify.Id)
            {
                return (false, "该视频分类名称已被使用");
            }
            vodVideoClassifyRepository.UpdateVodVideoClassify(vodVideoClassify);
            return (true, "更新成功");
        }
        /// <summary>
        /// 获取视频分类分页数据
        /// </summary>
        /// <returns></returns>
        public async Task<PagingData<D>> GetVodVideoClassifyPagingDataAsync<D>(D dtoType,int pageSize,int pageIndex)
        {
            IQueryable<VodVideoClassify> queryable= vodVideoClassifyRepository.VodVideoClassifyfQueryable();
            PagingData<D> pagingData=await PagingData<D>.CreateAsync(queryable,dtoType, pageSize, pageIndex);
            return pagingData;
        }
    }
}
