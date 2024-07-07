﻿using Common.Models;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections;
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
        public async Task<(bool,string)> AddVodVideoAsync(VodVideo vodVideo,Guid[] videoClassifyIds)
        {
            List<VodVideo> videoList=await vodVideoRepository.FindVodVideoByVideoNameAsync(vodVideo.VideoName);
            if (videoList.Count>0)
            {
                //该视频名称已被使用
                return (false,"该视频名称已被使用"); 
            }
            //新增视频信息
            await vodVideoRepository.AddVodVideoAsync(vodVideo);
            //绑定视频分类信息
            foreach (Guid videoClassifyId in videoClassifyIds) {
                await vodVideoRepository.BindVodVideoClassify(vodVideo.Id, videoClassifyId);
            }
            return (true, "新增成功");
        }
        /// <summary>
        /// 更新视频
        /// </summary>
        /// <param name="vodVideo"></param>
        /// <returns></returns>
        public async Task<(bool, string)> UpdateVodVideoAsync(VodVideo vodVideo, Guid[] videoClassifyIds) {
/*            List<VodVideo> videoList = await vodVideoRepository.FindVodVideoByVideoNameAsync(vodVideo.VideoName);
            if (videoList.Count > 0 && videoList[0].Id!=vodVideo.Id)
            {
                //该视频名称已被使用
                return (false, "该视频名称已被使用");
            }*/
            //更新视频信息
            vodVideoRepository.UpdateVodVideo(vodVideo);
            //清除原本绑定的视频分类
            await vodVideoRepository.ClearBindVodVideoClassify(vodVideo.Id);
            //更新视频分类
            foreach (Guid videoClassifyId in videoClassifyIds) { 
                await vodVideoRepository.BindVodVideoClassify(vodVideo.Id,videoClassifyId);
            }
            return (true, "更新成功");
        }
        /// <summary>
        /// 查询视频
        /// </summary>
        /// <param name="classifyId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<VodVideo>> QueryVodVideoAsync(Guid? classifyId)
        {
            IEnumerable<VodVideo> enumerable = await vodVideoRepository.QueryVodVideoAsync(classifyId);
            return enumerable;
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
/*            List<VodVideoClassify> classifyList=await vodVideoClassifyRepository.FindVodVideoClassifyByClassifyNameAsync(vodVideoClassify.ClassifyName);
            if (classifyList.Count > 0) {
                return (false, "该视频分类名称已被使用");
            }*/
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
        /// 
        /// </summary>
        /// <param name="classifyType">分类类型</param>
        /// <returns></returns>
        public async Task<IEnumerable<VodVideoClassify>> QueryVodVideoClassifyAsync(ClassifyTypes? classifyType)
        {
            IEnumerable<VodVideoClassify> enumerable =await vodVideoClassifyRepository.QueryVodVideoClassifyAsync(classifyType);
            return enumerable;
        }
    }
}
